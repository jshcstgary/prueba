using System.Linq.Expressions;

using AutoMapper;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Models.Dtos.Role;
using PruebaViamaticaBackend.Repository.Interfaces;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Services;

public class RoleService : IRoleService
{
    private readonly ILogger<IRoleService> _logger;

    private readonly IRoleRepository _repository;

    private readonly IMapper _mapper;

    public RoleService(ILogger<IRoleService> logger, IRoleRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RoleDto> Create(RoleCreateDto roleCreateDto)
    {
        _logger.LogInformation("Executing Service class - Create method");

        try
        {
            Role model = _mapper.Map<Role>(roleCreateDto);

            Role role = await _repository.Create(model);

            return _mapper.Map<RoleDto>(role);
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

    public async Task<RoleDto?> GetOne(Expression<Func<Role, bool>>? filter = null)
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
            RoleDto? roleFound = await this.GetOne(role => role.Id == roleDto.Id);

            if (roleFound == null)
            {
                return null;
            }

            Role role = _mapper.Map<Role>(roleDto);

            Role roleUpdated = await _repository.Update(role);

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
