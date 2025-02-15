using Microsoft.EntityFrameworkCore;

using PruebaViamaticaBackend.Constants;
using PruebaViamaticaBackend.Data;
using PruebaViamaticaBackend.Models;

using PruebaViamaticaBackend.Repository.Interfaces;

namespace PruebaViamaticaBackend.Repository;

public class AuthRepository : IAuthRepository
{
	private readonly ILogger<IAuthRepository> _logger;

	private readonly AppDbContext _context;

	protected readonly DbSet<Person> dbSet;

	public AuthRepository(ILogger<IAuthRepository> logger, AppDbContext context)
	{
		_logger = logger;
		_context = context;
		dbSet = _context.Set<Person>();
	}

	public async Task<Person?> SignIn(AuthData authData)
	{
		_logger.LogInformation("Executing Repository class - Auth method");

		try
		{
			return await dbSet
				.Include(person => person.User)
					.ThenInclude(user => user!.Roles)
						.ThenInclude(role => role.RoleOptions)
				.AsNoTracking()
				.FirstOrDefaultAsync(person => person.User != null && person.User.Status == Status.Active && (person.User.Username == authData.Username || person.User.Mail == authData.Username) && person.User.Password == authData.Password);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - Auth method");
		}
	}
}
