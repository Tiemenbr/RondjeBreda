using RondjeBreda.Domain.Interfaces;
using SQLite;

namespace RondjeBreda.Domain.Models.DatabaseModels;

/// <summary>
/// A dataclass for a route object
/// </summary>

[Table("Route")]
public class Route : IDatabaseTable
{
    [PrimaryKey]
    public string Name { get; set; }

    public bool Active { get; set; }
    public override string ToString()
    {
        return Name;
    }
}
