using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<APIResponse> Get(string? search,int? typeID,DateOnly? createdDate,int? pageSize,int? currentPage);
    }
}
