using FDMS_API.Models.DTOs;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly IGroupService _service;

        public GroupsController(IGroupService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetListGroup(int? pageSize,int? currentPage)
        {
            var response = await _service.Get(pageSize, currentPage);
            return StatusCode(response.StatusCode,response);
        }

        [Authorize(Roles ="Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(GroupDTO requestModel)
        {
            var response = await _service.Create(requestModel);
            return StatusCode(response.StatusCode,response);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{groupID}")]
        public async Task<IActionResult> GetByID(int groupID)
        {
            var response = await _service.GetByID(groupID);
            return StatusCode(response.StatusCode, response);
        }
        [HttpPut("{groupID}")]
        public async Task<IActionResult> Update(int groupID,GroupDTO requestModel)
        {
            var response = await _service.Update(groupID,requestModel);
            return StatusCode(response.StatusCode, response);
        }
    }
}
