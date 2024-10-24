using AutoMapper;
using FDMS_API.Data;
using FDMS_API.Data.Models;
using FDMS_API.Extentions;
using FDMS_API.Models.DTOs.Flight;
using FDMS_API.Models.ResponseModel;
using FDMS_API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

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

        public async Task<ServiceResponse> CreateNewFlight(FlightDTO model)
        {
            var newFlight = _mapper.Map<Flight>(model);
            newFlight.UserID = _userService.GetUserId();
            _context.Flights.Add(newFlight);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Created flight",
                    StatusCode = 201
                };
            }
            return new ServiceResponse
            {
                Success = false,
                Message = "Create flight failed",
                StatusCode = 400
            };
        }

        public async Task<ServiceResponse> Get(string? search, string? flightNo, DateOnly? flightDate)
        {
            var currentDate = DateOnly.FromDateTime(DateTime.Now);
            var currentTime = TimeOnly.FromDateTime(DateTime.Now);
            var listFlights = _context.Flights.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                listFlights = listFlights.Where(x => x.FlightNo.Contains(search));
            }
            if (!string.IsNullOrEmpty(flightNo))
            {
                listFlights = listFlights.Where(x => x.FlightNo == flightNo);
            }
            if (flightDate.HasValue)
            {
                listFlights = listFlights.Where(x => x.FlightDate == flightDate);
            }

            var finalFlights = await listFlights.Select(f => new GetFlight
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

            return new ServiceResponse
            {
                Success = true,
                Message = "Get flights success",
                Data = finalFlights,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> GetCurrentFlight()
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
                    SendFiles = f.Documents.Where(x => x.Version == (decimal)1.0).Count(),
                    ReturnFiles = f.Documents.Where(x => x.Version != (decimal)1.0).Count()
                                    + f.Documents.SelectMany(d => d.Versions).Count()
                }).ToListAsync();
            return new ServiceResponse
            {
                Success = true,
                Message = "Get current flight success",
                Data = currentFlight,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> GetFlightReported(int flightID)
        {
            var reports = await _context.Reports
                .Where(x => x.FlightID == flightID)
                .ToListAsync();

            var documents = await _context.Documents
                .Where(x => x.FlightID == flightID)
                .Select(d => new
                {
                    d.Type.TypeName,
                    LatestVersion = d.Versions
                        .OrderByDescending(v => v.Version)
                        .FirstOrDefault(),
                    Document = d
                })
                .ToListAsync();

            var results = documents.Select(doc => new
            {
                doc.TypeName,
                doc.Document.DocumentID,
                Title = doc.LatestVersion != null ? doc.LatestVersion.Title : doc.Document.Title,
                UploadDate = doc.LatestVersion != null ? doc.LatestVersion.UploadDate : doc.Document.UploadDate,
                Version = doc.LatestVersion != null ? doc.LatestVersion.Version : doc.Document.Version,
                Creator = doc.LatestVersion != null ? doc.LatestVersion.User?.Name : doc.Document.User?.Name,
                FilePath = doc.LatestVersion != null ? doc.LatestVersion.FilePath : doc.Document.FilePath,
                IsVersion = doc.LatestVersion != null
            }).ToList();

            var userID = _userService.GetUserId();

            var flightReported = await _context.Flights
                .Where(x => x.FlightID == flightID)
                .Select(f => new
                {
                    f.FlightID,
                    f.FlightNo,
                    f.FlightDate,
                    f.DepartureTime,
                    f.POL,
                    f.POU,
                    ReportedAt = f.Reports.Select(x => x.ReportedAt).FirstOrDefault(),
                    ReportBy = f.Reports.Select(x => x.User.Name).FirstOrDefault(),
                    SignatureImg = f.Reports.Select(x => x.SignatureURL).FirstOrDefault(),
                    Document = results 
                })
                .FirstOrDefaultAsync();

            return new ServiceResponse
            {
                Success = true,
                Message = "Get successfullt",
                Data = flightReported,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> GetFlight(int flightID)
        {
            var flight = await _context.Flights
                .Where(x => x.FlightID == flightID)
                .Include(f => f.Documents) 
                .ThenInclude(d => d.Versions)
                .FirstOrDefaultAsync();

            // Kiểm tra nếu flight không tồn tại
            if (flight == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Not found",
                    StatusCode = 404
                };
            }

            // Tách riêng các dữ liệu theo yêu cầu
            var originalDocuments = flight.Documents
                .Where(d => d.Version == (decimal)1.0)
                .Select(d => new
                {
                    d.DocumentID,
                    d.Title,
                    d.Version,
                    d.FilePath
                })
                .ToList();

            var updatedDocuments = flight.Documents
                .Where(d => d.Version == (decimal)1.1)
                .Select(d => new
                {
                    d.DocumentID,
                    d.Title,
                    d.Version,
                    d.FilePath
                })
                .ToList();

            var versionedDocuments = flight.Documents
                .SelectMany(d => d.Versions.Select(vd => new
                {
                    vd.DocumentID,
                    vd.Title,
                    vd.Version,
                    vd.FilePath
                }))
                .ToList();

            updatedDocuments = updatedDocuments.Concat(versionedDocuments).ToList();

            var result = new
            {
                flight.FlightID,
                flight.FlightNo,
                flight.AircraftID,
                flight.POL,
                flight.POU,
                Documents = new
                {
                    Original = originalDocuments,
                    Updated = updatedDocuments
                }
            };
            if (result == null) return new ServiceResponse
            {
                Success = false,
                Message = "Not found",
                StatusCode = 404
            };
            return new ServiceResponse
            {
                Success = true,
                Message = "Get flight successfully",
                Data = result,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> GetTodayFlight()
        {
            var userID = _userService.GetUserId();
            var currentFlight = await _context.Flights
               .Where(x => x.FlightDate == DateOnly.FromDateTime(DateTime.Now))
               .Select(f => new GetTodayFlight
               {
                   FlightID = f.FlightID,
                   FlightNo = f.FlightNo,
                   AircraftID = f.AircraftID,
                   FlightDate = f.FlightDate,
                   ArrivalTime = f.ArrivalTime,
                   DepartureTime = f.DepartureTime,
                   SendFiles = f.Documents.Where(x => x.Version == (decimal)1.0).Count(),
                   ReturnFiles = f.Documents.Where(x => x.Version != (decimal)1.0).Count()
                                   + f.Documents.SelectMany(d => d.Versions).Count(),
                   IsConfirm = f.Reports.Any(x => x.UserID == userID)
               }).ToListAsync();
            return new ServiceResponse
            {
                Success = true,
                Message = "Get today flight success",
                Data = currentFlight,
                StatusCode = 200
            };
        }

        public async Task<ServiceResponse> UpdateFlight(int flightID, FlightDTO model)
        {
            var flight = await _context.Flights.FindAsync(flightID);
            if (flight == null)
            {
                return new ServiceResponse
                {
                    Success = false,
                    Message = "Flight not found",
                    StatusCode = 404
                };
            }
            _mapper.Map(model, flight);

            _context.Flights.Update(flight);

            var result = await _context.SaveChangesAsync();

            if (result > 0)
            {
                return new ServiceResponse
                {
                    Success = true,
                    Message = "Flight updated successfully",
                    StatusCode = 200
                };
            }

            return new ServiceResponse
            {
                Success = false,
                Message = "Flight update failed",
                StatusCode = 400
            };
        }
    }
}
