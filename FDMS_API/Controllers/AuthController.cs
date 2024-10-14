using FDMS_API.Extentions;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.RequestModel;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FDMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO login)
        {        
            var response = await _service.Login(login);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(ForgotPassword request)
        {
            var response = await _service.RequestForgotPassword(request);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword request)
        {      
            var response = await _service.ResetPassword(request);
            return StatusCode(response.StatusCode, response);
        }


        [Authorize]
        [HttpPost("change-password")]
        public async Task<IActionResult> ChangePassword(ChangePassword request)
        {         
            var response = await _service.ChangePassword(request);
            return StatusCode(response.StatusCode, response);
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(string refreshToken)
        {
            var response = await _service.RefreshToken(refreshToken);
            return StatusCode(response.StatusCode,response);
        }
    }
}
