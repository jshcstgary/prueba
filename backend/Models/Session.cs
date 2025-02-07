﻿namespace PruebaViamaticaBackend.Models;

public partial class Session
{
	public int Id { get; set; }

	public DateTime EntryDate { get; set; }

	public DateTime CloseDate { get; set; }

	public int IdUser { get; set; }

	public virtual User IdUserNavigation { get; set; } = null!;
}
