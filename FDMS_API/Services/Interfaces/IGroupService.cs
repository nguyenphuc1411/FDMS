using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IGroupService
    {
        Task<APIResponse> Create(GroupDTO requestModel);

        Task<APIResponse> Update(int groupID,GroupDTO requestModel);

        Task<APIResponse> Get(int? pageSize,int? currentPage);

        Task<APIResponse> GetByID(int groupID);
    }
}
