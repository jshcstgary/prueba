using System.ComponentModel.DataAnnotations;
using PruebaViamaticaBackend.Models.Dtos.RoleOption;

namespace PruebaViamaticaBackend.Models.Dtos.Role;

public class RoleDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = null!;

    [Required]
    public ICollection<RoleOptionDto> RoleOptions { get; set; } = [];
}
