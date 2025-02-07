using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models.Dtos.Role;

public class RoleOptionCreateDto
{
    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = null!;
}
