namespace PruebaViamaticaBackend.Models;

public partial class Person
{
	public int Id { get; set; }

	public string Names { get; set; } = null!;

	public string Surnames { get; set; } = null!;

	public string Identification { get; set; } = null!;

	public DateTime BirthDate { get; set; }

	public virtual User User { get; set; } = new();
}
