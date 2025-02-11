using System.ComponentModel.DataAnnotations;

using PruebaViamaticaBackend.Models.Dtos.User;

namespace PruebaViamaticaBackend.Models.Dtos.Person;

public class PersonUpdateDto
{
    [Required]
    public int Id { get; set; }

    [Required]
    [MaxLength(60)]
    public string Names { get; set; } = null!;

    [Required]
    [MaxLength(60)]
    public string Surnames { get; set; } = null!;

    [Required]
    [StringLength(10)]
    public string Identification { get; set; } = null!;

    [Required]
    public DateTime? BirthDate { get; set; } = null!;

    [Required]
    public UserUpdateDto User { get; set; } = null!;
}
