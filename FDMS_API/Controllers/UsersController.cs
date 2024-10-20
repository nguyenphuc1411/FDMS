﻿using FDMS_API.Models.DTOs.User;
using FDMS_API.Models.RequestModel;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FDMS_API.Controllers
{
    [Authorize(Policy = "RequireAdmin")]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            _service = service;
        }
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
        [HttpPost("restore-access")]
        public async Task<IActionResult> RestoreAccess([FromBody] List<int> userIDs)
        {
            if (userIDs.Count == 0)
            {
                return BadRequest("List UserID is required");
            }
            var result = await _service.RestoreAccess(userIDs);
            return StatusCode(result.StatusCode, result);
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers(int? pageSize,int? currentPage)
        {
            var result = await _service.GetUsers(pageSize, currentPage);
            return StatusCode(result.StatusCode, result);
        }
        [HttpGet("terminated")]
        public async Task<IActionResult> GetTerminatedUsers()
        {
            var result = await _service.GetTerminatedUsers();
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(UserDTO userDTO)
        {
            var result = await _service.CreateUser(userDTO);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPut("{userID}")]
        public async Task<IActionResult> UpdateUser(int userID,UserDTO userDTO)
        {
            var result = await _service.UpdateUser(userID,userDTO);
            return StatusCode(result.StatusCode, result);
        }
        [HttpPost("change-owner")]
        public async Task<IActionResult> ChangeOwner(ChangeOwner changeOwner)
        {
            var result = await _service.ChangeOwner(changeOwner);
            return StatusCode(result.StatusCode, result);
        }
    }
}
