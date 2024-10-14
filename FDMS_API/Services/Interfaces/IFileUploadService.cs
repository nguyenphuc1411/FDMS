using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IFileUploadService
    {
        Task<UploadFileResult> UploadImage(IFormFile file, string folder);
        void DeleteImage(string filePath,string folder);

        Task<UploadFileResult> UploadDocument(IFormFile file,string folder);
        Task<UploadFileResult> UploadMutipleDocument(List<IFormFile> files, string folder);
    }
}
