using FDMS_API.Data.Models;
using FDMS_API.Data;
using Microsoft.EntityFrameworkCore;
using FDMS_API.Services.Interfaces;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Implementations
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly AppDbContext _context;
        private readonly IFileUploadService _fileService;

        public SystemSettingService(AppDbContext context, IFileUploadService fileService)
        {
            _context = context;
            _fileService = fileService;
        }


        public async Task<ServiceResponse> GetSystemSetting()
        {
            var setting = await _context.SystemSettings.FirstOrDefaultAsync();
            if (setting == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Not found setting",
                    StatusCode = 404
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Message = "Get setting success",
                Data = setting,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> UpdateSystemSetting(SettingDTO systemSetting)
        {

            var settingCurrent = await _context.SystemSettings.FirstOrDefaultAsync();
            if (settingCurrent != null)
            {
                // Đã có thì xóa ảnh cũ và upload ảnh
                _fileService.DeleteImage(settingCurrent.LogoURL, "Images");
                // Upload ảnh mới
                var uploadResult = await _fileService.UploadImage(systemSetting.ImageFile, "Logo");

                if (uploadResult.Success)
                {
                    settingCurrent.Theme = systemSetting.Theme;
                    settingCurrent.LogoURL = uploadResult.FilePath.ToString();
                    settingCurrent.IsCaptchaRequired = systemSetting.IsCaptchaRequired;

                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return new ServiceResponse()
                        {
                            Success = true,
                            Message = "Updated setting",
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new ServiceResponse()
                        {
                            Success = false,
                            Message = "Update failed",
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    return new ServiceResponse()
                    {
                        Success = false,
                        Message = "An error while uploading",
                        StatusCode = 400
                    };
                }

            }
            else
            {
                // Upload ảnh mới
                var uploadResult = await _fileService.UploadImage(systemSetting.ImageFile, "Logo");

                if (uploadResult.Success)
                {
                    var setting = new Data.Models.SystemSetting
                    {
                        Theme = systemSetting.Theme,
                        LogoURL = uploadResult.FilePath.ToString(),
                        IsCaptchaRequired = systemSetting.IsCaptchaRequired
                    };
                    _context.SystemSettings.Add(setting);

                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return new ServiceResponse()
                        {
                            Success = true,
                            Message = "Created setting",
                            StatusCode = 200
                        };
                    }
                    else
                    {
                        return new ServiceResponse()
                        {
                            Success = false,
                            Message = "Create failed",
                            StatusCode = 400
                        };
                    }
                }
                else
                {
                    return new ServiceResponse()
                    {
                        Success = false,
                        Message = "An error while uploading",
                        StatusCode = 400
                    };
                }
            }
        }
    }
}
