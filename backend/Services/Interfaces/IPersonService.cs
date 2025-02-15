using System.Linq.Expressions;

using PruebaViamaticaBackend.Models;
using PruebaViamaticaBackend.Models.Dtos.Person;

namespace PruebaViamaticaBackend.Services.Interfaces;

public interface IPersonService
{
    // Task<PersonDto> Create(PersonCreateDto personCreateDto);
    Task<RowsChanged> Create(ICollection<PersonCreateDto> personsCreateDto);

    Task<IEnumerable<PersonDto>> GetAll(Expression<Func<Person, bool>>? filter = null);

    Task<IEnumerable<PersonCount>> GetCount();

    Task<PersonDto?> GetOne(Expression<Func<Person, bool>> filter);

    Task<PersonDto?> Update(PersonUpdateDto personUpdateDto);

    Task<bool> Delete(int id);
}
