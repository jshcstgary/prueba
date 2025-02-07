using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models.Dtos.Role;

public class RoleCreateDto
{
    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = null!;
}
