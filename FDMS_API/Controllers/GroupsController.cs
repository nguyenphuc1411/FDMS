using FDMS_API.Models.DTOs.Group;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
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
            var result = await _service.Get(pageSize, currentPage);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> Create(GroupDTO requestModel)
        {
            var result = await _service.Create(requestModel);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("{groupID}")]
        public async Task<IActionResult> GetByID(int groupID)
        {
            var result = await _service.GetByID(groupID);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("{groupID}")]
        public async Task<IActionResult> Update(int groupID,GroupDTO requestModel)
        {
            var result = await _service.Update(groupID,requestModel);
            return StatusCode(result.StatusCode, result);
        }
    }
}
