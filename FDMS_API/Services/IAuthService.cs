using FDMS_API.DTOs;

namespace FDMS_API.Services
{
    public interface IAuthService
    {
        Task<APIResponse<string>> Login(LoginDTO login);
    }
}
