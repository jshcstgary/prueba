using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;

namespace PruebaViamaticaBackend.Services.Interfaces;

public interface IAuthService
{
	Task<PersonDto> SignIn(AuthData authData);

	Task SetSession(int idUser, bool status);

	Task<int> SignOut(int idPerson);
}
