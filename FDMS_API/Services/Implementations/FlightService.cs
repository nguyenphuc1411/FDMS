using FDMS_API.Data;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using System.Security.Claims;

namespace FDMS_API.Services.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly AppDbContext _context;

        public FlightService(AppDbContext context)
        {
            _context = context;
        }

        public Task<APIResponse> CreateNewFlight()
        {
            throw new NotImplementedException();
        }
    }
}
