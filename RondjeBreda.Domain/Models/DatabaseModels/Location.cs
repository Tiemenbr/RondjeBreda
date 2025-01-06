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
    [Column("Longitude")]
    [PrimaryKey]
    public double Longitude { get; set; }

    [Column("Latitude")]
    [PrimaryKey]
    public double Latitud { get; set; }

    [Column("Description")]
    [ForeignKey(typeof(Description))]
    public Description Description { get; set; }

    [Column("ImagePath")]
    public string ImagePath { get; set; }

    [Column("Name")]
    public string Name { get; set; }
}