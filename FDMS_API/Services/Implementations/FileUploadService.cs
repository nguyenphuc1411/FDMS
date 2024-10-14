using FDMS_API.Extentions;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;

namespace FDMS_API.Services.Implementations
{
    public class FileUploadService: IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public void DeleteImage(string filePath,string folder)
        {
            string fullPath = Path.Combine(_environment.WebRootPath, folder, filePath);
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }  

        public async Task<UploadFileResult> UploadDocument(IFormFile file, string folder)
        {
            if (file.IsValidFileFormat())
            {
                var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "Documents", folder);

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

                return new UploadFileResult
                {
                    Success = true,
                    FilePath = Path.Combine(folder, uniqueFileName)
                };

            }
            return new UploadFileResult
            {
                Success = false,
            };
        }

        public async Task<UploadFileResult> UploadImage(IFormFile file, string folder)
        {
            if (file.IsImage())
            {
                var uploadsFolderPath = Path.Combine(_environment.WebRootPath, "Images", folder);

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

                return new UploadFileResult
                {
                    Success = true,
                    FilePath = Path.Combine(folder, uniqueFileName)
                };

            }
            return new UploadFileResult
            {
                Success = false,
            };
        }

        public async Task<UploadFileResult> UploadMutipleDocument(List<IFormFile> files, string folder)
        {
            foreach (var file in files)
            {
                if (file.IsValidFileFormat())
                    return new UploadFileResult
                    {
                        Success = false,
                    };
            }
            var listFilePath = new List<string>();
            foreach (var file in files)
            {
                var fileExtention = Path.GetExtension(file.FileName);

                string newfileName = DateTime.Now.Ticks.ToString() + fileExtention;
                string filePath = Path.Combine(_environment.WebRootPath, "Images", folder, newfileName);

                // Kiểm tra xem thư mục đã tồn tại chưa, nếu chưa thì tạo
                if (!Directory.Exists(Path.Combine(_environment.WebRootPath, "Documents", folder)))
                {
                    Directory.CreateDirectory(Path.Combine(_environment.WebRootPath, "Documents", folder));
                }

                // Lưu file vào đường dẫn
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
                listFilePath.Add(Path.Combine(folder, newfileName));
            }
            return new UploadFileResult
            {
                Success = true,
                FilePath = listFilePath
            };
        }
    }
}
