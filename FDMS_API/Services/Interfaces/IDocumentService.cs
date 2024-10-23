using FDMS_API.Models.DTOs.Document;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<ServiceResponse> Get(string? search,int? typeID,DateOnly? createdDate,int? pageSize,int? currentPage);
        Task<ServiceResponse> GetRecently(int? size);
        Task<ServiceResponse> Upload(AdminUploadDocument requestModel);
        Task<ServiceResponse> UploadVersion(VersionDTO requestModel);
        Task<ServiceResponse> UserUpload(UserUploadDocument requestModel);
        Task<ServiceResponse> ViewDocs(int documentID);
    }
}
