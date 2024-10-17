using AutoMapper;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FDMS_API.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public UserService(IHttpContextAccessor httpContextAccessor, AppDbContext context, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
        }

        public async Task<APIResponse> TerminateUser(List<int> userIDs)
        {
            try
            {
                var users = await _context.Users.Where(x => userIDs.Contains(x.UserID)).ToListAsync();
                if (users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        user.IsTerminated = true;
                    }
                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        return new APIResponse
                        {
                            Success = true,
                            Message = "Terminated users success",
                            StatusCode = 200
                        };
                    }
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Terminate users failed",
                        StatusCode = 400
                    };

                }
                return new APIResponse
                {
                    Success = false,
                    Message = "Users is not exists",
                    StatusCode = 404
                };
            }
            catch (Exception ex)
            {
                return new APIResponse
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<APIResponse> RestoreAccess(List<int> userIDs)
        {
            var users = await _context.Users.Where(x => userIDs.Contains(x.UserID) && x.IsTerminated == true).ToListAsync();
            if (users.Count > 0)
            {
                foreach (var user in users)
                {
                    user.IsTerminated = false;
                }
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Restore access users success",
                        StatusCode = 200
                    };
                }
                return new APIResponse
                {
                    Success = false,
                    Message = "Restore users failed",
                    StatusCode = 400
                };

            }
            return new APIResponse
            {
                Success = false,
                Message = "Users is not exists",
                StatusCode = 404
            };

        }


        // Get user already login
        public async Task<UserDTO> GetCurrentUserDTO()
        {
            var userID = GetUserId();
            var user = await _context.Users.FindAsync(userID);
            return _mapper.Map<UserDTO>(user);
        }

        public int GetUserId()
        {
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }
        public async Task<User> GetCurrentUser()
        {
            var userID = GetUserId();
            var user = await _context.Users.FindAsync(userID);
            return user;
        }


    }
}
