using AutoMapper;
using AutoMapper.QueryableExtensions;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs.Document;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Drawing;
using System.Linq;
using System.Net.WebSockets;

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

        public async Task<ServiceResponse> Delete(int documentID)
        {
            var userID = _userService.GetUserId();
            var document = await _context.Documents.FirstOrDefaultAsync(x => x.DocumentID == documentID);
            if (document == null) return new ServiceResponse
            {
                Success = false,
                Message = "Not found document",
                StatusCode = 404
            };
            if (document.Version != (decimal)1.1 || document.UserID != userID)
                return new ServiceResponse
                {
                    Success = false,
                    Message = "User current do not permission",
                    StatusCode = 400
                };
            //Kiểm tra đã xác nhận chưa, đã xác nhận thì không cho xóa
            bool isExists = await _context.Reports.AnyAsync(x => x.FlightID == document.FlightID && document.UserID == userID);
            if (isExists)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "This flight already reported by you.",
                    StatusCode = 400
                };
            }
            // Xóa tài liệu
            _context.Documents.Remove(document);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Delete document successfully.",
                    StatusCode = 200
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "An error occured while deleting.",
                StatusCode = 400
            };
        }

        public async Task<ServiceResponse> Get(string? search, int? typeID, DateOnly? createdDate, int? pageSize, int? currentPage)
        {
            var listDocuments = _context.Documents.AsQueryable();
            if (typeID.HasValue)
            {
                listDocuments = listDocuments.Where(x => x.TypeID == typeID);
            }
            if (!string.IsNullOrEmpty(search))
            {
                listDocuments = listDocuments.Where(x => x.Title.Contains(search));
            }
            if (createdDate.HasValue)
            {
                listDocuments = listDocuments.Where(x => DateOnly.FromDateTime(x.UploadDate.Date) == createdDate);
            }
            var finalDocuments = await listDocuments.ProjectTo<GetDocuments>(_mapper.ConfigurationProvider).ToListAsync();
            if (pageSize.HasValue && currentPage.HasValue)
            {
                var pagedResult = finalDocuments.Pagination(pageSize.Value, currentPage.Value);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Get document success",
                    Data = pagedResult,
                    StatusCode = 200
                };
            }
            return new ServiceResponse
            {
                Success = true,
                Message = "Get document success",
                Data = finalDocuments,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> GetDocumentByFlight(int flightID)
        {
            var documents = await _context.Documents
            .Where(x => x.FlightID == flightID)
            .Select(d => new
            {
                d.Type.TypeName,
                LatestVersion = d.Versions
                    .OrderByDescending(v => v.Version)
                    .FirstOrDefault(),
                Document = d 
            })
            .ToListAsync();

            var results = documents.Select(doc => new
            {
                doc.TypeName, 
                doc.Document.DocumentID,
                Title = doc.LatestVersion != null ? doc.LatestVersion.Title : doc.Document.Title,
                UploadDate = doc.LatestVersion != null ? doc.LatestVersion.UploadDate : doc.Document.UploadDate,
                Version = doc.LatestVersion != null ? doc.LatestVersion.Version : doc.Document.Version,
                Creator = doc.LatestVersion != null ? doc.LatestVersion.User?.Name : doc.Document.User?.Name, 
                FilePath = doc.LatestVersion != null ? doc.LatestVersion.FilePath : doc.Document.FilePath, 
                IsVersion = doc.LatestVersion != null 
            }).ToList();

            return new ServiceResponse
            {
                Success = true,
                Message = "Get successfully",
                Data = results,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> GetRecently(int? size)
        {
            var listdocuments = _context.Documents
             .Include(d => d.Flight)
             .Include(d => d.Versions)
             .AsQueryable();
            if (listdocuments == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Not found",
                    StatusCode = 404
                };
            }

            var documents = await listdocuments
                .Select(d => new
                {
                    d.DocumentID,
                    d.Title,
                    d.Version,
                    d.FilePath,
                    d.Type.TypeName,
                    d.Flight.FlightNo,
                    d.Flight.FlightDate,
                    d.Flight.DepartureTime,
                    d.User.Name,
                    d.UploadDate,
                    IsVersion = false
                })
                .ToListAsync();

            var versions = await listdocuments
                .SelectMany(d => d.Versions.Select(vd => new
                {
                    vd.DocumentID,
                    vd.Title,
                    vd.Version,
                    vd.FilePath,
                    d.Type.TypeName,
                    d.Flight.FlightNo,
                    d.Flight.FlightDate,
                    d.Flight.DepartureTime,
                    vd.User.Name,
                    vd.UploadDate,
                    IsVersion = true
                }))
                .ToListAsync();
            if (size.HasValue)
            {
                var resultBySize = documents.Concat(versions).OrderByDescending(x => x.UploadDate).Take(size.Value);
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Get successfully",
                    Data = resultBySize,
                    StatusCode = 200
                };
            }
            var result = documents.Concat(versions).OrderByDescending(x=>x.UploadDate);
            return new ServiceResponse
            {
                Success = true,
                Message = "Get successfully",
                Data = result,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> Upload(AdminUploadDocument requestModel)
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
                                return new ServiceResponse
                                {
                                    Success = true,
                                    Message = "Upload document success",
                                    StatusCode = 201
                                };
                            }
                            return new ServiceResponse
                            {
                                Success = false,
                                Message = "Upload document failed",
                                StatusCode = 400
                            };

                        }

                        return new ServiceResponse
                        {
                            Success = false,
                            Message = "Permission document is required",
                            StatusCode = 400
                        };
                    }
                    else
                    {
                        return new ServiceResponse
                        {
                            Success = false,
                            Message = "Upload new document failed",
                            StatusCode = 400
                        };
                    }
                }
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Upload file failed",
                    StatusCode = 500
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

        public async Task<ServiceResponse> UploadVersion(VersionDTO requestModel)
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
                    var maxVersion = versionExists.Max(x => x.Version);
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
                    return new ServiceResponse
                    {
                        Success = true,
                        Message = "Upload new version document success",
                        StatusCode = 201
                    };
                }
                else
                {
                    return new ServiceResponse
                    {
                        Success = false,
                        Message = "Upload new version document failed",
                        StatusCode = 400
                    };
                }
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "Upload file failed",
                StatusCode = 500
            };
        }

        public async Task<ServiceResponse> UserUpload(UserUploadDocument requestModel)
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
                    return new ServiceResponse
                    {
                        Success = true,
                        Message = "Upload document success",
                        StatusCode = 201
                    };
                }
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Upload document failed",
                    StatusCode = 400
                };
            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Upload file failed",
                    StatusCode = 500
                };
            }
        }

        public async Task<ServiceResponse> ViewDocs(int documentID)
        {
            var document = await _context.Documents
            .Where(x => x.DocumentID == documentID)
            .Select(doc => new
            {
                doc.DocumentID,
                doc.Title,
                Type = doc.Type.TypeName,
                CreatedDate = doc.UploadDate,
                doc.Version,
                doc.FilePath,
                Creator = doc.User.Name,
                Permissions = doc.DocumentPermissions.Select(dp => new
                {
                    dp.Group.GroupName,
                    Permission = GetPermissionText(dp.Group.Permissions.FirstOrDefault(x => x.GroupID == dp.GroupID))
                }),
                UpdatedVersion = doc.Versions.Select(v => new
                {
                    v.VersionID,
                    v.Title,
                    v.Version,
                    v.UploadDate,
                    CreatorName = v.User.Name
                })
            })
            .FirstOrDefaultAsync();

            if (document == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Document not found",
                    Data = null,
                    StatusCode = 404
                };
            }

            return new ServiceResponse
            {
                Success = true,
                Message = "Get docs success",
                Data = document,
                StatusCode = 200
            };
        }
       

        public async Task<ServiceResponse> ViewDocsForUser(int documentID)
        {
            var document = await _context.Documents.Where(x => x.DocumentID == documentID)
                    .Select(doc => new
                    {
                        doc.DocumentID,
                        doc.Title,
                        doc.Version,
                        doc.FilePath
                    }).FirstOrDefaultAsync();
            if(document == null) return new ServiceResponse
            {
                Success = true,
                Message = "Not found document",
                StatusCode = 404
            };
            return new ServiceResponse
            {
                Success = true,
                Message = "Get document successfully",
                Data = document,
                StatusCode = 200
            };
        }
        static string GetPermissionText(Permission permission)
        {
            if (permission == null) return "No Permission";

            if (permission.CanRead && permission.CanModify)
                return "View and Edit";
            else if (permission.CanRead)
                return "Read Only";
            else
                return "No Permission";
        }
    }
}
