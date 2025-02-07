using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models.Dtos.User;

public class UserCreateDto
{
    [Required]
    [MaxLength(60)]
    public string Username { get; set; } = null!;

    [Required]
    [MaxLength(60)]
    public string Password { get; set; } = null!;
}
