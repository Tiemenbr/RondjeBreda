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
    [PrimaryKey]
    [ForeignKey(typeof(Route))]
    public Route Route { get; set; }

    [PrimaryKey]
    [ForeignKey(typeof(Location))]
    public Location Location { get; set; }

    public string Note { get; set; }

    public bool Visited { get; set; }

    public int RouteOrderNumber { get; set; }
}