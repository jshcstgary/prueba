using System.ComponentModel.DataAnnotations;

using PruebaViamaticaBackend.Models.Dtos.Role;

namespace PruebaViamaticaBackend.Models.Dtos.User;

public class UserCreateDto
{
	[Required]
	[MaxLength(20)]
	[MinLength(8)]
	[RegularExpression(@"^(?=.*[A-Z])(?=.*\d)[A-Za-z0-9]{8,20}$", ErrorMessage = "The field Username must contain at least one uppercase letter, one lowercase letter and one number.")]
	public string Username { get; set; } = null!;

	[Required]
	[MaxLength(50)]
	[MinLength(8)]
	[RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_])(?=\S).*", ErrorMessage = "The field Password must contain at least one uppercase letter, a special sign and no spaces.")]
	public string Password { get; set; } = null!;

	[Required]
	public ICollection<RoleDto> Roles { get; set; } = [];
}
