using AutoMapper;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.DTOs.Flight;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
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

        public async Task<APIResponse> CreateNewFlight(FlightDTO model)
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

        public async Task<APIResponse> Get()
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);
            var listFlight = await _context.Flights.Select(f => new GetFlight
            {
                FlightID = f.FlightID,
                FlightNo = f.FlightNo,
                DepartureDate = f.FlightDate,
                TotalDocument = f.Documents.Count,
                Route = f.POL.GetInitials() + " - " + f.POU.GetInitials(),

                // So sánh ngày bay và thời gian hiện tại
                IsFinished = f.FlightDate < currentDate ||
                   (f.FlightDate == currentDate && f.ArrivalTime < currentTime)
            }).ToListAsync();

            return new APIResponse
            {
                Success = true,
                Message = "Get flights success",
                Data = listFlight,
                StatusCode = 200
            };
        }

        public async Task<APIResponse> GetCurrentFlight()
        {
            var currentFlight = await _context.Flights
                .Where(x => x.FlightDate == DateOnly.FromDateTime(DateTime.Now))
                .Select(f => new GetCurrentFlight
                {
                    FlightID = f.FlightID,
                    FlightNo = f.FlightNo,
                    AircraftID = f.AircraftID,
                    FlightDate = f.FlightDate,
                    ArrivalTime = f.ArrivalTime,
                    DepartureTime = f.DepartureTime,
                    SendFiles = f.Documents.Where(x=>x.Version== (decimal)1.0).Count(),
                    ReturnFiles = f.Documents.Where(x => x.Version != (decimal)1.0).Count(),
                }).ToListAsync();
            return new APIResponse
            {
                Success = true,
                Message = "Get current flight success",
                Data = currentFlight,
                StatusCode = 200
            };
        }


    }
}
