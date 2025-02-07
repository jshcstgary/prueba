namespace PruebaViamaticaBackend.Models;

public partial class Role
{
	public int Id { get; set; }

	public string Name { get; set; } = null!;

	public virtual ICollection<RolOption> IdRolOptions { get; set; } = new List<RolOption>();

	public virtual ICollection<User> IdUsers { get; set; } = new List<User>();
}
