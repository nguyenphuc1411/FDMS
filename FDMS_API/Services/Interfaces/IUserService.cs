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
        Task<ServiceResponse> TerminateUser(List<int> userIDs);
        Task<ServiceResponse> RestoreAccess(List<int> userIDs);
        Task<ServiceResponse> GetUsers(int? pageSize, int? currentPage);
        Task<ServiceResponse> GetTerminatedUsers();
        Task<ServiceResponse> CreateUser(UserDTO userDTO);
        Task<ServiceResponse> UpdateUser(int userID,UserDTO userDTO);
        Task<ServiceResponse> ChangeOwner(ChangeOwner changeOwner);
    }
}
