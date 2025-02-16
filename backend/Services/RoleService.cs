using System.Linq.Expressions;

using AutoMapper;

using PruebaViamaticaBackend.Constants;
using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Role;
using PruebaViamaticaBackend.Repository.Interfaces;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Services;

public class RoleService(ILogger<IRoleService> logger, IRoleRepository repository, IMapper mapper) : IRoleService
{
	private readonly ILogger<IRoleService> _logger = logger;

	private readonly IRoleRepository _repository = repository;

	private readonly IMapper _mapper = mapper;

	public async Task<RoleDto> Create(RoleCreateDto roleCreateDto)
	{
		_logger.LogInformation("Executing Service class - Create method");

		try
		{
			Role role = _mapper.Map<Role>(roleCreateDto);

			role.Status = Status.Active;

			Role newRole = await _repository.Create(role);

			return _mapper.Map<RoleDto>(newRole);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Service class - Create method");
		}
	}

	public async Task<IEnumerable<RoleDto>> GetAll(Expression<Func<Role, bool>>? filter = null)
	{
		_logger.LogInformation("Executing Service class - GetAll method");

		try
		{
			IEnumerable<Role> roles = await _repository.GetAll(filter);

			return _mapper.Map<IEnumerable<RoleDto>>(roles);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Service class - GetAll method");
		}
	}

	public async Task<RoleDto?> GetOne(Expression<Func<Role, bool>> filter)
	{
		_logger.LogInformation("Executing Service class - GetOne method");

		try
		{
			Role? role = await _repository.GetOne(filter);

			return _mapper.Map<RoleDto>(role);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Service class - GetOne method");
		}
	}

	public async Task<RoleDto?> Update(RoleDto roleDto)
	{
		_logger.LogInformation("Executing Service class - Update method");

		try
		{
			RoleDto? roleDtoFound = await GetOne(role => role.Id == roleDto.Id);

			if (roleDtoFound == null)
			{
				return null;
			}

			Role role = _mapper.Map<Role>(roleDto);

			Role? roleUpdated = await _repository.Update(role);

			if (roleUpdated == null)
			{
				return null;
			}

			RoleDto roleDtoUpdated = _mapper.Map<RoleDto>(roleUpdated);

			return roleDtoUpdated;
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Service class - Update method");
		}
	}

	public async Task<bool> Delete(int id)
	{
		_logger.LogInformation("Executing Service class - Delete method");

		try
		{
			Role? role = await _repository.GetOne(role => role.Id == id);

			if (role == null)
			{
				return false;
			}

			role.Status = Status.Delete;

			await _repository.Delete(role!);

			return true;
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Service class - Delete method");
		}
	}
}
