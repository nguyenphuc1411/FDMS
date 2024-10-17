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
            CreateMap<SystemSetting, SystemSetting>().ReverseMap();
            CreateMap<Flight, CreateFlight>().ReverseMap();
            CreateMap<Group, GroupDTO>().ReverseMap();
            CreateMap<Permission,PermissionDTO>().ReverseMap();
        }
    }
}
