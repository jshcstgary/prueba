using System.ComponentModel.DataAnnotations;

using PruebaViamaticaBackend.Models.Dtos.Role;

namespace PruebaViamaticaBackend.Models.Dtos.User;

public class UserDto
{
	[Required]
	public int Id { get; set; }

	[Required]
	[MaxLength(20)]
	public string Username { get; set; } = null!;

	[Required]
	[MaxLength(120)]
	public string Mail { get; set; } = null!;

	[Required]
	public bool SessionActive { get; set; }

	[Required]
	[MaxLength(20)]
	public string Status { get; set; } = null!;

	[Required]
	public ICollection<RoleDto> Roles { get; set; } = [];
}
