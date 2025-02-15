using AutoMapper;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;
using PruebaViamaticaBackend.Repository.Interfaces;
using PruebaViamaticaBackend.Services.Interfaces;

namespace PruebaViamaticaBackend.Services;

public class AuthService(ILogger<IAuthService> logger, IAuthRepository repository, IMapper mapper) : IAuthService
{
	private readonly ILogger<IAuthService> _logger = logger;

	private readonly IAuthRepository _repository = repository;

	private readonly IMapper _mapper = mapper;

	public async Task<PersonDto> SignIn(AuthData authData)
	{
		_logger.LogInformation("Executing Service class - SignIn method");

		try
		{
			Person? person = await _repository.SignIn(authData);

			return _mapper.Map<PersonDto>(person);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			_logger.LogInformation("Leaving Service class - SignIn method");
		}
	}
}
