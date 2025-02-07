using System.Linq.Expressions;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Models.Dtos.Role;

namespace PruebaViamaticaBackend.Services.Interfaces;

public interface IRoleService
{
    Task<RoleDto> Create(RoleCreateDto roleCreateDto);
    Task<IEnumerable<RoleDto>> GetAll(Expression<Func<Role, bool>>? filter = null);
    Task<RoleDto?> GetOne(Expression<Func<Role, bool>>? filter = null);
    Task<RoleDto?> Update(RoleDto roleDto);
    Task<bool> Delete(int id);
}
