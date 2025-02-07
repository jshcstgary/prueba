using AutoMapper;

using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Models.Dtos.User;

using PruebaViamaticaBackend.Models;

namespace PruebaViamaticaBackend.Lib;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();

        CreateMap<Person, RoleDto>().ReverseMap();
        CreateMap<Person, PersonCreateDto>().ReverseMap();
    }
}
