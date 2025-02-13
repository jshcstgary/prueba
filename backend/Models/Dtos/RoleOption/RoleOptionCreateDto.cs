using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models.Dtos.RoleOption;

public class RoleOptionCreateDto
{
    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = null!;
}
