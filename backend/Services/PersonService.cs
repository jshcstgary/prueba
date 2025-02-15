using System.Linq.Expressions;

using AutoMapper;

using PruebaViamaticaBackend.Constants;
using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Repository.Interfaces;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Services;

public class PersonService(ILogger<IPersonService> logger, IPersonRepository repository, IMapper mapper) : IPersonService
{
	private readonly ILogger<IPersonService> _logger = logger;

	private readonly IPersonRepository _repository = repository;

	private readonly IMapper _mapper = mapper;

    // public async Task<PersonDto> Create(PersonCreateDto personCreateDto)
    public async Task<RowsChanged> Create(ICollection<PersonCreateDto> personsCreateDto)
	{
		_logger.LogInformation("Executing Service class - Create method");

		try
		{
			ICollection<Person> persons = _mapper.Map<ICollection<Person>>(personsCreateDto);

			foreach (Person person in persons)
			{
				person.User!.Mail = $"{person.Names.ToLower().First()}{person.Surnames.ToLower().Split(" ")[0]}{person.Surnames.ToLower().Split(" ")[1].First()}@mail.com";
				person.User!.Status = Status.Active;
				person.User!.SessionActive = SessionStatus.Inactive;
			}

			RowsChanged rowsChanged = await _repository.Create(persons);

			return rowsChanged;
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

	public async Task<IEnumerable<PersonCount>> GetCount()
	{
		_logger.LogInformation("Executing Service class - GetCount method");

		try
		{
			return await _repository.GetCount();
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Service class - GetCount method");
		}
	}

	public async Task<PersonDto?> GetOne(Expression<Func<Person, bool>> filter)
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

	public async Task<PersonDto?> Update(PersonUpdateDto personUpdateDto)
	{
		_logger.LogInformation("Executing Service class - Update method");

		try
		{
			PersonDto? personFound = await GetOne(person => person.Id == personUpdateDto.Id);

			if (personFound == null)
			{
				return null;
			}

			Person person = _mapper.Map<Person>(personUpdateDto);

			Person personUpdated = await _repository.Update(person);

			return _mapper.Map<PersonDto>(personUpdated);
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

			person.User!.Status = Status.Delete;

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
