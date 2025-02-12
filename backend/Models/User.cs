namespace PruebaViamaticaBackend.Models;

public partial class User
{
	public int Id { get; set; }

	public string Username { get; set; } = null!;

	public string Password { get; set; } = null!;

	public string Mail { get; set; } = null!;

	public bool SessionActive { get; set; }

	public string Status { get; set; } = null!;

	public int IdPerson { get; set; }

	public virtual Person IdNavigation { get; set; } = null!;

	public virtual ICollection<Session> Sessions { get; set; } = [];

	public virtual ICollection<Role> Roles { get; set; } = [];
}
