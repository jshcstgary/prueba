namespace PruebaViamaticaBackend.Models;

public partial class RoleOption
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public string? Status { get; set; }

	public string Link { get; set; } = null!;

	public virtual ICollection<Role> Roles { get; set; } = [];
}
