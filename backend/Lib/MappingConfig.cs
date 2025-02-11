using AutoMapper;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Models.Dtos.Role;
using PruebaViamaticaBackend.Models.Dtos.RoleOption;
using PruebaViamaticaBackend.Models.Dtos.User;

namespace PruebaViamaticaBackend.Lib;

public class MappingConfig : Profile
{
    public MappingConfig()
    {
        CreateMap<Person, PersonDto>().ReverseMap();
        CreateMap<Person, PersonCreateDto>().ReverseMap();
        CreateMap<Person, PersonUpdateDto>().ReverseMap();

        CreateMap<Role, RoleDto>().ReverseMap();
        CreateMap<Role, RoleCreateDto>().ReverseMap();

        CreateMap<RoleOption, RoleOptionDto>().ReverseMap();
        CreateMap<RoleOption, RoleOptionCreateDto>().ReverseMap();

        CreateMap<User, UserDto>().ReverseMap();
        CreateMap<User, UserCreateDto>().ReverseMap();
        CreateMap<User, UserUpdateDto>().ReverseMap();
    }
}
