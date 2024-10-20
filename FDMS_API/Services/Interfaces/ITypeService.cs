using FDMS_API.Models.DTOs.Type;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface ITypeService
    {
        Task<APIResponse> Create(TypeDTO requestModel);

        Task<APIResponse> Update(int typeID, TypeDTO requestModel);

        Task<APIResponse> Get(int? pageSize, int? currentPage);

        Task<APIResponse> GetByID(int typeID);
    }
}
