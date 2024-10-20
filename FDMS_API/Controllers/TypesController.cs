using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs.Type;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class TypesController : ControllerBase
    {
        private readonly ITypeService _service;

        public TypesController(ITypeService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int? pageSize, int? currentPage)
        {
            var result = await _service.Get(pageSize, currentPage);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{typeID}")]
        public async Task<IActionResult> GetByID(int typeID)
        {
            var result = await _service.GetByID(typeID);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TypeDTO typeDTO)
        {
            var result = await _service.Create(typeDTO);
            return StatusCode(result.StatusCode,result);
        }
        [HttpPut("{typeID}")]
        public async Task<IActionResult> Update(int typeID,TypeDTO typeDTO)
        {
            var result = await _service.Update(typeID,typeDTO);
            return StatusCode(result.StatusCode, result);
        }
    }
}
