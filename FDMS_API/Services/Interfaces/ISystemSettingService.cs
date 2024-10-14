﻿using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface ISystemSettingService
    {
        Task<APIResponse> UpdateSystemSetting(SystemSettingDTO systemSetting);
        Task<APIResponse> GetSystemSetting();
    }
}