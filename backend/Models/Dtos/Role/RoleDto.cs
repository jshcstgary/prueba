using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models.Dtos.Role;

public class RoleDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = null!;
}
