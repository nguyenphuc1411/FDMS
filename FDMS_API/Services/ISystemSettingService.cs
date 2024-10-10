using FDMS_API.Data.Models;
using FDMS_API.DTOs;

namespace FDMS_API.Services
{
    public interface ISystemSettingService
    {
        Task<APIResponse<string>> UpdateSystemSetting(SystemSettingDTO systemSetting);
        Task<APIResponse<SystemSetting>> GetSystemSetting();
    }
}
