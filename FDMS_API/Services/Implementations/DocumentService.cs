using AutoMapper;
using AutoMapper.QueryableExtensions;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs.Document;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FDMS_API.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        private readonly IFileUploadService _fileUploadService;
        public DocumentService(AppDbContext context, IMapper mapper, IUserService userService, IFileUploadService fileUploadService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
            _fileUploadService = fileUploadService;
        }

        public async Task<APIResponse> Get(string? search,int? typeID, DateOnly? createdDate,int? pageSize,int? currentPage)
        {
            var listDocuments = _context.Documents.AsQueryable();
            if (typeID.HasValue)
            {
                listDocuments = listDocuments.Where(x => x.TypeID == typeID);
            }
            if (!string.IsNullOrEmpty(search))
            {
                listDocuments = listDocuments.Where(x=>x.Title.Contains(search));
            }
            if(createdDate.HasValue)
            {
                listDocuments = listDocuments.Where(x =>DateOnly.FromDateTime(x.UploadDate.Date) == createdDate);
            }      
            var finalDocuments = await listDocuments.ProjectTo<GetDocuments>(_mapper.ConfigurationProvider).ToListAsync();
            if (pageSize.HasValue && currentPage.HasValue)
            {
                var pagedResult = finalDocuments.Pagination(pageSize.Value, currentPage.Value);
                return new APIResponse
                {
                    Success = true,
                    Message = "Get document success",
                    Data = pagedResult,
                    StatusCode = 200
                };
            }
            return new APIResponse
            {
                Success = true,
                Message = "Get document success",
                Data = finalDocuments,
                StatusCode = 200
            };
        }

        public async Task<APIResponse> GetRecently(int? size)
        {
            var recentlyDocuments = await _context.Documents
    .Select(doc => new GetRecentlyDocuments
    {
        DocumentID = doc.DocumentID,
        Title = doc.Title,
        DocumentType = doc.Type.TypeName,
        UploadDate = doc.UploadDate,
        DepartureDate = doc.Flight.FlightDate.ToDateTime(doc.Flight.DepartureTime),
        Creator = doc.User.Name,
        FlightNo = doc.Flight.FlightNo
    })
    .OrderByDescending(d => d.UploadDate) // Sắp xếp theo thời gian upload mới nhất
    .GroupBy(doc => doc.DocumentID) // Nhóm theo DocumentID để lấy tài liệu mới nhất
    .Select(g => g.FirstOrDefault()) // Chọn tài liệu đầu tiên trong nhóm (tài liệu mới nhất)
    .ToListAsync();

            return new APIResponse
            {
                Success = true,
                Message = "Get documents success",
                Data = recentlyDocuments,
                StatusCode = 200
            };

        }

        public async Task<APIResponse> Upload(AdminUploadDocument requestModel)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var uploadResult = await _fileUploadService.UploadDocument(requestModel.File, "Original");
                if (uploadResult.Success)
                {
                    var newDocument = _mapper.Map<Document>(requestModel);
                    newDocument.Title = requestModel.File.FileName;
                    newDocument.UserID = _userService.GetUserId();
                    newDocument.Version = (decimal)1.0;
                    newDocument.FilePath = uploadResult.FilePath.ToString();
                    await _context.Documents.AddAsync(newDocument);

                    var result = await _context.SaveChangesAsync();
                    if (result > 0)
                    {
                        if (requestModel.GroupIDs.Count() > 0)
                        {
                            foreach (var item in requestModel.GroupIDs)
                            {
                                await _context.DocumentPermissions.AddAsync(new DocumentPermission
                                {
                                    DocumentID = newDocument.DocumentID,
                                    GroupID = item
                                });
                            }
                            var result1 = await _context.SaveChangesAsync();
                            if (result1 > 0)
                            {
                                await transaction.CommitAsync();
                                return new APIResponse
                                {
                                    Success = true,
                                    Message = "Upload document success",
                                    StatusCode = 201
                                };
                            }
                            return new APIResponse
                            {
                                Success = false,
                                Message = "Upload document failed",
                                StatusCode = 400
                            };

                        }

                        return new APIResponse
                        {
                            Success = false,
                            Message = "Permission document is required",
                            StatusCode = 400
                        };
                    }
                    else
                    {
                        return new APIResponse
                        {
                            Success = false,
                            Message = "Upload new document failed",
                            StatusCode = 400
                        };
                    }
                }
                return new APIResponse
                {
                    Success = false,
                    Message = "Upload file failed",
                    StatusCode = 500
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

        public async Task<APIResponse> UploadVersion(VersionDTO requestModel)
        {
            var uploadResult = await _fileUploadService.UploadDocument(requestModel.File, "Updated");
            if (uploadResult.Success)
            {
                var newVersion = new VersionDocument
                {
                    Title = requestModel.File.FileName,
                    DocumentID = requestModel.DocumentID,
                    UserID = _userService.GetUserId(),
                    FilePath = uploadResult.FilePath.ToString()
                };
                var versionExists = await _context.VersionDocuments.Where(x => x.DocumentID == requestModel.DocumentID).ToListAsync();
                if (versionExists.Count() > 0)
                {
                    // +version 0.1
                    var maxVersion = versionExists.Max(x=>x.Version);
                    newVersion.Version = maxVersion + (decimal)0.1;
                }
                else
                {
                    newVersion.Version = (decimal)1.1;
                }
                await _context.VersionDocuments.AddAsync(newVersion);

                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Upload new version document success",
                        StatusCode = 201
                    };
                }
                else
                {
                    return new APIResponse
                    {
                        Success = false,
                        Message = "Upload new version document failed",
                        StatusCode = 400
                    };
                }
            }
            return new APIResponse
            {
                Success = false,
                Message = "Upload file failed",
                StatusCode = 500
            };
        }

        public async Task<APIResponse> UserUpload(UserUploadDocument requestModel)
        {
            var uploadResult = await _fileUploadService.UploadDocument(requestModel.File, "UploadByUser");
            if (uploadResult.Success)
            {
                var newDocument = new Document
                {
                    Title = requestModel.File.FileName,
                    UserID = _userService.GetUserId(),
                    Version = (decimal)1.1,
                    FlightID = requestModel.FlightID,
                    TypeID = requestModel.TypeID,
                    FilePath = uploadResult.FilePath.ToString()
                };
                await _context.Documents.AddAsync(newDocument);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new APIResponse
                    {
                        Success = true,
                        Message = "Upload document success",
                        StatusCode = 201
                    };
                }
                return new APIResponse
                {
                    Success = false,
                    Message = "Upload document failed",
                    StatusCode = 400
                };
            }
            else
            {
                return new APIResponse
                {
                    Success = false,
                    Message = "Upload file failed",
                    StatusCode = 500
                };
            }
        }

        public async Task<APIResponse> ViewDocs(int documentID)
        {
            var document = await _context.Documents.Where(x => x.DocumentID == documentID)
                .Select(doc => new
                {
                    doc.DocumentID,
                    doc.Title,
                    Type = doc.Type.TypeName,
                    CreatedDate = doc.UploadDate,
                    doc.Version,
                    Creator = doc.User.Name,
                    Permissions= doc.DocumentPermissions.Select(dp => new
                    {
                        dp.Group.GroupName
                    }),
                    UpdatedVersion = doc.Versions.Select(v =>new
                    {
                        v.VersionID,
                        v.Title,
                        v.Version,
                        v.UploadDate,
                        v.User.Name
                    })
                })
                .FirstOrDefaultAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Get docs success",
                Data = document,
                StatusCode = 200
            };
        }
    }
}
