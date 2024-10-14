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
        public async Task<IActionResult> CreateFlightt(CreateFlightDTO flight)
        {
            var response = await _service.CreateNewFlight(flight);
            return StatusCode(response.StatusCode, response);
        }
    }
}
