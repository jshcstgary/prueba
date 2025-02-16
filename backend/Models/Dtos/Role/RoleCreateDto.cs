using System.ComponentModel.DataAnnotations;

using PruebaViamaticaBackend.Models.Dtos.RoleOption;

namespace PruebaViamaticaBackend.Models.Dtos.Role;

public class RoleCreateDto
{
	[Required]
	[MaxLength(60)]
	public string Name { get; set; } = null!;

	[Required]
	public ICollection<RoleOptionDto> RoleOptions { get; set; } = [];
}
