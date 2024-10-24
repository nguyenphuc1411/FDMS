using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs.Flight;
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
        [Authorize(Policy = "RequireAdmin")]
        [HttpPost]
        public async Task<IActionResult> CreateFlightt(FlightDTO flight)
        {
            var result = await _service.CreateNewFlight(flight);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Policy = "RequireAdmin")]
        [HttpPut("{flightID}")]
        public async Task<IActionResult> UpdateFlight(int flightID,FlightDTO flight)
        {
            var result = await _service.UpdateFlight(flightID,flight);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("current-flight")]
        public async Task<IActionResult> GetCurrentFlight()
        {
            var result = await _service.GetCurrentFlight();
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Policy = "RequireAdmin")]
        [HttpGet]
        public async Task<IActionResult> Get(string? search, string? flightNo, DateOnly? flightDate)
        {
            var result = await _service.Get(search, flightNo, flightDate);
            return StatusCode(result.StatusCode, result);
        }

        // API FOR USER
        [Authorize]
        [HttpGet("today-flight")]
        public async Task<IActionResult> GetTodayFlight()
        {
            var result = await _service.GetTodayFlight();
            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpGet("flightreported/{flightID}")]
        public async Task<IActionResult> GetFlightReported(int flightID)
        {
            var result = await _service.GetFlightReported(flightID);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpGet("getflight/{flightID}")]
        public async Task<IActionResult> GetFlight(int flightID)
        {
            var result = await _service.GetFlight(flightID);
            return StatusCode(result.StatusCode, result);
        }
    }
}
