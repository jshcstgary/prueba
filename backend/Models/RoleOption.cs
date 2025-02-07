namespace PruebaViamaticaBackend.Models;

public partial class RoleOption
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<Role> IdRols { get; set; } = new List<Role>();
}
