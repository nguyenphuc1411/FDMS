using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Authorize(Policy ="RequireAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentsController : ControllerBase
    {
        private readonly IDocumentService _service;

        public DocumentsController(IDocumentService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get(string? search, int? typeID,DateOnly? createdDate,int? pageSize,int? currentPage)
        {
            var result = await _service.Get(search,typeID, createdDate,pageSize,currentPage);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("recently")]
        public async Task<IActionResult> GetRecently(int? size)
        {
            var result = await _service.GetRecently(size);
            return StatusCode(result.StatusCode, result);
        }
    }
}
