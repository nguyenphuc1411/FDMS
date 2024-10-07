using AutoMapper;
using FDMS_API.Data.Models;
using FDMS_API.DTOs;

namespace FDMS_API.Mappings
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<User,UserDTO>().ReverseMap();
            CreateMap<SystemSetting,SystemSettingDTO>().ReverseMap();
        }
    }
}
