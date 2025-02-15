using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

using PruebaViamaticaBackend.Models.Dtos.User;

namespace PruebaViamaticaBackend.Models.Dtos.Person;

public class PersonCreateDto : IValidatableObject
{
	[Required]
	[MaxLength(60)]
	public string Names { get; set; } = null!;

	[Required]
	[MaxLength(60)]
	public string Surnames { get; set; } = null!;

	[Required]
	[StringLength(10)]
	[RegularExpression(@"^\d{10}$", ErrorMessage = "The field Identification must contain only digits.")]
	public string Identification { get; set; } = null!;

	[Required]
	public DateTime? BirthDate { get; set; } = null!;

	[Required]
	public UserCreateDto User { get; set; } = null!;

	public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
	{
		if (Regex.IsMatch(Identification, @"(\d)\1{3}"))
		{
			yield return new ValidationResult("The field Identification must not contain four numbers repeated consecutively.", [nameof(Identification)]);
		}
	}
}
