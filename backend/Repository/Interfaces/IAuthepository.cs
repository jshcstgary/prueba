using PruebaViamaticaBackend.Models;

namespace PruebaViamaticaBackend.Repository.Interfaces;

public interface IAuthRepository
{
	Task<Person?> SignIn(AuthData authData);

	Task SetSession(int idUser, bool status);

	Task<int> SignOut(int idPerson);
}
