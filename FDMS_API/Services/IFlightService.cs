using FDMS_API.DTOs;

namespace FDMS_API.Services
{
    public interface IFlightService
    {
        APIResponse<string> CreateNewFlight(); 
    }
}
