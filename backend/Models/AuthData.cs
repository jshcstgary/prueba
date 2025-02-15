using System.ComponentModel.DataAnnotations;

namespace PruebaViamaticaBackend.Models;

public class AuthData
{
    [Required]
    public string Username { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public int IdRole { get; set; }
}
