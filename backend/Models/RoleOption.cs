namespace PruebaViamaticaBackend.Models;

public partial class RoleOption
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public string Status { get; set; } = null!;

	public virtual ICollection<Role> IdRoles { get; set; } = [];
}
