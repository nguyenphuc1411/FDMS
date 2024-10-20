using AutoMapper;
using AutoMapper.QueryableExtensions;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.DTOs.Group;
using FDMS_API.Models.DTOs.User;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FDMS_API.Services.Implementations
{
    public class GroupService : IGroupService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public GroupService(AppDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<APIResponse> Create(GroupDTO requestModel)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                bool isExists = await _context.Groups.AnyAsync(x => x.GroupName == requestModel.GroupName);
                if (isExists) return new APIResponse
                {
                    Success = false,
                    Message = "GroupName is exists in system",
                    StatusCode = 400
                };
                var newGroup = new Group
                {
                    GroupName = requestModel.GroupName,
                    Note = requestModel.Note,
                    UserID = _userService.GetUserId(),
                };
                await _context.Groups.AddAsync(newGroup);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {               
                    if (requestModel.UserIDs.Count() > 0)
                    {
                        var listUserGroups = new List<GroupUser>();
                        foreach (var userID in requestModel.UserIDs)
                        {
                            await _context.GroupUsers.AddAsync(new GroupUser
                            {
                                UserID = userID,
                                GroupID = newGroup.GroupID
                            });
                        }
                        var result1 = await _context.SaveChangesAsync();
                        if (result1 > 0)
                        {
                            await transaction.CommitAsync();
                            return new APIResponse
                            {
                                Success = true,
                                Message = "Create group and group users success",
                                StatusCode = 201
                            };
                        }
                        else
                        {
                            return new APIResponse
                            {
                                Success = false,
                                Message = "An error",
                                StatusCode = 400
                            };
                        }
                    }
                    await transaction.CommitAsync();
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Create group success",
                        StatusCode = 201
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Creae Group failed",
                        StatusCode = 400
                    };
                }                               
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new APIResponse
                {
                    Success = false,
                    Message = "An error",
                    Data = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<APIResponse> Get(int? pageSize,int? currentPage)
        {
            var listGroup = await _context.Groups.ProjectTo<GetGroups>(_mapper.ConfigurationProvider).ToListAsync();
            if (pageSize.HasValue && currentPage.HasValue)
            {
               
                var pS = pageSize.Value;
                var cP = currentPage.Value;
                var paginatedResult = listGroup.Pagination(pS, cP);

                return new APIResponse
                {
                    Success = true,
                    Message = "Get groups success",
                    Data = paginatedResult,
                    StatusCode = 200
                };
            }
            return new APIResponse
            {
                Success = true,
                Message = "Get groups success",
                Data = listGroup,
                StatusCode = 200
            };
        }

        public async Task<APIResponse> GetByID(int groupID)
        {
            var group = await _context.Groups.Where(x=>x.GroupID==groupID)

                .Select(g =>new GetGroup
                {
                    GroupID = g.GroupID,
                    GroupName = g.GroupName,
                    Note = g.Note,
                    Users = g.GroupUsers.Select(gu=>new GetUser
                    {
                        UserID = gu.User.UserID,
                        Name = gu.User.Name,
                        Email = gu.User.Email,
                        Role = gu.User.Role
                    }).ToList()

                })

                .FirstOrDefaultAsync();
            if(group == null)
                return new APIResponse
                {
                    Success = false,
                    Message = "Not found group",
                    StatusCode = 404
                };
            return new APIResponse
            {
                Success = true,
                Message = "Get group success",
                Data = group,
                StatusCode = 200
            };
        }

        public async Task<APIResponse> Update(int groupID, GroupDTO requestModel)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var group = await _context.Groups.FindAsync(groupID);
                if (group == null)
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Not found group",
                        StatusCode = 404
                    };

                group.GroupName = requestModel.GroupName;
                group.Note = requestModel.Note;
                _context.Groups.Update(group);
                var newGroupUsers = requestModel.UserIDs;
                var currentGroupUsers = await _context.GroupUsers.Where(x => x.GroupID == groupID).ToListAsync();
                if (newGroupUsers.Count()>0)
                {                 
                    foreach (var item in currentGroupUsers)
                    {
                        // Xóa những bảng ghi không phải userID mới
                        if (!newGroupUsers.Contains(item.UserID))
                        {
                            _context.GroupUsers.Remove(item);
                        }
                    }
                    foreach (var item in newGroupUsers)
                    {
                        if(!currentGroupUsers.Any(x=>x.UserID == item))
                        {
                            var groupUser = new GroupUser
                            {
                                GroupID = groupID,
                                UserID = item
                            };
                            await _context.GroupUsers.AddAsync(groupUser);
                        }
                    }
                }
                else
                {
                    _context.GroupUsers.RemoveRange(currentGroupUsers);
                }
                var result =  await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await transaction.CommitAsync();
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Group updated successfully",
                        StatusCode = 200
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Group updated failed",
                        StatusCode = 400
                    };
                }
               
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }
    }
}
