using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models.Dtos.RoleOption;

public class RoleOptionDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = null!;
}
