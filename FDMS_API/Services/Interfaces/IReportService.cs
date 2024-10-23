using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IReportService
    {
        public Task<ServiceResponse> Report(ReportDTO reportDTO);
    }
}
