using FDMS_API.DTOs;
using FDMS_API.Extentions;
using FDMS_API.Services;

namespace FDMS_API.Repositories
{
    public class UploadImageService: IUploadImageService
    {
        private readonly IWebHostEnvironment _environment;

        public UploadImageService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void DeleteImage(string filePath)
        {
            string fullPath = Path.Combine(_environment.WebRootPath,"Images" , filePath);
            if(File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }

        public async Task<UploadImageResult> UploadImage(IFormFile file,string folder)
        {
            if (file.IsImage())
            {
                var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "Images",folder);

                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }

                var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName).ToLowerInvariant();
                var filePath = Path.Combine(uploadsFolderPath, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                return new UploadImageResult
                {
                    Success = true,
                    FilePath = folder+"/"+ uniqueFileName
                };
                   
            }
            return new UploadImageResult
            {
                Success = false,
            };
        }
    }
}
