using AutoMapper;
using FDMS_API.Data.Models;
using FDMS_API.Models.DTOs;
using FDMS_API.Models.DTOs.Document;
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
            CreateMap<SystemSetting, SystemSetting>().ReverseMap();
            CreateMap<Flight, FlightDTO>().ReverseMap();
            CreateMap<Permission,PermissionDTO>().ReverseMap();

            CreateMap<Document, GetDocuments>()
                .ForMember(dest => dest.Creator, src => src.MapFrom(x => x.User.Name))
                .ForMember(dest => dest.FlightNo, src => src.MapFrom(x => x.Flight.FlightNo))
                .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.Type.TypeName));

            CreateMap<Document, GetRecentlyDocuments>()
               .ForMember(dest => dest.Creator, src => src.MapFrom(x => x.User.Name))
               .ForMember(dest => dest.FlightNo, src => src.MapFrom(x => x.Flight.FlightNo))
               .ForMember(dest => dest.DocumentType, src => src.MapFrom(x => x.Type.TypeName))
               .ForMember(dest => dest.DepartureDate, src => src.MapFrom(x => x.Flight.FlightDate.ToDateTime(x.Flight.DepartureTime)));

            CreateMap<Group, GetGroups>()
                .ForMember(dest => dest.Creator, src => src.MapFrom(x => x.User.Email))
                .ForMember(dest => dest.TotalMembers, src => src.MapFrom(x => x.GroupUsers.Count()));

        }
    }
}
