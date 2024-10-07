using AutoMapper;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.DTOs;
using FDMS_API.Services;
using Microsoft.EntityFrameworkCore;

namespace FDMS_API.Repositories
{
    public class SystemSettingService : ISystemSettingService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IUploadImageService _uploadImageService;
        public SystemSettingService(AppDbContext context, IUserService userService, IUploadImageService uploadImageService)
        {
            _context = context;
            _userService = userService;
            _uploadImageService = uploadImageService;
        }

        public async Task<APIResponse<string>> CreateSystemSetting(SystemSettingDTO systemSetting)
        {
            int currentUserID = _userService.GetUserId();

            try
            {
                var uploadResult = await _uploadImageService.UploadImage(systemSetting.ImageFile, "Settings");
                if (uploadResult.Success)
                {
                    var ss = new SystemSetting
                    {
                        UserID = currentUserID,
                        Theme = systemSetting.Theme,
                        LogoURL = uploadResult.FilePath,
                        IsCaptchaRequired = systemSetting.IsCaptchaRequired,
                        Updated_At = DateTime.Now
                    };
                    _context.SystemSettings.Add(ss);
                    var result = await _context.SaveChangesAsync();
                    if(result > 0)
                    {
                        return new APIResponse<string>()
                        {
                            Success = true,
                            Message = "Created system setting success",                           
                            StatusCode = 201
                        };
                    }
                    return new APIResponse<string>()
                    {
                        Success = false,
                        Message = "Create system setting failed",
                        StatusCode = 400
                    };
                }
                else
                {
                    return new APIResponse<string>()
                    {
                        Success = false,
                        Message = "Upload Image failed",
                        StatusCode = 400
                    };
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

        public async Task<APIResponse<object>> GetLatestSetting()
        {
            try
            {
                var latestSetting = await _context.SystemSettings
                                   .OrderByDescending(x => x.Updated_At)
                                   .FirstOrDefaultAsync();
                if (latestSetting != null)
                {
                    var data = new
                    {
                        Theme = latestSetting.Theme,
                        LogoURL = latestSetting.LogoURL,
                        IsCaptchaRequired = latestSetting.IsCaptchaRequired
                    };
                    return new APIResponse<object>()
                    {
                        Success = true,
                        Message = "Get lasted setting success",
                        Data = data,
                        StatusCode = 200
                    };
                }
                return new APIResponse<object>()
                {
                    Success = false,
                    Message = "Not found",
                    StatusCode = 404
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<object>()
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }
    }
}
