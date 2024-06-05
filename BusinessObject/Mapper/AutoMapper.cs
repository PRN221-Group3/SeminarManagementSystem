using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Models;

namespace BusinessObject.Mapper
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() { 
            CreateMap<User, UserDto>().ReverseMap();
        }
    }
}
