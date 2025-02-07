using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using PruebaViamaticaBackend.Data;

using Microsoft.Data.SqlClient;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Repository.Interfaces;

namespace PruebaViamaticaBackend.Repository;

public class PersonRepository : IPersonRepository
{
	private readonly ILogger<IPersonRepository> _logger;

	private readonly AppDbContext _context;

	protected readonly DbSet<Person> dbSet;

	public PersonRepository(ILogger<IPersonRepository> logger, AppDbContext context)
	{
		_logger = logger;
		_context = context;
		dbSet = _context.Set<Person>();
	}

	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}

	// public async Task<Person> Create(Person person)
	public async Task<int?> Create(Person person)
	{
		_logger.LogInformation("Executing Repository class - Create method");

		try
		{
			person.User!.SessionActive = false;

			var names = new SqlParameter("@Names", System.Data.SqlDbType.NVarChar, 60)
			{
				Value = person.Names,
				Direction = System.Data.ParameterDirection.Input
			};

			var surnames = new SqlParameter("@Surnames", System.Data.SqlDbType.NVarChar, 60)
			{
				Value = person.Surnames,
				Direction = System.Data.ParameterDirection.Input
			};

			var identification = new SqlParameter("@Identification", System.Data.SqlDbType.NVarChar, 10)
			{
				Value = person.Identification,
				Direction = System.Data.ParameterDirection.Input
			};

			var birthDate = new SqlParameter("@BirthDate", System.Data.SqlDbType.DateTime)
			{
				Value = person.BirthDate,
				Direction = System.Data.ParameterDirection.Input
			};

			var username = new SqlParameter("@Username", System.Data.SqlDbType.NVarChar, 50)
			{
				Value = person.User.Username,
				Direction = System.Data.ParameterDirection.Input
			};

			var password = new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 50)
			{
				Value = person.User.Password,
				Direction = System.Data.ParameterDirection.Input
			};

			var mail = new SqlParameter("@Mail", System.Data.SqlDbType.NVarChar, 120)
			{
				Value = person.User.Mail,
				Direction = System.Data.ParameterDirection.Input
			};

			var sessionActive = new SqlParameter("@SessionActive", System.Data.SqlDbType.Bit)
			{
				Value = person.User.SessionActive,
				Direction = System.Data.ParameterDirection.Input
			};

			var status = new SqlParameter("@Status", System.Data.SqlDbType.NVarChar, 20)
			{
				Value = person.User.Status,
				Direction = System.Data.ParameterDirection.Input
			};

			var idPerson = new SqlParameter("@IdPerson", System.Data.SqlDbType.Int)
			{
				Direction = System.Data.ParameterDirection.Output
			};

			await _context.Database.ExecuteSqlRawAsync(
				"EXEC INSERT_PERSON_USER @Names, @Surnames, @Identification, @BirthDate, @Username, @Password, @Mail, @SessionActive, @Status, @IdPerson OUTPUT",
				names, surnames, identification, birthDate, username, password, mail, sessionActive, status, idPerson
			);

			return idPerson.Value != DBNull.Value ? (int?)idPerson.Value : null;

			// return entityEntry.Entity;
		}
		catch (SqlException ex)
		{
			if (ex.Number == 50001)
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("The identification number already exists in the system.");
			}
			else if (ex.Number == 50002)
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("The username already exists in the system.");
			}
			else
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("An error occurred while processing the  request.");
			}
		}
		catch (Exception)
		{
			throw new Exception("An error occurred while processing the  request.");
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - Create method");
		}
	}

	public async Task<IEnumerable<Person>> GetAll(Expression<Func<Person, bool>>? filter = null)
	{
		_logger.LogInformation("Executing Repository class - GetAll method");

		try
		{
			IQueryable<Person> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query
				.Include(person => person.User)
				.ToListAsync();
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - GetAll method");
		}
	}

	public async Task<Person?> GetOne(Expression<Func<Person, bool>>? filter = null)
	{
		_logger.LogInformation("Executing Repository class - GetOne method");

		try
		{
			IQueryable<Person> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query
				.Include(person => person.User)
				.FirstOrDefaultAsync();
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - GetOne method");
		}
	}

	public async Task<Person> Update(Person person)
	{
		_logger.LogInformation("Executing Repository class - Update method");

		try
		{
			EntityEntry<Person> entityEntry = _context.Update(person);

			await Save();

			return entityEntry.Entity;
		}
		catch (SqlException ex)
		{
			if (ex.Number == 50001)
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("The username already exists in the system.");
			}
			else
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("An error occurred while processing the  request.");
			}
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - Update method");
		}
	}

	public async Task Delete(Person person)
	{
		_logger.LogInformation("Executing Repository class - Delete method");

		try
		{
			dbSet.Remove(person);

			await Save();
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - Delete method");
		}
	}
}
