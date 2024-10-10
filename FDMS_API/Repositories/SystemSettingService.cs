using FDMS_API.Data.Models;
using FDMS_API.Data;
using FDMS_API.DTOs;
using FDMS_API.Services;
using Microsoft.EntityFrameworkCore;

namespace FDMS_API.Repositories
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly AppDbContext _context;
        private readonly IUploadImageService _uploadImageService;
        public SystemSettingService(AppDbContext context ,IUploadImageService uploadImageService)
        {
            _context = context;
            _uploadImageService = uploadImageService;
        }

        public async Task<APIResponse<SystemSetting>> GetSystemSetting()
        {
            var setting = await _context.SystemSettings.FirstOrDefaultAsync();
            if(setting == null)
            {
                return new APIResponse<SystemSetting>
                {
                    Success = false,
                    Message = "Not found setting",
                    StatusCode = 404
                };
            }
            return new APIResponse<SystemSetting>
            {
                Success = true,
                Message = "Get setting success",
                Data = setting,
                StatusCode = 200
            };
        }

        public async Task<APIResponse<string>> UpdateSystemSetting(SystemSettingDTO systemSetting)
        {
            try
            {
                var settingCurrent = await _context.SystemSettings.FirstOrDefaultAsync();
                if (settingCurrent != null)
                {
                    // Đã có thì xóa ảnh cũ và upload ảnh
                    _uploadImageService.DeleteImage(settingCurrent.LogoURL);
                    // Upload ảnh mới
                    var uploadResult = await _uploadImageService.UploadImage(systemSetting.ImageFile, "Logo");

                    if(uploadResult.Success)
                    {
                        settingCurrent.Theme = systemSetting.Theme;
                        settingCurrent.LogoURL = uploadResult.FilePath;
                        settingCurrent.IsCaptchaRequired = systemSetting.IsCaptchaRequired;

                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            return new APIResponse<string>()
                            {
                                Success = true,
                                Message = "Updated setting",
                                StatusCode = 200
                            };
                        }
                        else
                        {
                            return new APIResponse<string>()
                            {
                                Success = false,
                                Message = "Update failed",
                                StatusCode = 400
                            };
                        }
                    }
                    else
                    {
                        return new APIResponse<string>()
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
                    var uploadResult = await _uploadImageService.UploadImage(systemSetting.ImageFile, "Logo");

                    if (uploadResult.Success)
                    {
                        var setting = new SystemSetting
                        {
                            Theme = systemSetting.Theme,
                            LogoURL = uploadResult.FilePath,
                            IsCaptchaRequired = systemSetting.IsCaptchaRequired
                        };
                        _context.SystemSettings.Add(setting);

                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            return new APIResponse<string>()
                            {
                                Success = true,
                                Message = "Created setting",
                                StatusCode = 200
                            };
                        }
                        else
                        {
                            return new APIResponse<string>()
                            {
                                Success = false,
                                Message = "Create failed",
                                StatusCode = 400
                            };
                        }
                    }
                    else
                    {
                        return new APIResponse<string>()
                        {
                            Success = false,
                            Message = "An error while uploading",
                            StatusCode = 400
                        };
                    }
                }
                
            }
            catch (Exception ex)
            {
                return new APIResponse<string>()
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }
    }
}
