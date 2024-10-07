using FDMS_API.DTOs;

namespace FDMS_API.Services
{
    public interface ISystemSettingService
    {
        Task<APIResponse<string>> CreateSystemSetting(SystemSettingDTO systemSetting);
        Task<APIResponse<object>> GetLatestSetting();
    }
}
