using System.Linq.Expressions;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;

namespace PruebaViamaticaBackend.Services.Interfaces;

public interface IPersonService
{
    // Task<PersonDto> Create(PersonCreateDto personCreateDto);
    Task<int?> Create(PersonCreateDto personCreateDto);
    Task<IEnumerable<PersonDto>> GetAll(Expression<Func<Person, bool>>? filter = null);
    Task<PersonDto?> GetOne(Expression<Func<Person, bool>> filter);
    Task<PersonDto?> Update(PersonUpdateDto personUpdateDto);
    Task<bool> Delete(int id);
}
