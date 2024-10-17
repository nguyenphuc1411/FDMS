using AutoMapper;
using AutoMapper.QueryableExtensions;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FDMS_API.Services.Implementations
{
    public class TypeService : ITypeService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public TypeService(AppDbContext context, IUserService userService, IMapper mapper)
        {
            _context = context;
            _userService = userService;
            _mapper = mapper;
        }

        public async Task<APIResponse> Create(TypeDTO requestModel)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var newType = new FDMS_API.Data.Models.Type
                {
                    TypeName = requestModel.TypeName,
                    Note = requestModel.Note,
                    UserID = _userService.GetUserId(),
                };
                await _context.Types.AddAsync(newType);
                await _context.SaveChangesAsync();
                var listUserGroups = new List<Permission>();
                if (requestModel.Permissions != null && requestModel.Permissions.Count() > 0)
                    foreach (var permission in requestModel.Permissions)
                    {
                        await _context.Permissions.AddAsync(new Permission
                        {
                            TypeID = newType.TypeID,
                            GroupID = permission.GroupID,
                            CanRead = permission.CanRead,
                            CanModify = permission.CanModify
                        });
                    }

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    await transaction.CommitAsync();
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Create type success",
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

        public async Task<APIResponse> Get(int? pageSize, int? currentPage)
        {
            var listType = await _context.Types
                .Select(t => new GetType
                {
                    TypeID = t.TypeID,
                    TypeName = t.TypeName,
                    CreatedAt = t.Created_At,
                    Note = t.Note,
                    Creator = t.User.Name,
                    GroupPermissions = t.Permissions.Select(p => new GroupPermission
                    {
                        GroupID = p.GroupID,
                        GroupName = p.Group.GroupName,
                        CanModify = p.CanRead,
                        CanRead = p.CanRead
                    }).ToList()
                })
                .ToListAsync();

            if (pageSize.HasValue && currentPage.HasValue)
            {
                var pS = pageSize.Value;
                var cP = currentPage.Value;
                var paginatedResult = listType.Pagination(pS, cP);

                return new APIResponse
                {
                    Success = true,
                    Message = "Get types success",
                    Data = paginatedResult,
                    StatusCode = 200
                };
            }
            return new APIResponse
            {
                Success = true,
                Message = "Get types success",
                Data = listType,
                StatusCode = 200
            };
        }

        public async Task<APIResponse> GetByID(int typeID)
        {
           var type = await _context.Types.Where(x=>x.TypeID == typeID)
                .Select(t=>new GetType
                {
                    TypeID = t.TypeID,
                    TypeName = t.TypeName,
                    Note = t.Note,
                    CreatedAt = t.Created_At,
                    Creator = t.User.Name,
                    GroupPermissions = t.Permissions.Select(p =>new GroupPermission
                    {
                        GroupID = p.GroupID,
                        GroupName = p.Group.GroupName,
                        CanModify = p.CanModify,
                        CanRead = p.CanRead
                    }).ToList()

                }).FirstOrDefaultAsync();
            if (type == null)
                return new APIResponse
                {
                    Success = false,
                    Message = "Not found type",
                    StatusCode = 404
                };
            return new APIResponse
            {
                Success = true,
                Message = "Get type success",
                Data = type,
                StatusCode = 200
            };
        }

        public async Task<APIResponse> Update(int typeID, TypeDTO requestModel)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var type = await _context.Types.FindAsync(typeID);
                if (type == null)
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Not found document type",
                        StatusCode = 404
                    };

                type.TypeName = requestModel.TypeName;
                type.Note = requestModel.Note;

                var newPermissionDTOs = requestModel.Permissions ?? new List<PermissionDTO>();
                if (newPermissionDTOs.Count() > 0)
                {
                    var currentPermissions = await _context.Permissions.Where(x => x.TypeID == typeID).ToListAsync();

                    foreach (var item in currentPermissions)
                    {
                        // Xóa hoặc cập nhật những bảng ghi không phải permission trong list các permission mới
                        if (!newPermissionDTOs.Any(x=>x.TypeID == typeID && x.GroupID == item.GroupID ))
                        { 
                            _context.Permissions.Remove(item);
                        }
                        else
                        {
                            var updatePermission = newPermissionDTOs.FirstOrDefault(x => x.TypeID == typeID && x.GroupID == item.GroupID);
                            item.CanRead = updatePermission.CanRead;
                            item.CanModify = updatePermission.CanModify;
                            _context.Permissions.Update(item);
                        }                    
                    }
                    // Thêm những Permission mới vào db
                    foreach (var item in newPermissionDTOs)
                    {
                        if (!currentPermissions.Any(x => x.GroupID == item.GroupID 
                        && x.TypeID ==typeID))
                        {
                            var permission = new Permission
                            {
                                GroupID = item.GroupID,
                                TypeID = typeID,
                                CanRead = item.CanRead,
                                CanModify = item.CanModify
                            };
                            await _context.Permissions.AddAsync(permission);
                        }                 
                    }
                }
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return new APIResponse
                {
                    Success = true,
                    Message = "Document type updated successfully",
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
