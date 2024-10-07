using FDMS_API.DTOs;
using FDMS_API.Services;
using System.Security.Claims;

namespace FDMS_API.Repositories
{
    public class FlightService : IFlightService
    {
        private readonly IHttpContextAccessor _contextAccesstor;

        public FlightService(IHttpContextAccessor contextAccesstor)
        {
            _contextAccesstor = contextAccesstor;
        }

        public APIResponse<string> CreateNewFlight()
        {
            throw new NotImplementedException();
        }
    }
}
