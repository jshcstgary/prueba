using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models.Dtos.RoleOption;

public class RoleOptionUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = null!;
}
