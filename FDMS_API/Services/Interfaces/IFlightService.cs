using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IFlightService
    {
        Task<APIResponse> CreateNewFlight(CreateFlight model);

        Task<APIResponse> GetCurrentFlight();

        Task<APIResponse> Get();
    }
}
