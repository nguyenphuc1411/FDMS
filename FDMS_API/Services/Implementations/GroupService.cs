using AutoMapper;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs;
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
        public GroupService(AppDbContext context, IUserService userService)
        {
            _context = context;
            _userService = userService;
        }

        public async Task<APIResponse> Create(GroupDTO requestModel)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newGroup = new Group
                {
                    GroupName = requestModel.GroupName,
                    Note = requestModel.Note,
                    UserID = _userService.GetUserId(),
                };
                await _context.Groups.AddAsync(newGroup);
                await _context.SaveChangesAsync();
                var listUserGroups = new List<GroupUser>();
                if (requestModel.UserIDs != null && requestModel.UserIDs.Count()>0)
                    foreach (var userID in requestModel.UserIDs)
                    {
                        await _context.GroupUsers.AddAsync(new GroupUser
                        {
                            UserID = userID,
                            GroupID = newGroup.GroupID
                        });
                    }

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
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
                        Message = "An error",
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
            var listGroup = await _context.Groups
              .Select(x => new GetGroup
              {
                  GroupID = x.GroupID,
                  GroupName = x.GroupName,
                  Note = x.Note,
                  CreatedAt = x.Created_At,
                  Creator = x.User.Email,
                  Users = x.GroupUsers.Select(gu => new UserDTO
                  {
                      UserID = gu.UserID,
                      Name = gu.User.Name,
                      Email = gu.User.Email,
                      Phone = gu.User.Phone,
                      Role = gu.User.Role
                  }).ToList(),
              }).ToListAsync();
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
                    CreatedAt = g.Created_At,
                    Creator = g.User.Email,
                    Users = g.GroupUsers.Select(gu=>new UserDTO
                    {
                        UserID = gu.User.UserID,
                        Name = gu.User.Name,
                        Email = gu.User.Email,
                        Phone = gu.User.Phone,
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

                var newGroupUsers = requestModel.UserIDs ?? new List<int>(); 
                if(newGroupUsers.Count()>0)
                {
                    var currentGroupUsers = await _context.GroupUsers.Where(x => x.GroupID == groupID).ToListAsync();

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
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = "Group updated successfully",
                    StatusCode = 200
                };
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
