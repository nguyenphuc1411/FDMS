using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;

namespace FDMS_API.Services.Implementations
{
    public class ReportService : IReportService
    {
        private readonly AppDbContext _context;
        private readonly IUserService _userService;
        private readonly IFileUploadService _fileService;
        public ReportService(AppDbContext context, IUserService userService, IFileUploadService fileService)
        {
            _context = context;
            _userService = userService;
            _fileService = fileService;
        }

        public async Task<ServiceResponse> Report(ReportDTO reportDTO)
        {
            var uploadImg = await _fileService.UploadImage(reportDTO.File, "Signature");
            if(uploadImg.Success)
            {
                var newReport = new Report
                {
                    UserID = _userService.GetUserId(),
                    FlightID = reportDTO.FlightID,
                    SignatureURL = uploadImg.FilePath.ToString()
                };
                await _context.Reports.AddAsync(newReport);
                var result = await _context.SaveChangesAsync();
                if (result > 0)
                {
                    return new ServiceResponse
                    {
                        Success = true,
                        Message = "Report Success",
                        StatusCode = 200
                    };
                }
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Report failed",
                    StatusCode = 400
                };

            }
            else
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Upload signaute failed",
                    StatusCode = 400
                };
            }
          
        }
    }
}
