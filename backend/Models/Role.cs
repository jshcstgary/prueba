namespace PruebaViamaticaBackend.Models;

public partial class Role
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public string? Status { get; set; }

	public virtual ICollection<RoleOption> RoleOptions { get; set; } = [];

	public virtual ICollection<User> Users { get; set; } = [];
}
