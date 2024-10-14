using AutoMapper;
using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;

namespace FDMS_API.Configurations.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<SystemSetting, SystemSettingDTO>().ReverseMap();
            CreateMap<Flight, CreateFlightDTO>().ReverseMap();
        }
    }
}
