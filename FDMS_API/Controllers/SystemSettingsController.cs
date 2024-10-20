using FDMS_API.Models.DTOs;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingService _service;

        public SystemSettingsController(ISystemSettingService service)
        {
            _service = service;
        }
        [HttpPost]
        public async Task<IActionResult> UpdateSystemSetting([FromForm] SettingDTO systemSettingDTO)
        {
            var result = await _service.UpdateSystemSetting(systemSettingDTO);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet]
        public async Task<IActionResult> GetSetting()
        {
            var result = await _service.GetSystemSetting();
            return StatusCode(result.StatusCode, result);
        }

    }
}
