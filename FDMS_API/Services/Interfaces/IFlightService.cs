using FDMS_API.Models.DTOs.Flight;
using FDMS_API.Models.ResponseModel;

namespace FDMS_API.Services.Interfaces
{
    public interface IFlightService
    {
        Task<ServiceResponse> CreateNewFlight(FlightDTO model);

        Task<ServiceResponse> UpdateFlight(int flightID,FlightDTO model);

        Task<ServiceResponse> GetCurrentFlight();

        Task<ServiceResponse> Get(string? search, string? flightNo, DateOnly? flightDate);

        Task<ServiceResponse> GetTodayFlight();

        Task<ServiceResponse> GetFlightReported(int flightID);

        Task<ServiceResponse> GetFlight(int flightID);
    }
}
