using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs.User;
using FDMS_API.Models.RequestModel;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IUserService
    {
        int GetUserId();
        Task<GetUser> GetCurrentUserDTO();
        Task<User> GetCurrentUser();
        Task<APIResponse> TerminateUser(List<int> userIDs);
        Task<APIResponse> RestoreAccess(List<int> userIDs);
        Task<APIResponse> GetUsers(int? pageSize, int? currentPage);
        Task<APIResponse> GetTerminatedUsers();
        Task<APIResponse> CreateUser(UserDTO userDTO);
        Task<APIResponse> UpdateUser(int userID,UserDTO userDTO);
        Task<APIResponse> ChangeOwner(ChangeOwner changeOwner);
    }
}
