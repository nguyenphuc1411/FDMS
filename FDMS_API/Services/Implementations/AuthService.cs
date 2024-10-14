using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.RequestModel;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FDMS_API.Services.Implementations
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;
        private readonly IUserService _userService;
        public AuthService(AppDbContext dbContext, IConfiguration config, IMailService mailService, IUserService userService)
        {
            _context = dbContext;
            _config = config;
            _mailService = mailService;
            _userService = userService;
        }

        public async Task<APIResponse> Login(LoginDTO login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user == null)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "The user is not exists",
                    StatusCode = 404
                };
            }
            else
            {
                if (user.IsTerminated == true)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "This account is terminated",
                        StatusCode = 403  // Forbidden
                    };
                }
                if (login.Password.VerifyPassword(user.PasswordHash))
                {
                    string token = GenerateToken(user, _config["JWT:Key"]);
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Login success",
                        Data = token,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Email or Password incorrect",
                        StatusCode = 400
                    };
                }
            }
        }

        public static string GenerateToken(User user, string JWTkey)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.UserID.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role,user.Role)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDesciption = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddSeconds(30),
                SigningCredentials = creds
            };
            var token = jwtSecurityTokenHandler.CreateToken(tokenDesciption);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public async Task<APIResponse> RequestForgotPassword(ForgotPassword request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return new APIResponse
                {
                    Success = false,
                    Message = "Email is not exists in system",
                    StatusCode = 400
                };
            // Tạo token mới
            var newToken = GenerateToken(user, _config["JWT:Key"]);
            // Tạo resetLink
            string resetPasswordLink = $"{"linkfrontend"}/reset-password?email={user.Email}&token={newToken}";
            // Check token hệ thống
            var tokenusers = await _context.UserTokens.Where(x => x.UserID == user.UserID && x.TokenType == "FORGOT PASSWORD").ToListAsync();
            if (tokenusers.Count == 0)
            {
                // Chưa có trong hệ thống và xác nhận gửi mail

                var mailRequest = new MailRequest
                {
                    ToEmail = request.Email,
                    Subject = "Reset Password Confirm",
                    Body = GenerateForgotPasswordEmailBody(resetPasswordLink, user.Name)
                };

                var isSendMailSuccess = await _mailService.SendEmailAsync(mailRequest);

                if (isSendMailSuccess)
                {
                    // Lưu thông tin token vào database
                    var userToken = new UserToken
                    {
                        Token = newToken,
                        TokenType = "FORGOT PASSWORD",
                        Email = user.Email,
                        UserID = user.UserID
                    };
                    _context.UserTokens.Add(userToken);
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return new APIResponse
                        {
                            Success = true,
                            Message = "Send mail success",
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new APIResponse
                        {
                            Success = false,
                            Message = "Something went wrong, please try again!",
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Send mail failed, please try again!",
                        StatusCode = 400
                    };
                }
            }

            // Đã có token 

            var newestToken = tokenusers.OrderByDescending(x => x.ExpirationDate).FirstOrDefault();
            if (newestToken.ExpirationDate > DateTime.Now)
            {
                var remainTime = (int)Math.Ceiling((newestToken.ExpirationDate - DateTime.Now).TotalMinutes);
                // Chưa hết hạn token cũ
                return new APIResponse
                {
                    Success = false,
                    Message = $"Already send mail, try again after {remainTime} minutes!",
                    StatusCode = 400
                };
            }

            var mailRequest1 = new MailRequest
            {
                ToEmail = request.Email,
                Subject = "Reset Password Confirm",
                Body = GenerateForgotPasswordEmailBody(resetPasswordLink, user.Name)
            };

            var isSendMailSuccess1 = await _mailService.SendEmailAsync(mailRequest1);

            if (isSendMailSuccess1)
            {
                // Lưu thông tin token vào database
                var userToken1 = new UserToken
                {
                    Token = newToken,
                    TokenType = "FORGOT PASSWORD",
                    Email = user.Email,
                    UserID = user.UserID
                };
                _context.UserTokens.Add(userToken1);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Send mail success",
                        StatusCode = 200
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Something went wrong, please try again!",
                        StatusCode = 400
                    };
                }
            }
            else
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Send mail failed, please try again!",
                    StatusCode = 400
                };
            }
        }

        // Hàm tạo body email forgot password
        public string GenerateForgotPasswordEmailBody(string resetPasswordLink, string fullName)
        {
            // Tạo nội dung email với HTML
            string emailBody = $@"
               <html>
        <head>
            <style>
                body {{
                    font-family: Arial, sans-serif;
                    background-color: #f4f4f4;
                    margin: 0;
                    padding: 0;
                }}
                .container {{
                    max-width: 600px;
                    margin: 50px auto;
                    background-color: #ffffff;
                    border-radius: 8px;
                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                    padding: 20px;
                }}
                h2 {{
                    color: #333;
                }}
                p {{
                    font-size: 16px;
                    line-height: 1.5;
                }}
                a {{
                    display: inline-block;
                    background-color: #007bff;
                    color: white;
                    padding: 10px 20px;
                    text-decoration: none;
                    border-radius: 5px;
                    margin-top: 20px;
                    font-weight: bold;
                }}
                a:hover {{
                    background-color: #0056b3;
                }}
                .footer {{
                    margin-top: 20px;
                    font-size: 14px;
                    color: #777;
                }}
            </style>
        </head>
        <body>
            <div class='container'>
                <h2>Hello {fullName},</h2>
                <p>You have requested to reset your password. This link will expire in 5 minutes. Please click the button below to reset your password:</p>
                <a style={"color:white"} href='{resetPasswordLink}' target='_blank'>Reset Password</a>
                <p class='footer'>If you did not request a password reset, please ignore this email.</p>
                <p class='footer'>Thank you!</p>
            </div>
        </body>
        </html>

            ";

            return emailBody;
        }

        public async Task<APIResponse> ResetPassword(ResetPassword request)
        {
            var userToken = await _context.UserTokens.FirstOrDefaultAsync(x => x.TokenType == "FORGOT PASSWORD"
            && x.ExpirationDate > DateTime.Now && x.Token == request.Token && x.Email == request.Email && x.IsUsed == false);

            if (userToken == null)
                return new APIResponse
                {
                    Success = false,
                    Message = "Invalid request",
                    StatusCode = 400
                };

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == request.Email);

            var newPasswordHash = request.NewPassword.HashPassword();
            user.PasswordHash = newPasswordHash;
            var result = await _context.SaveChangesAsync();
            if (result>0)
            {
                userToken.IsUsed = true;
                _context.UserTokens.Update(userToken);
                await _context.SaveChangesAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = "Reset password success",
                    StatusCode = 200
                };
            }
            return new APIResponse
            {
                Success = false,
                Message = "Reset password failed, please try again",
                StatusCode = 400
            };
        }

        public async Task<APIResponse> ChangePassword(ChangePassword request)
        {
            var user = await _userService.GetCurrentUser();

            bool isCorrectPassword = request.OldPassword.VerifyPassword(user.PasswordHash);

            if (isCorrectPassword)
            {
                user.PasswordHash = request.NewPassword.HashPassword();
                var result = await _context.SaveChangesAsync(); 
                if (result>0)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Change password success",
                        StatusCode = 200
                    };
                }
                return new APIResponse
                {
                    Success = true,
                    Message = "Change password failed, please try again",
                    StatusCode = 400
                };
            }
            return new APIResponse
            {
                Success = false,
                Message = "Change password failed, please try again",
                StatusCode = 400
            };
        }
    }
}
