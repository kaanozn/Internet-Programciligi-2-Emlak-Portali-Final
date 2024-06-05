using AutoMapper;
using WebApplication1.Dtos;
using WebApplication1.Models;

namespace WebApplication1.Mapping
{
    public class Map : Profile
    {
        public Map()
        {
            CreateMap<AppUser, UserDto>().ReverseMap();
            CreateMap<House, HouseDto>().ReverseMap();
            CreateMap<HouseImages, HouseImagesDto>().ReverseMap();
            CreateMap<Province, ProvinceDto>().ReverseMap();
            CreateMap<District, DistrictDto>().ReverseMap();
            CreateMap<House, DistrictHous>()
                .ForMember(dest => dest.Districts, opt => opt.MapFrom(src => src.District))
                .ReverseMap();
            CreateMap<District,DistrictAddDto>().ReverseMap();
            CreateMap<Province,ProvinceAddDto>().ReverseMap();
            CreateMap<House, GetDetailHostDto>();
            CreateMap<HouseImages, HouseImagesDto>();
            CreateMap<District, DistrictDto>();
            CreateMap<Province, ProvinceDto>();
            CreateMap<AppUser, AppUserDto>();
        }
    }
}
