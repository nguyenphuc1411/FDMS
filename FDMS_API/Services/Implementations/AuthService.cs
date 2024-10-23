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

        public async Task<ServiceResponse> Login(LoginDTO login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user == null)
            {
                return new ServiceResponse
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
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "This account is terminated",
                        StatusCode = 403  // Forbidden
                    };
                }
                if (login.Password.VerifyPassword(user.PasswordHash))
                {
                    // Kiểm tra nếu token có rồi thì cập nhật nếu chưa thì tạo

                    var refreshTokenInSystem = await _context.UserTokens.FirstOrDefaultAsync(x => x.UserID == user.UserID && x.TokenType == "REFRESH TOKEN");
                    var token = new TokenResponse();
                    DateTime expirationToken = DateTime.Now.AddDays(1);

                    if (refreshTokenInSystem != null)
                    {
                        // đã tồn tại token cập nhật thời gian hết hạn
                        refreshTokenInSystem.ExpirationDate = expirationToken;
                        _context.UserTokens.Update(refreshTokenInSystem);
                        token.AccessToken = GenerateToken(user.UserID.ToString(), _config["JWT:Key"]);
                        token.RefreshToken = refreshTokenInSystem.Token;
                        token.AccessTokenExpiration = expirationToken;
                    }
                    else
                    {
                        var refreshToken = GenerateRefreshToken();
                        token.AccessToken = GenerateToken(user.UserID.ToString(), _config["JWT:Key"]);
                        token.RefreshToken = refreshToken;
                        token.AccessTokenExpiration = expirationToken;                     
                        // Lưu Refresh token vào database
                        var newUserToken = new UserToken
                        {
                            Token = refreshToken,
                            TokenType = "REFRESH TOKEN",
                            ExpirationDate = expirationToken,
                            UserID = user.UserID,
                            Email = user.Email
                        };
                        await _context.UserTokens.AddAsync(newUserToken);
                    }              
                    await _context.SaveChangesAsync();
                    return new ServiceResponse
                    {
                        Success = true,
                        Message = "Login success",
                        Data = token,
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Email or Password incorrect",
                        StatusCode = 400
                    };
                }
            }
        }

        public async Task<ServiceResponse> RefreshToken(string refreshToken)
        {
            var userToken = await _context.UserTokens.FirstOrDefaultAsync(x => x.Token == refreshToken
            && x.TokenType == "REFRESH TOKEN" && x.ExpirationDate >= DateTime.Now);
            if(userToken != null)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x=>x.UserID == userToken.UserID);
                var token = new TokenResponse
                {
                    AccessToken = GenerateToken(user.UserID.ToString(), _config["JWT:Key"]),
                    RefreshToken = refreshToken,
                    AccessTokenExpiration = userToken.ExpirationDate
                };
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Refresh token success",
                    Data = token,
                    StatusCode = 200
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "Unauthorized",
                StatusCode = 401
            };
        }

        public async Task<ServiceResponse> RequestForgotPassword(ForgotPassword request)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == request.Email);
            if (user == null)
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Email is not exists in system",
                    StatusCode = 400
                };
            // Tạo token mới
            var newToken = GenerateToken(user.UserID.ToString(), _config["JWT:Key"]);
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
                        return new ServiceResponse
                        {
                            Success = true,
                            Message = "Send mail success",
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new ServiceResponse
                        {
                            Success = false,
                            Message = "Something went wrong, please try again!",
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    return new ServiceResponse
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
                return new ServiceResponse
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
                    return new ServiceResponse
                    {
                        Success = true,
                        Message = "Send mail success",
                        StatusCode = 200
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Something went wrong, please try again!",
                        StatusCode = 400
                    };
                }
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Send mail failed, please try again!",
                    StatusCode = 400
                };
            }
        }      

        public async Task<ServiceResponse> ResetPassword(ResetPassword request)
        {
            var userToken = await _context.UserTokens.FirstOrDefaultAsync(x => x.TokenType == "FORGOT PASSWORD"
            && x.ExpirationDate > DateTime.Now && x.Token == request.Token && x.Email == request.Email && x.IsUsed == false);

            if (userToken == null)
                return new ServiceResponse
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
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Reset password success",
                    StatusCode = 200
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "Reset password failed, please try again",
                StatusCode = 400
            };
        }

        public async Task<ServiceResponse> ChangePassword(ChangePassword request)
        {
            var user = await _userService.GetCurrentUser();

            bool isCorrectPassword = request.OldPassword.VerifyPassword(user.PasswordHash);

            if (isCorrectPassword)
            {
                user.PasswordHash = request.NewPassword.HashPassword();
                var result = await _context.SaveChangesAsync(); 
                if (result>0)
                {
                    return new ServiceResponse
                    {
                        Success = true,
                        Message = "Change password success",
                        StatusCode = 200
                    };
                }
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Change password failed, please try again",
                    StatusCode = 400
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "Change password failed, please try again",
                StatusCode = 400
            };
        }


        public static string GenerateToken(string userID,string JWTkey)
        {
            var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,userID)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JWTkey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var tokenDesciption = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(1),
                SigningCredentials = creds
            };
            var token = jwtSecurityTokenHandler.CreateToken(tokenDesciption);
            return jwtSecurityTokenHandler.WriteToken(token);
        }

        public static string GenerateRefreshToken()
        {
            return Guid.NewGuid().ToString();
        }

        public static string GenerateForgotPasswordEmailBody(string resetPasswordLink, string fullName)
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
    }
}
