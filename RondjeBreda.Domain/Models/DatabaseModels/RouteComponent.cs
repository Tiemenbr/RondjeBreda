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
    public string RouteName { get; set; } // Foreign key and composite key

    public double LocationLongitude { get; set; } // Foreign key and composite key
    public double LocationLatitude { get; set; } // Foreign key and composite key

    public string Note { get; set; }

    public bool Visited { get; set; }

    public int RouteOrderNumber { get; set; }

    [ManyToOne]
    public Route route { get; set; } 

    [ManyToOne]
    public Location location { get; set; }
}