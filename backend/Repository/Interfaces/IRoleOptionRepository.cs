using System.Linq.Expressions;

using PruebaViamaticaBackend.Models;

namespace PruebaViamaticaBackend.Repository.Interfaces;

public interface IRoleOptionRepository
{
	Task<RoleOption> Create(RoleOption roleOption);

	Task<IEnumerable<RoleOption>> GetAll(Expression<Func<RoleOption, bool>>? filter = null);

	Task<RoleOption?> GetOne(Expression<Func<RoleOption, bool>> filter);

	Task<RoleOption> Update(RoleOption roleOption);

	Task Delete(RoleOption roleOption);

	Task Save();
}
