using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IUserService
    {
        int GetUserId();
        Task<UserDTO> GetCurrentUserDTO();
        Task<User> GetCurrentUser();
        Task<APIResponse> TerminateUser(List<int> userIDs);
    }
}
