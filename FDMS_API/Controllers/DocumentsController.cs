using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
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
            var result = await _service.Get(search, typeID, createdDate);
            return StatusCode(result.StatusCode, result);
        }
    }
}
