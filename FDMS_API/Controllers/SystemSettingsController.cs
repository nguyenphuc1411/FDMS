using FDMS_API.Models.DTOs;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SystemSettingsController : ControllerBase
    {
        private readonly ISystemSettingService _service;

        public SystemSettingsController(ISystemSettingService service)
        {
            _service = service;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> UpdateSystemSetting([FromForm] SystemSettingDTO systemSettingDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.UpdateSystemSetting(systemSettingDTO);
                return StatusCode(result.StatusCode, result);
            }
            return BadRequest();
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetSetting()
        {
            var result = await _service.GetSystemSetting();
            return StatusCode(result.StatusCode, result);
        }
        
    }
}
