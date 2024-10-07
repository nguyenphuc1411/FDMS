using FDMS_API.DTOs;

namespace FDMS_API.Services
{
    public interface IUserService
    {
        int GetUserId();
        Task<UserDTO> GetCurrentUser();
        Task<APIResponse<string>> TerminateUser(List<int> userIDs);
    }
}
