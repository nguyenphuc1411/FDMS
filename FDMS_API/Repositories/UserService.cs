using AutoMapper;
using FDMS_API.Data;
using FDMS_API.DTOs;
using FDMS_API.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace FDMS_API.Repositories
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

        public async Task<UserDTO> GetCurrentUser()
        {
            var userID = GetUserId();
            var user = await _context.Users.FindAsync(userID);
            return _mapper.Map<UserDTO>(user);
        }

        public int GetUserId()
        {        
            return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
        }

        public async Task<APIResponse<string>> TerminateUser(List<int> userIDs)
        {
            try
            {
                var users = await _context.Users.Where(x=>userIDs.Contains(x.UserID)).ToListAsync();
                if(users.Count > 0)
                {
                    foreach (var user in users)
                    {
                        user.IsTerminated = true;
                    }
                    var result = await _context.SaveChangesAsync();
                    if(result > 0)
                    {
                        return new APIResponse<string>
                        {
                            Success = true,
                            Message = "Terminated users success",
                            StatusCode = 200
                        };
                    }
                    return new APIResponse<string>
                    {
                        Success = false,
                        Message = "Terminate users failed",
                        StatusCode = 400
                    };

                }
                return new APIResponse<string>
                {
                    Success = false,
                    Message = "Users is not exists",
                    StatusCode = 404
                };
            }
            catch (Exception ex)
            {
                return new APIResponse<string>
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }
    }
}
