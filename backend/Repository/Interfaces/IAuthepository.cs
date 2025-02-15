using PruebaViamaticaBackend.Models;

namespace PruebaViamaticaBackend.Repository.Interfaces;

public interface IAuthRepository
{
	Task<Person?> SignIn(AuthData authData);
}
