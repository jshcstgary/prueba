using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using PruebaViamaticaBackend.Data;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Repository.Interfaces;

namespace PruebaViamaticaBackend.Repository;

public class RoleRepository : IRoleRepository
{
	private readonly ILogger<IRoleRepository> _logger;

	private readonly AppDbContext _context;

	protected readonly DbSet<Role> dbSet;

	public RoleRepository(ILogger<IRoleRepository> logger, AppDbContext context)
	{
		_logger = logger;
		_context = context;
		dbSet = _context.Set<Role>();
	}

	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}

	public async Task<Role> Create(Role role)
	{
		_logger.LogInformation("Executing Repository class - Create method");

		try
		{
			EntityEntry<Role> entityEntry = await dbSet.AddAsync(role);

			foreach (RoleOption roleOption in role.RoleOptions)
			{
				_context.Set<RoleOption>().Attach(roleOption);
				_context.Entry(roleOption).State = EntityState.Unchanged;
			}

			await Save();

			return entityEntry.Entity;
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

	public async Task<IEnumerable<Role>> GetAll(Expression<Func<Role, bool>>? filter = null)
	{
		_logger.LogInformation("Executing Repository class - GetAll method");

		try
		{
			IQueryable<Role> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query
				.Include(role => role.RoleOptions)
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

	public async Task<Role?> GetOne(Expression<Func<Role, bool>> filter)
	{
		_logger.LogInformation("Executing Repository class - GetOne method");

		try
		{
			return await dbSet
				.Include(role => role.RoleOptions)
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

	public async Task<Role?> Update(Role role)
	{
		_logger.LogInformation("Executing Repository class - Update method");

		try
		{
			Role? existingRole = await dbSet
				.Include(role => role.RoleOptions)
				.FirstOrDefaultAsync(r => r.Id == role.Id);

			if (existingRole == null)
			{
				return null;
			}

			existingRole.Name = role.Name;
			existingRole.Status = role.Status;

			List<int> newRoleOptionIds = [.. role.RoleOptions.Select(roleOption => roleOption.Id)];
			List<int> currentRoleOptionIds = [.. existingRole.RoleOptions.Select(ro => ro.Id)];

			List<RoleOption> roleOptionsToRemove = [.. existingRole.RoleOptions.Where(roleOption => !newRoleOptionIds.Contains(roleOption.Id))];

			foreach (RoleOption roleOptionToRemove in roleOptionsToRemove)
			{
				existingRole.RoleOptions.Remove(roleOptionToRemove);
			}

			List<RoleOption> roleOptionsToAdd = [.. role.RoleOptions.Where(roleOption => !currentRoleOptionIds.Contains(roleOption.Id))];

			foreach (RoleOption roleOptionToAdd in roleOptionsToAdd)
			{
				if (!_context.RoleOptions.Local.Any(roleOption => roleOption.Id == roleOptionToAdd.Id))
				{
					_context.Attach(roleOptionToAdd);
				}

				existingRole.RoleOptions.Add(roleOptionToAdd);
			}

			await Save();

			return existingRole;
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

	public async Task Delete(Role role)
	{
		_logger.LogInformation("Executing Repository class - Delete method");

		try
		{
			dbSet.Remove(role);

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
