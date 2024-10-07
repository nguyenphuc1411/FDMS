using FDMS_API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }

        [Authorize(Roles ="Admin")]
        [HttpPost("terminate")]
        public async Task<IActionResult> TerminateUsers([FromBody]List<int> userIDs )
        {
            if(userIDs.Count == 0)
            {
                return BadRequest("List UserID is required");
            }
            var result = await _service.TerminateUser(userIDs);
            return StatusCode(result.StatusCode, result);
        }
    }
}
