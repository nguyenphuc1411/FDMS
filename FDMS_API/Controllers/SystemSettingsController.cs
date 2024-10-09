using FDMS_API.DTOs;
using FDMS_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Authorize(Roles ="Admin")]
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
        public async Task<IActionResult> CreateSystemSetting([FromForm] SystemSettingDTO systemSettingDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _service.CreateSystemSetting(systemSettingDTO);
                return StatusCode(result.StatusCode, result);
            }
            return BadRequest();
        }
        [HttpGet]
        public async Task<IActionResult> GetLatestSetting()
        {
            var result = await _service.GetLatestSetting();
            return StatusCode(result.StatusCode, result);
        }
    }
}
