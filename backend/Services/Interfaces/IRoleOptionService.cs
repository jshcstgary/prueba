using System.Linq.Expressions;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Role;
using PruebaViamaticaBackend.Models.Dtos.RoleOption;

namespace PruebaViamaticaBackend.Services.Interfaces;

public interface IRoleOptionService
{
    Task<RoleOptionDto> Create(RoleOptionCreateDto roleOptionCreateDto);
    Task<IEnumerable<RoleOptionDto>> GetAll(Expression<Func<RoleOption, bool>>? filter = null);
    Task<RoleOptionDto?> GetOne(Expression<Func<RoleOption, bool>>? filter = null);
    Task<RoleOptionDto?> Update(RoleOptionDto roleOptionDto);
    Task<bool> Delete(int id);
}
