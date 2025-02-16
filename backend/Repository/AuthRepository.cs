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
		_logger.LogInformation("Executing Repository class - SignIn method");

		try
		{
			Person? person = await dbSet
				.Include(person => person.User)
					.ThenInclude(user => user.Roles)
						.ThenInclude(role => role.RoleOptions)
				.FirstOrDefaultAsync(person => person.User != null && person.User.Status == Status.Active && (person.User.Username == authData.Username || person.User.Mail == authData.Username) && person.User.Password == authData.Password);

			if (person == null)
			{
				return null;
			}

			if (person!.User.SessionActive)
			{
				throw new Exception("SignIn:Ya ha una sesión iniciada.");
			}

			person!.User.SessionActive = SessionStatus.Active;

			_context.Update(person);

			await _context.SaveChangesAsync();

			return person;
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - SignIn method");
		}
	}

	public async Task SetSession(int idUser, bool status)
	{
		_logger.LogInformation("Executing Repository class - SetSession method");

		try
		{
			Session newSession = new()
			{
				Id = 0,
				EntryDate = status ? DateTime.Now : null,
				CloseDate = status ? null : DateTime.Now,
				IdUser = idUser,
				Status = status ? Status.Open : Status.Closed,
				User = null
			};

			await _context.Sessions.AddAsync(newSession);

			await _context.SaveChangesAsync();
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - SetSession method");
		}
	}

	public async Task<int> SignOut(int idPerson)
	{
		_logger.LogInformation("Executing Repository class - SignOut");

		try
		{
			Person? person = await dbSet
				.Include(person => person.User)
					.ThenInclude(user => user.Roles)
						.ThenInclude(role => role.RoleOptions)
				.FirstOrDefaultAsync(person => person.Id == idPerson);

			person!.User.SessionActive = SessionStatus.Inactive;

			_context.Update(person);

			await _context.SaveChangesAsync();

			return person!.User.Id;
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - SignOut");
		}
	}
}
