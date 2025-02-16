using System.Data;

using System.Linq.Expressions;

using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

using PruebaViamaticaBackend.Constants;
using PruebaViamaticaBackend.Data;
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

	public async Task<RowsChanged> Create(ICollection<Person> newPersons)
	{
		_logger.LogInformation("Executing Repository class - Create method");

		RowsChanged rowsChanged = new();

		foreach (Person newPerson in newPersons)
		{
			try
			{
				string idRoles = string.Join(",", newPerson.User.Roles.Select(role => role.Id));

				SqlParameter namesParam = new("@Names", SqlDbType.NVarChar, 60)
				{
					Value = newPerson.Names,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter surnamesParam = new("@Surnames", SqlDbType.NVarChar, 60)
				{
					Value = newPerson.Surnames,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter identificationParam = new("@Identification", SqlDbType.NVarChar, 10)
				{
					Value = newPerson.Identification,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter birthDateParam = new("@BirthDate", SqlDbType.DateTime)
				{
					Value = newPerson.BirthDate,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter usernameParam = new("@Username", SqlDbType.NVarChar, 50)
				{
					Value = newPerson.User.Username,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter passwordParam = new("@Password", SqlDbType.NVarChar, 50)
				{
					Value = newPerson.User.Password,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter mailParam = new("@Mail", SqlDbType.NVarChar, 120)
				{
					Value = newPerson.User.Mail,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter sessionActiveParam = new("@SessionActive", SqlDbType.Bit)
				{
					Value = newPerson.User.SessionActive,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter statusParam = new("@Status", SqlDbType.NVarChar, 20)
				{
					Value = newPerson.User.Status,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter idRolesParam = new("@IdRoles", SqlDbType.NVarChar, 200)
				{
					Value = idRoles,
					Direction = System.Data.ParameterDirection.Input
				};

				SqlParameter idPersonParam = new("@IdPerson", SqlDbType.Int)
				{
					Direction = System.Data.ParameterDirection.Output
				};

				SqlParameter idUserParam = new("@IdUser", SqlDbType.Int)
				{
					Direction = System.Data.ParameterDirection.Output
				};

				int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
					"EXEC INSERT_PERSON_USER @Names, @Surnames, @Identification, @BirthDate, @Username, @Password, @Mail, @SessionActive, @Status, @IdRoles, @IdPerson OUTPUT, @IdUser OUTPUT",
					namesParam, surnamesParam, identificationParam, birthDateParam, usernameParam, passwordParam, mailParam, sessionActiveParam, statusParam, idRolesParam, idPersonParam, idUserParam
				);

				if (rowsAffected == 0)
				{
					rowsChanged.RowsNotInserted++;
					rowsChanged.IdentificationsNotInserted.Add([newPerson.Identification, "No se pudo realizar la inserciónn."]);

					_logger.LogWarning($"Person not inserted: {newPerson.Identification}");

					continue;
				}

				rowsChanged.RowsInserted++;
			}
			catch (SqlException ex)
			{
				rowsChanged.RowsNotInserted++;

				if (ex.Number == 50001)
				{
					_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

					rowsChanged.IdentificationsNotInserted.Add([newPerson.Identification, "El número de identificación ya existe en el sistema."]);
				}
				else if (ex.Number == 50002)
				{
					_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

					rowsChanged.IdentificationsNotInserted.Add([newPerson.Identification, "El nombre de usuario ya existe en el sistema."]);
				}
				else
				{
					_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

					rowsChanged.IdentificationsNotInserted.Add([newPerson.Identification, ex.Message]);
				}

				continue;
			}
			catch (Exception ex)
			{
				rowsChanged.RowsNotInserted++;
				rowsChanged.IdentificationsNotInserted.Add([newPerson.Identification, ex.Message]);

				_logger.LogError($"PersonRepository class - Create method - {ex.Message}");

				continue;
			}
		}

		_logger.LogInformation("Leaving Repository class - Create method");

		return rowsChanged; ;
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
					.ThenInclude(user => user!.Roles)
						.ThenInclude(role => role.RoleOptions)
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

	public async Task<IEnumerable<PersonCount>> GetCount()
	{
		_logger.LogInformation("Executing Repository class - GetCount method");

		try
		{
			int allUsers = await _context.Persons.CountAsync();

			int activeUsers = await _context.Persons
				.Where(person => person.User.Status == Status.Active)
				.CountAsync();

			int deletedUsers = await _context.Persons
				.Where(person => person.User.Status == Status.Delete)
				.CountAsync();

			int lockedUsers = await _context.Persons
				.Where(person => person.User.Status == Status.Lock)
				.CountAsync();

			int activeSessionUsers = await _context.Persons
				.Where(person => person.User.SessionActive == SessionStatus.Active)
				.CountAsync();

			int inactiveSessionUsers = await _context.Persons
				.Where(person => person.User.SessionActive == SessionStatus.Inactive)
				.CountAsync();

			return [
				new PersonCount()
				{
					Label = "Todos los usuarios",
					Amount = allUsers
				},
				new PersonCount()
				{
					Label = "Usuarios activos",
					Amount = activeUsers
				},
				new PersonCount()
				{
					Label = "Usuarios bloqueados",
					Amount = lockedUsers
				},
				new PersonCount()
				{
					Label = "Usuarios eliminados",
					Amount = deletedUsers
				},
				new PersonCount()
				{
					Label = "Usuarios con sesión activa",
					Amount = activeSessionUsers
				},
				new PersonCount()
				{
					Label = "Usuarios con sesión inactiva",
					Amount = inactiveSessionUsers
				}
			];
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Repository class - GetCount method");
		}
	}

	public async Task<Person?> GetOne(Expression<Func<Person, bool>> filter)
	{
		_logger.LogInformation("Executing Repository class - GetOne method");

		try
		{
			return await dbSet
				.Include(person => person.User)
					.ThenInclude(user => user!.Roles)
						.ThenInclude(role => role.RoleOptions)
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
			string idRoles = string.Join(",", person.User.Roles.Select(role => role.Id));

			SqlParameter idPersonParam = new("@IdPerson", SqlDbType.Int)
			{
				Value = person.Id,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter identificationParam = new("@Identification", SqlDbType.NVarChar, 10)
			{
				Value = person.Identification,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter namesParam = new("@Names", SqlDbType.NVarChar, 60)
			{
				Value = person.Names,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter surnamesParam = new("@Surnames", SqlDbType.NVarChar, 60)
			{
				Value = person.Surnames,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter birthDateParam = new("@BirthDate", SqlDbType.DateTime)
			{
				Value = person.BirthDate,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter idUserParam = new("@IdUser", SqlDbType.Int)
			{
				Value = person.User.Id,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter usernameParam = new("@Username", SqlDbType.NVarChar, 50)
			{
				Value = person.User.Username,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter passwordParam = new("@Password", SqlDbType.NVarChar, 50)
			{
				Value = person.User.Password,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter sessionActiveParam = new("@SessionActive", SqlDbType.Bit)
			{
				Value = person.User.SessionActive,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter statusParam = new("@Status", SqlDbType.NVarChar, 20)
			{
				Value = person.User.Status,
				Direction = System.Data.ParameterDirection.Input
			};

			SqlParameter idRolesParam = new("@IdRoles", SqlDbType.NVarChar, 200)
			{
				Value = idRoles,
				Direction = System.Data.ParameterDirection.Input
			};

			int rowsAffected = await _context.Database.ExecuteSqlRawAsync(
				"EXEC UPDATE_PERSON_USER @IdPerson, @Identification, @Names, @Surnames, @BirthDate, @IdUser, @Username, @Password, @SessionActive, @Status, @IdRoles",
				idPersonParam, identificationParam, namesParam, surnamesParam, birthDateParam, idUserParam, usernameParam, passwordParam, sessionActiveParam, statusParam, idRolesParam
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
