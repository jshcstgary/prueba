using System.Linq.Expressions;

using AutoMapper;

using PruebaViamaticaBackend.Constants;
using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.RoleOption;
using PruebaViamaticaBackend.Repository.Interfaces;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Services;

public class RoleOptionService : IRoleOptionService
{
    private readonly ILogger<IRoleOptionService> _logger;

    private readonly IRoleOptionRepository _repository;

    private readonly IMapper _mapper;

    public RoleOptionService(ILogger<IRoleOptionService> logger, IRoleOptionRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RoleOptionDto> Create(RoleOptionCreateDto roleOptionCreateDto)
    {
        _logger.LogInformation("Executing Service class - Create method");

        try
        {
            RoleOption roleOption = _mapper.Map<RoleOption>(roleOptionCreateDto);

            roleOption.Status = Status.Active;

            RoleOption newRoleOption = await _repository.Create(roleOption);

            return _mapper.Map<RoleOptionDto>(newRoleOption);
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

    public async Task<IEnumerable<RoleOptionDto>> GetAll(Expression<Func<RoleOption, bool>>? filter = null)
    {
        _logger.LogInformation("Executing Service class - GetAll method");

        try
        {
            IEnumerable<RoleOption> roleOptions = await _repository.GetAll(filter);

            return _mapper.Map<IEnumerable<RoleOptionDto>>(roleOptions);
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

    public async Task<RoleOptionDto?> GetOne(Expression<Func<RoleOption, bool>> filter)
    {
        _logger.LogInformation("Executing Service class - GetOne method");

        try
        {
            RoleOption? roleOption = await _repository.GetOne(filter);

            return _mapper.Map<RoleOptionDto>(roleOption);
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

    public async Task<RoleOptionDto?> Update(RoleOptionDto roleOptionDto)
    {
        _logger.LogInformation("Executing Service class - Update method");

        try
        {
            RoleOptionDto? roleOptionFound = await GetOne(roleOption => roleOption.Id == roleOptionDto.Id);

            if (roleOptionFound == null)
            {
                return null;
            }

            RoleOption roleOption = _mapper.Map<RoleOption>(roleOptionDto);

            RoleOption roleOptionUpdated = await _repository.Update(roleOption);

            return _mapper.Map<RoleOptionDto>(roleOptionUpdated);
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
            RoleOption? roleOption = await _repository.GetOne(roleOption => roleOption.Id == id);

            if (roleOption == null)
            {
                return false;
            }

            await _repository.Delete(roleOption!);

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
