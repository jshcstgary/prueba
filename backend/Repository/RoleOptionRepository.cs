using System.Linq.Expressions;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

using PruebaViamaticaBackend.Data;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Repository.Interfaces;

namespace PruebaViamaticaBackend.Repository;

public class RoleOptionRepository : IRoleOptionRepository
{
	private readonly ILogger<IRoleOptionRepository> _logger;

	private readonly AppDbContext _context;

	protected readonly DbSet<RoleOption> dbSet;

	public RoleOptionRepository(ILogger<IRoleOptionRepository> logger, AppDbContext context)
	{
		_logger = logger;
		_context = context;
		dbSet = _context.Set<RoleOption>();
	}

	public async Task Save()
	{
		await _context.SaveChangesAsync();
	}

	public async Task<RoleOption> Create(RoleOption roleOption)
	{
		_logger.LogInformation("Executing Repository class - Create method");

		try
		{
			EntityEntry<RoleOption> entityEntry = await dbSet.AddAsync(roleOption);

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

	public async Task<IEnumerable<RoleOption>> GetAll(Expression<Func<RoleOption, bool>>? filter = null)
	{
		_logger.LogInformation("Executing Repository class - GetAll method");

		try
		{
			IQueryable<RoleOption> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.ToListAsync();
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

	public async Task<RoleOption?> GetOne(Expression<Func<RoleOption, bool>>? filter = null)
	{
		_logger.LogInformation("Executing Repository class - GetOne method");

		try
		{
			IQueryable<RoleOption> query = dbSet;

			if (filter != null)
			{
				query = query.Where(filter);
			}

			return await query.FirstOrDefaultAsync();
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

	public async Task<RoleOption> Update(RoleOption roleOption)
	{
		_logger.LogInformation("Executing Repository class - Update method");

		try
		{
			EntityEntry<RoleOption> entityEntry = _context.Update(roleOption);

			await Save();

			return entityEntry.Entity;
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

	public async Task Delete(RoleOption roleOption)
	{
		_logger.LogInformation("Executing Repository class - Delete method");

		try
		{
			dbSet.Remove(roleOption);

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
