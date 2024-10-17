using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Authorize(Roles = "Admin")]
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
            var response = await _service.Get(pageSize, currentPage);
            return StatusCode(response.StatusCode, response);
        }
        [HttpGet("{typeID}")]
        public async Task<IActionResult> GetByID(int typeID)
        {
            var response = await _service.GetByID(typeID);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPost]
        public async Task<IActionResult> Create(TypeDTO typeDTO)
        {
            var response = await _service.Create(typeDTO);
            return StatusCode(response.StatusCode,response);
        }
        [HttpPut("{typeID}")]
        public async Task<IActionResult> Update(int typeID,TypeDTO typeDTO)
        {
            var response = await _service.Update(typeID,typeDTO);
            return StatusCode(response.StatusCode, response);
        }
    }
}
