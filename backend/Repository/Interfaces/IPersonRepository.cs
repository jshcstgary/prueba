using System.Linq.Expressions;

using PruebaViamaticaBackend.Models;

namespace PruebaViamaticaBackend.Repository.Interfaces;

public interface IPersonRepository
{
	Task<Person> Create(Person person);

	Task<IEnumerable<Person>> GetAll(Expression<Func<Person, bool>>? filter = null);

	Task<IEnumerable<PersonCount>> GetCount();

	Task<Person?> GetOne(Expression<Func<Person, bool>> filter);

	Task<Person> Update(Person person);

	Task Delete(Person person);

	Task Save();
}
