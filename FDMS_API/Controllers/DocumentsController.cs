using FDMS_API.Models.DTOs.Document;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Drawing;

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
        [Authorize(Policy = "RequireAdmin")]
        [HttpGet]
        public async Task<IActionResult> Get(string? search, int? typeID,DateOnly? createdDate,int? pageSize,int? currentPage)
        {
            var result = await _service.Get(search,typeID, createdDate,pageSize,currentPage);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("recently")]
        public async Task<IActionResult> GetRecently(int? size)
        {
            var result = await _service.GetRecently(size);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Policy ="RequireAdmin")]
        [HttpPost]
        public async Task<IActionResult> Upload(AdminUploadDocument documentDTO)
        {
            var result = await _service.Upload(documentDTO);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("flight/{flightID}")]
        public async Task<IActionResult> GetDocumentByFlight(int flightID)
        {
            var result = await _service.GetDocumentByFlight(flightID);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Policy = "RequireAdmin")]
        [HttpGet("{documentID}")]
        public async Task<IActionResult> ViewDocs(int documentID)
        {
            var result = await _service.ViewDocs(documentID);
            return StatusCode(result.StatusCode, result);
        }



        //API FOR USER
        [Authorize(Policy = "RequireReadPermission")]
        [HttpGet("foruser/{documentID}")]
        public async Task<IActionResult> ViewDocsForUser(int documentID)
        {
            var result = await _service.ViewDocsForUser(documentID);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize(Policy = "RequireEditPermission")]
        [HttpPost("upload-version")]
        public async Task<IActionResult> UploadVersion(VersionDTO versionDTO)
        {
            var result = await _service.UploadVersion(versionDTO);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpPost("user-upload")]
        public async Task<IActionResult> UserUpload(UserUploadDocument model)
        {
            var result = await _service.UserUpload(model);
            return StatusCode(result.StatusCode, result);
        }
        [Authorize]
        [HttpDelete("{documentID}")]
        public async Task<IActionResult> Delete(int documentID)
        {
            var result = await _service.Delete(documentID);
            return StatusCode(result.StatusCode, result);
        }
    }
}
