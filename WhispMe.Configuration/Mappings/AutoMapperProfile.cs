using AutoMapper;
using WhispMe.DAL.Entities;
using WhispMe.DTO;

namespace WhispMe.Configuration.Mappings;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<Message, MessageDto>().ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();

        CreateMap<Room, RoomDto>().ReverseMap();

        CreateMap<User, UserDto>().ReverseMap();
    }
}
