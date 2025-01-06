using RondjeBreda.Domain.Interfaces;
using SQLite;

namespace RondjeBreda.Domain.Models.DatabaseModels;

/// <summary>
/// A dataclass for a route object
/// </summary>

[Table("Route")]
public class Route : IDatabaseTable
{
    [Column("Name")]
    [PrimaryKey]
    public string Name { get; set; }

    [Column("Active")]
    public bool Active { get; set; }
}
