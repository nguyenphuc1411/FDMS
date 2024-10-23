using FDMS_API.Models.DTOs.Document;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IDocumentService
    {
        Task<APIResponse> Get(string? search,int? typeID,DateOnly? createdDate,int? pageSize,int? currentPage);
        Task<APIResponse> GetRecently(int? size);
        Task<APIResponse> Upload(AdminUploadDocument requestModel);
        Task<APIResponse> UploadVersion(VersionDTO requestModel);
        Task<APIResponse> UserUpload(UserUploadDocument requestModel);
        Task<APIResponse> ViewDocs(int documentID);
    }
}
