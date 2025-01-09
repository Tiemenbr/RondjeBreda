using RondjeBreda.Domain.Interfaces;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace RondjeBreda.Domain.Models.DatabaseModels;

/// <summary>
/// A dataclass for a location object
/// </summary>

[Table("Location")]
public class Location : IDatabaseTable
{
    [PrimaryKey]
    public double Longitude { get; set; }

    [PrimaryKey]
    public double Latitude { get; set; }

    [ForeignKey(typeof(Description))]
    public Description Description { get; set; }

    public string ImagePath { get; set; }

    public string Name { get; set; }
}