using FDMS_API.Models.DTOs;
using FDMS_API.Models.RequestModel;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<APIResponse> Login(LoginDTO login);

        Task<APIResponse> RefreshToken(string refreshToken);

        Task<APIResponse> RequestForgotPassword(ForgotPassword request);

        Task<APIResponse> ResetPassword(ResetPassword request);

        Task<APIResponse> ChangePassword(ChangePassword request);
    }
}
