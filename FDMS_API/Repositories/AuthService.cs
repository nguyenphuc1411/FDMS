using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.DTOs;
using FDMS_API.Extentions;
using FDMS_API.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FDMS_API.Repositories
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _dbContext;
        private readonly IConfiguration _config;
        public AuthService(AppDbContext dbContext, IConfiguration config)
        {
            _dbContext = dbContext;
            _config = config;
        }

        public async Task<APIResponse<string>> Login(LoginDTO login)
        {
            try
            {
                var user = await _dbContext.Users.FirstOrDefaultAsync(u=>u.Email == login.Email);
                if (user == null)
                {
                    return new APIResponse<string>()
                    {
                        Success = false,
                        Message = "The user is not exists",
                        Data = null,
                        StatusCode = 404
                    };
                }
                else
                {
                    if (user.IsTerminated == true)
                    {
                        return new APIResponse<string>()
                        {
                            Success = false,
                            Message = "This account is terminated",
                            Data = null,
                            StatusCode = 403  // Forbidden
                        };
                    }
                    if (login.Password.VerifyPassword(user.PasswordHash))
                    {                     
                        string token = GenerateToken(user, _config["JWT:Key"], _config["JWT:Issuer"], _config["JWT:Audience"]);
                        return new APIResponse<string>()
                        {
                            Success = true,
                            Message = "Login success",
                            Data =token,
                            StatusCode = 200
                        };
                    }                
                    else
                    {
                        return new APIResponse<string>()
                        {
                            Success = false,
                            Message = "Email or Password incorrect",
                            Data = null,
                            StatusCode = 400
                        };
                    }
                }
            }
            // Xử lý ngoại lệ
            catch (Exception ex)
            {
                return new APIResponse<string>()
                {
                    Success = false,
                    Message = ex.Message,
                    Data = null,
                    StatusCode = 500
                };
            }
        }

        public static string GenerateToken(User user,string JWTkey,string issuer,string audience)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience:audience,
                claims: claims,
                expires: DateTime.UtcNow.AddSeconds(20),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
