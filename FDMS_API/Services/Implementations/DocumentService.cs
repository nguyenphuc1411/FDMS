using AutoMapper;
using AutoMapper.QueryableExtensions;
using FDMS_API.Data;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs.Document;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FDMS_API.Services.Implementations
{
    public class DocumentService : IDocumentService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public DocumentService(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
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
                listDocuments = listDocuments.Where(x=>x.Name.Contains(search));
            }
            if(createdDate.HasValue)
            {
                listDocuments = listDocuments.Where(x =>DateOnly.FromDateTime(x.Created_At.Date) == createdDate);
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
    }
}
