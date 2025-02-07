using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models.Dtos.User;

public class UserDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(120)]
    public string Mail { get; set; } = null!;

    [Required]
    [MaxLength(1)]
    public string SessionActive { get; set; } = null!;

    [Required]
    [MaxLength(20)]
    public string Status { get; set; } = null!;
}
