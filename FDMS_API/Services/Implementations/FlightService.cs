using AutoMapper;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using System.Security.Claims;

namespace FDMS_API.Services.Implementations
{
    public class FlightService : IFlightService
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        private readonly IUserService _userService;
        public FlightService(AppDbContext context, IMapper mapper, IUserService userService)
        {
            _context = context;
            _mapper = mapper;
            _userService = userService;
        }

        public async Task<APIResponse> CreateNewFlight(CreateFlightDTO model)
        {
            var newFlight = _mapper.Map<Flight>(model);
            newFlight.UserID = _userService.GetUserId();
            _context.Flights.Add(newFlight);
            var result = await _context.SaveChangesAsync();
            if(result > 0)
            {
                return new APIResponse
                {
                    Success = true,
                    Message = "Created flight",
                    StatusCode = 201
                };
            }
            return new APIResponse
            {
                Success = false,
                Message = "Create flight failed",
                StatusCode = 400
            };
        }
    }
}
