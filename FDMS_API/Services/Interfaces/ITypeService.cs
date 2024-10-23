using FDMS_API.Models.DTOs.Type;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface ITypeService
    {
        Task<ServiceResponse> Create(TypeDTO requestModel);

        Task<ServiceResponse> Update(int typeID, TypeDTO requestModel);

        Task<ServiceResponse> Get(int? pageSize, int? currentPage);

        Task<ServiceResponse> GetByID(int typeID);
    }
}
