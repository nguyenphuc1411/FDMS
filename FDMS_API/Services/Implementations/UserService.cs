using AutoMapper;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs.Type;
using FDMS_API.Models.DTOs.User;
using FDMS_API.Models.RequestModel;
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
        public async Task<GetUser> GetCurrentUserDTO()
        {
            var userID = GetUserId();
            var user = await _context.Users.FindAsync(userID);
            return _mapper.Map<GetUser>(user);
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

        public async Task<APIResponse> GetUsers(int? pageSize, int? currentPage)
        {
            var users = await _context.Users.Where(x=>x.IsTerminated==false)
              .Select(t => new GetUser
              {
                  UserID = t.UserID,
                  Name = t.Name,
                  Email = t.Email,
                  Role = t.Role
              })
              .ToListAsync();

            if (pageSize.HasValue && currentPage.HasValue)
            {
                var pS = pageSize.Value;
                var cP = currentPage.Value;
                var paginatedResult = users.Pagination(pS, cP);

                return new APIResponse
                {
                    Success = true,
                    Message = "Get users success",
                    Data = paginatedResult,
                    StatusCode = 200
                };
            }
            return new APIResponse
            {
                Success = true,
                Message = "Get users success",
                Data = users,
                StatusCode = 200
            };
        }

        public async Task<APIResponse> GetTerminatedUsers()
        {
            var users = await _context.Users.Where(x => x.IsTerminated == true)
              .Select(t => new GetUser
              {
                  UserID = t.UserID,
                  Name = t.Name,
                  Email = t.Email,
                  Role = t.Role
              })
              .ToListAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Get terminated users success",
                Data = users,
                StatusCode = 200
            };
        }

        public async Task<APIResponse> CreateUser(UserDTO userDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var isExistsEmail = await _context.Users.FirstOrDefaultAsync(x => x.Email == userDTO.Email);
                if (isExistsEmail != null) return new APIResponse
                {
                    Success = false,
                    Message = "Email already exists in system",
                    StatusCode = 400
                };
                var newUser = _mapper.Map<User>(userDTO);
                newUser.Role = "USER";
                newUser.PasswordHash = "User@123".HashPassword();
                await _context.Users.AddAsync(newUser);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    if (userDTO.Groups.Count() > 0)
                    {
                        foreach (var item in userDTO.Groups)
                        {
                            await _context.GroupUsers.AddAsync(new GroupUser
                            {
                                UserID = newUser.UserID,
                                GroupID = item
                            });
                        }
                        var result1 = await _context.SaveChangesAsync();

                        if(result1 > 0)
                        {
                            await transaction.CommitAsync();
                            return new APIResponse
                            {
                                Success = true,
                                Message = "Create user success",
                                StatusCode = 201
                            };
                        }
                        await transaction.RollbackAsync();
                        return new APIResponse
                        {
                            Success = false,
                            Message = "Create user failed",
                            StatusCode = 400
                        };
                    }
                    await transaction.CommitAsync();
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Create user success",
                        StatusCode = 201
                    };
                }
                return new APIResponse
                {
                    Success = false,
                    Message = "Create user failed",
                    StatusCode = 400
                };

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new APIResponse
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<APIResponse> UpdateUser(int userID,UserDTO userDTO)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Users.FindAsync(userID);

                if (user == null)
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"Not found user have ID{userID}",
                        StatusCode = 404
                    };

                user.Name = userDTO.Name;
                user.Email = userDTO.Email;
                user.Phone = userDTO.Phone;
                _context.Users.Update(user);
                var currentUserGroups = await _context.GroupUsers.Where(x=>x.UserID==userID).ToListAsync();
                if (userDTO.Groups.Count() > 0)
                {
                    _context.GroupUsers.RemoveRange(currentUserGroups);
                    await _context.SaveChangesAsync();
                    foreach (var item in userDTO.Groups)
                    {
                        await _context.GroupUsers.AddAsync(new GroupUser
                        {
                            UserID = userID,
                            GroupID = item
                        });
                    }
                }
                else
                {
                    _context.GroupUsers.RemoveRange(currentUserGroups);
                }
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await transaction.CommitAsync();
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Update user success",
                        StatusCode = 200
                    };
                }
                await transaction.RollbackAsync();
                return new APIResponse
                {
                    Success = false,
                    Message = "Update user falied",
                    StatusCode = 400
                };

            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return new APIResponse
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };

            }
        }

        public async Task<APIResponse> ChangeOwner(ChangeOwner changeOwner)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                int currentOwnerID = GetUserId();
                var user = await _context.Users.FirstOrDefaultAsync(x => x.Email == changeOwner.Email);
                if (user == null)
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = $"Email {changeOwner.Email} is not exists in system",
                        StatusCode = 400
                    };
                }
                var currentOwner = await _context.Users.FindAsync(currentOwnerID);
                if (currentOwner != null)
                    if (currentOwner.Role == "ADMIN")
                    {
                        if (!changeOwner.ConfirmPassword.VerifyPassword(currentOwner.PasswordHash))
                        {
                            return new APIResponse
                            {
                                Success = false,
                                Message = "Confirm Password is incorrect",
                                StatusCode = 400
                            };
                        }
                        currentOwner.Role = "USER";
                        user.Role = "ADMIN";
                        _context.Users.Update(currentOwner);
                        _context.Users.Update(user);

                        var result = await _context.SaveChangesAsync();
                        if (result > 0)
                        {
                            await transaction.CommitAsync();
                            return new APIResponse
                            {
                                Success = true,
                                Message = "Change owner success",
                                StatusCode = 200
                            };
                        }
                        await transaction.RollbackAsync();
                        return new APIResponse
                        {
                            Success = false,
                            Message = "An Error",
                            StatusCode = 400
                        };
                    }

                return new APIResponse
                {
                    Success = false,
                    Message = "Not found owner",
                    StatusCode = 404
                };
            }
            catch(Exception ex)
            {
                await transaction.RollbackAsync();
                return new APIResponse
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }
    }
}
