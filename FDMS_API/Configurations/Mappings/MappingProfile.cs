using AutoMapper;
using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.DTOs.Flight;
using FDMS_API.Models.DTOs.Group;
using FDMS_API.Models.DTOs.User;

namespace FDMS_API.Configurations.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, GetUser>().ReverseMap();
            CreateMap<UserDTO,User>().ForMember(dest=>dest.Groups,opt=>opt.Ignore()).ReverseMap();
            CreateMap<SystemSetting, SystemSetting>().ReverseMap();
            CreateMap<Flight, FlightDTO>().ReverseMap();
            CreateMap<Group, GroupDTO>().ReverseMap();
            CreateMap<Permission,PermissionDTO>().ReverseMap();
        }
    }
}
