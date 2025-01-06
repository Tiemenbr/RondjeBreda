using RondjeBreda.Domain.Interfaces;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RondjeBreda.Domain.Models.DatabaseModels;

/// <summary>
/// A dataclass for a routecomponent object
/// </summary>
[Table("RouteComponent")]
public class RouteComponent : IDatabaseTable
{
    [Column("Route")]
    [PrimaryKey]
    [ForeignKey(typeof(Route))]
    public Route Route { get; set; }

    [Column("Location")]
    [PrimaryKey]
    [ForeignKey(typeof(Location))]
    public Location Location { get; set; }

    [Column("Note")]
    public string Note { get; set; }

    [Column("Visited")]
    public bool Visited { get; set; }

    [Column("RouteOrderNumber")]
    public int RouteOrderNumber { get; set; }
}