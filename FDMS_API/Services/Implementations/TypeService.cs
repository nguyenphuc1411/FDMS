using AutoMapper;
using AutoMapper.QueryableExtensions;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.DTOs.Type;
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

        public async Task<ServiceResponse> Create(TypeDTO requestModel)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var isExists = await _context.Types.AnyAsync(x=>x.TypeName == requestModel.TypeName);
                if (isExists)
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Document type is exists in system",
                        StatusCode = 400
                    };
                }
                var newType = new FDMS_API.Data.Models.Type
                {
                    TypeName = requestModel.TypeName,
                    Note = requestModel.Note,
                    UserID = _userService.GetUserId(),
                };
                await _context.Types.AddAsync(newType);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {     
                    if (requestModel.Permissions.Count() > 0)
                    {
                        var listUserGroups = new List<Permission>();
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

                        var result1 = await _context.SaveChangesAsync();
                        if (result1 > 0)
                        {
                            await transaction.CommitAsync();
                            return new ServiceResponse
                            {
                                Success = true,
                                Message = "Create document type and permission success",
                                StatusCode = 201
                            };
                        }
                        else
                        {
                            return new ServiceResponse
                            {
                                Success = false,
                                Message = "Create document type and permission failed",
                                StatusCode = 400
                            };
                        }
                    }
                    else
                    {
                        await transaction.CommitAsync();
                        return new ServiceResponse
                        {
                            Success = true,
                            Message = "Create document type success",
                            StatusCode = 201
                        };
                    }
                }
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Create document type failed",
                    StatusCode = 400
                };

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ServiceResponse
                {
                    Success = false,
                    Message = "An error",
                    Data = ex.Message,
                    StatusCode = 500
                };
            }
        }

        public async Task<ServiceResponse> Get(int? pageSize, int? currentPage)
        {
            var listType = await _context.Types
                .Select(t => new GetTypes
                {
                    TypeID = t.TypeID,
                    TypeName = t.TypeName,
                    CreatedAt = t.Created_At,
                    Note = t.Note,
                    Creator = t.User.Name,
                    TotalGroups = t.Permissions.Count()
                })
                .ToListAsync();

            if (pageSize.HasValue && currentPage.HasValue)
            {
                var pS = pageSize.Value;
                var cP = currentPage.Value;
                var paginatedResult = listType.Pagination(pS, cP);

                return new ServiceResponse
                {
                    Success = true,
                    Message = "Get types success",
                    Data = paginatedResult,
                    StatusCode = 200
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Message = "Get types success",
                Data = listType,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> GetByID(int typeID)
        {
           var type = await _context.Types.Where(x=>x.TypeID == typeID)
                .Select(t=>new GetType
                {
                    TypeID = t.TypeID,
                    TypeName = t.TypeName,
                    Note = t.Note,
                    GroupPermissions = t.Permissions.Select(p =>new GroupPermission
                    {
                        GroupID = p.GroupID,
                        GroupName = p.Group.GroupName,
                        CanModify = p.CanModify,
                        CanRead = p.CanRead
                    }).ToList()

                }).FirstOrDefaultAsync();
            if (type == null)
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Not found type",
                    StatusCode = 404
                };
            return new ServiceResponse
            {
                Success = true,
                Message = "Get type success",
                Data = type,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> Update(int typeID, TypeDTO requestModel)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var type = await _context.Types.FindAsync(typeID);
                if (type == null)
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Not found document type",
                        StatusCode = 404
                    };

                type.TypeName = requestModel.TypeName;
                type.Note = requestModel.Note;
                _context.Types.Update(type);

                var permissionWithType = await _context.Permissions.Where(x => x.TypeID == typeID).ToListAsync();
               
                if (requestModel.Permissions.Count() > 0)
                {
                    _context.Permissions.RemoveRange(permissionWithType);
                    await  _context.SaveChangesAsync();
                    foreach (var item in requestModel.Permissions)
                    {
                        await _context.Permissions.AddAsync(new Permission {
                            TypeID = typeID,
                            GroupID = item.GroupID,
                            CanModify = item.CanModify,
                            CanRead = item.CanRead
                        });
                    }
                }
                else
                {
                    _context.Permissions.RemoveRange(permissionWithType);
                }
                var result = await _context.SaveChangesAsync();
                if(result > 0)
                {
                    await transaction.CommitAsync();
                    return new ServiceResponse
                    {
                        Success = true,
                        Message = "Update document type success",
                        StatusCode = 200
                    };
                }
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Update document type failed",
                    StatusCode = 400
                };

            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new ServiceResponse
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = 500
                };
            }
        }
    }
}
