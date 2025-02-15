using System.Collections;

namespace PruebaViamaticaBackend.Models;

public class RowsChanged
{
    public int RowsInserted { get; set; }

    public int RowsNotInserted { get; set; }

    public ICollection<ICollection<string>> IdentificationsNotInserted { get; set; } = [];
}
