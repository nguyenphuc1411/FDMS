using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FlightsController : ControllerBase
    {
        private readonly IFlightService _service;

        public FlightsController(IFlightService service)
        {
            _service = service;
        }
        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateFlightt(FlightDTO flight)
        {
            var response = await _service.CreateNewFlight(flight);
            return StatusCode(response.StatusCode, response);
        }
        [Authorize]
        [HttpGet("current-flight")]
        public async Task<IActionResult> GetCurrentFlight()
        {
            var response = await _service.GetCurrentFlight();
            return StatusCode(response.StatusCode, response);
        }
        [Authorize(Roles ="Admin")]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var response = await _service.Get();
            return StatusCode(response.StatusCode, response);
        }
    }
}
