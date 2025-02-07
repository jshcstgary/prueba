using System.Linq.Expressions;

using AutoMapper;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Repository.Interfaces;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Services;

public class PersonService : IPersonService
{
    private readonly ILogger<IPersonService> _logger;

    private readonly IPersonRepository _repository;

    private readonly IMapper _mapper;

    public PersonService(ILogger<IPersonService> logger, IPersonRepository repository, IMapper mapper)
    {
        _logger = logger;
        _repository = repository;
        _mapper = mapper;
    }

    // public async Task<PersonDto> Create(PersonCreateDto personCreateDto)
    public async Task<int?> Create(PersonCreateDto personCreateDto)
    {
        _logger.LogInformation("Executing Service class - Create method");

        try
        {
            Person person = _mapper.Map<Person>(personCreateDto);

            person.User!.Mail = $"{person.Names.ToLower().First()}{person.Surnames.ToLower().Split(" ")[0]}{person.Surnames.ToLower().Split(" ")[1].First()}@mail.com";
            person.User!.Status = "ACTIVATED";

            // Person person = await _repository.Create(model);
            int? idPerson = await _repository.Create(person);

            // return _mapper.Map<PersonDto>(person);
            return idPerson;
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

    public async Task<IEnumerable<PersonDto>> GetAll(Expression<Func<Person, bool>>? filter = null)
    {
        _logger.LogInformation("Executing Service class - GetAll method");

        try
        {
            IEnumerable<Person> persons = await _repository.GetAll(filter);

            return _mapper.Map<IEnumerable<PersonDto>>(persons);
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

    public async Task<PersonDto?> GetOne(Expression<Func<Person, bool>>? filter = null)
    {
        _logger.LogInformation("Executing Service class - GetOne method");

        try
        {
            Person? person = await _repository.GetOne(filter);

            return _mapper.Map<PersonDto>(person);
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

    public async Task<PersonDto?> Update(PersonDto personDto)
    {
        _logger.LogInformation("Executing Service class - Update method");

        try
        {
            PersonDto? personFound = await this.GetOne(person => person.Id == personDto.Id);

            if (personFound == null)
            {
                return null;
            }

            Person person = _mapper.Map<Person>(personDto);

            Person personUpdated = await _repository.Update(person);

            PersonDto personDtoUpdated = _mapper.Map<PersonDto>(personUpdated);

            return personDtoUpdated;
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
            Person? person = await _repository.GetOne(person => person.Id == id);

            if (person == null)
            {
                return false;
            }

            await _repository.Delete(person!);

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
