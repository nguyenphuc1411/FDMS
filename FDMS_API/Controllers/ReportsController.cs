using FDMS_API.Models.DTOs;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly IReportService _service;

        public ReportsController(IReportService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Report(ReportDTO reportDTO)
        {
            var result = await _service.Report(reportDTO);
            return StatusCode(result.StatusCode,result);
        }
    }
}
