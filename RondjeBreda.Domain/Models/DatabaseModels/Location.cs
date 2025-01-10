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
    public double Longitude { get; set; } //part of composite key
    public double Latitude { get; set; } //part of composite key

    [ForeignKey(typeof(Description))]
    public string DescriptionNL { get; set; } //foreign Key

    public string ImagePath { get; set; }

    public string Name { get; set; }

    [OneToOne]
    public Description description { get; set; }
}