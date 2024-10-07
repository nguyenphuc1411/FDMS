using FDMS_API.DTOs;

namespace FDMS_API.Services
{
    public interface IUploadImageService
    {
        Task<UploadImageResult> UploadImage(IFormFile file,string folder);
    }
}
