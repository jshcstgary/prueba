using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using PruebaViamaticaBackend.Data;

using Microsoft.Data.SqlClient;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Repository.Interfaces;
using PruebaViamaticaBackend.Constants;

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

	// public async Task<int?> Create(Person newPerson)
	public async Task<Person> Create(Person newPerson)
	{
		_logger.LogInformation("Executing Repository class - Create method");

		try
		{
			var names = new SqlParameter("@Names", System.Data.SqlDbType.NVarChar, 60)
			{
				Value = newPerson.Names,
				Direction = System.Data.ParameterDirection.Input
			};

			var surnames = new SqlParameter("@Surnames", System.Data.SqlDbType.NVarChar, 60)
			{
				Value = newPerson.Surnames,
				Direction = System.Data.ParameterDirection.Input
			};

			var identification = new SqlParameter("@Identification", System.Data.SqlDbType.NVarChar, 10)
			{
				Value = newPerson.Identification,
				Direction = System.Data.ParameterDirection.Input
			};

			var birthDate = new SqlParameter("@BirthDate", System.Data.SqlDbType.DateTime)
			{
				Value = newPerson.BirthDate,
				Direction = System.Data.ParameterDirection.Input
			};

			var username = new SqlParameter("@Username", System.Data.SqlDbType.NVarChar, 50)
			{
				Value = newPerson.User!.Username,
				Direction = System.Data.ParameterDirection.Input
			};

			var password = new SqlParameter("@Password", System.Data.SqlDbType.NVarChar, 50)
			{
				Value = newPerson.User.Password,
				Direction = System.Data.ParameterDirection.Input
			};

			var mail = new SqlParameter("@Mail", System.Data.SqlDbType.NVarChar, 120)
			{
				Value = newPerson.User.Mail,
				Direction = System.Data.ParameterDirection.Input
			};

			var sessionActive = new SqlParameter("@SessionActive", System.Data.SqlDbType.Bit)
			{
				Value = newPerson.User.SessionActive,
				Direction = System.Data.ParameterDirection.Input
			};

			var status = new SqlParameter("@Status", System.Data.SqlDbType.NVarChar, 20)
			{
				Value = newPerson.User.Status,
				Direction = System.Data.ParameterDirection.Input
			};

			var idPerson = new SqlParameter("@IdPerson", System.Data.SqlDbType.Int)
			{
				Direction = System.Data.ParameterDirection.Output
			};

			var idUser = new SqlParameter("@IdUser", System.Data.SqlDbType.Int)
			{
				Direction = System.Data.ParameterDirection.Output
			};

			int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
				"EXEC INSERT_PERSON_USER @Names, @Surnames, @Identification, @BirthDate, @Username, @Password, @Mail, @SessionActive, @Status, @IdPerson OUTPUT, @IdUser OUTPUT",
				names, surnames, identification, birthDate, username, password, mail, sessionActive, status, idPerson, idUser
			);

			if (rowsAffected == 0)
			{
				throw new Exception("Could not insert record.");
			}

			Person person = new Person
			{
				Id = (int)idPerson.Value,
				Identification = newPerson.Identification,
				Names = newPerson.Names,
				Surnames = newPerson.Surnames,
				BirthDate = newPerson.BirthDate,
				User = new User
				{
					Id = (int)idUser.Value,
					Username = newPerson.User!.Username,
					Password = newPerson.User.Password,
					Mail = newPerson.User.Mail,
					Status = newPerson.User.Status,
					SessionActive = newPerson.User.SessionActive,
					IdNavigation = newPerson.User.IdNavigation,
					IdPerson = newPerson.User.IdPerson,
					Roles = newPerson.User.Roles,
					Sessions = newPerson.User.Sessions
				}
			};

			// return idPerson.Value != DBNull.Value ? (int?)idPerson.Value : null;
			return person;
		}
		catch (SqlException ex)
		{
			if (ex.Number == 50001)
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("El número de identificación ya existe en el sistema.");
			}
			else if (ex.Number == 50002)
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("El nombre de usuario ya existe en el sistema.");
			}
			else
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("Un error ocurrió durante la inserción.");
			}
		}
		catch (Exception)
		{
			throw new Exception("Un error ocurrió durante la inserción.");
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
				.AsNoTracking()
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

	public async Task<Person?> GetOne(Expression<Func<Person, bool>> filter)
	{
		_logger.LogInformation("Executing Repository class - GetOne method");

		try
		{
			return await dbSet
				.Include(person => person.User)
				.AsNoTracking()
				.FirstOrDefaultAsync(filter);
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
			var idPerson = new SqlParameter("@IdPerson", System.Data.SqlDbType.Int)
			{
				Value = person.Id,
				Direction = System.Data.ParameterDirection.Input
			};

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

			var birthDate = new SqlParameter("@BirthDate", System.Data.SqlDbType.DateTime)
			{
				Value = person.BirthDate,
				Direction = System.Data.ParameterDirection.Input
			};

			var idUser = new SqlParameter("@IdUser", System.Data.SqlDbType.Int)
			{
				Value = person.User!.Id,
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

			int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
				"EXEC UPDATE_PERSON_USER @IdPerson, @Names, @Surnames, @BirthDate, @IdUser, @Username, @Password, @SessionActive, @Status",
				idPerson, names, surnames, birthDate, idUser, username, password, sessionActive, status
			);

			return person;
		}
		catch (SqlException ex)
		{
			if (ex.Number == 50001)
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("El nombre de usuario ya existe en el sistema.");
			}
			else
			{
				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				throw new Exception("Un error ocurrió durante la inserción.");
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
			person.User!.Status = Status.Delete;

			_context.Update(person);

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
