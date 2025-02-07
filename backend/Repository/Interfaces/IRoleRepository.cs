using System.Linq.Expressions;

using PruebaViamaticaBackend.Models;

namespace PruebaViamaticaBackend.Repository.Interfaces;

public interface IRoleRepository
{
	Task<Role> Create(Role role);

	Task<IEnumerable<Role>> GetAll(Expression<Func<Role, bool>>? filter = null);

	Task<Role?> GetOne(Expression<Func<Role, bool>>? filter = null);

	Task<Role> Update(Role role);

	Task Delete(Role role);

	Task Save();
}
