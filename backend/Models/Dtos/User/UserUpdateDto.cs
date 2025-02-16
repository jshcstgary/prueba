using System.ComponentModel.DataAnnotations;

using PruebaViamaticaBackend.Models.Dtos.Role;

namespace PruebaViamaticaBackend.Models.Dtos.User;

public class UserUpdateDto
{
	[Required]
	public int Id { get; set; }

	[Required]
	[MaxLength(60)]
	public string Username { get; set; } = null!;

	[Required]
	[MaxLength(120)]
	public string Mail { get; set; } = null!;

	[Required(AllowEmptyStrings = true)]
	[RegularExpression(@"^(?=.*[A-Z])(?=.*[\W_])(?=\S).*", ErrorMessage = "The field Password must contain at least one uppercase letter, a special sign and no spaces.")]
	public string Password { get; set; } = null!;

	[Required]
	public bool SessionActive { get; set; }

	[Required]
	[MaxLength(20)]
	public string Status { get; set; } = null!;

	[Required]
	public ICollection<RoleDto> Roles { get; set; } = [];
}
