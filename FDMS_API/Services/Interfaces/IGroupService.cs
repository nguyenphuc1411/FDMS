using FDMS_API.Models.DTOs.Group;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IGroupService
    {
        Task<ServiceResponse> Create(GroupDTO requestModel);

        Task<ServiceResponse> Update(int groupID,GroupDTO requestModel);

        Task<ServiceResponse> Get(int? pageSize,int? currentPage);

        Task<ServiceResponse> GetByID(int groupID);
    }
}
