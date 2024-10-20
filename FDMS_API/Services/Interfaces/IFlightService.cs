using FDMS_API.Models.DTOs.Flight;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IFlightService
    {
        Task<APIResponse> CreateNewFlight(FlightDTO model);

        Task<APIResponse> GetCurrentFlight();

        Task<APIResponse> Get(string? search, string? flightNo, DateOnly? flightDate);
    }
}
