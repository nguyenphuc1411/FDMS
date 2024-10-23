using FDMS_API.Models.DTOs;
using FDMS_API.Models.RequestModel;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ServiceResponse> Login(LoginDTO login);

        Task<ServiceResponse> RefreshToken(string refreshToken);

        Task<ServiceResponse> RequestForgotPassword(ForgotPassword request);

        Task<ServiceResponse> ResetPassword(ResetPassword request);

        Task<ServiceResponse> ChangePassword(ChangePassword request);
    }
}
