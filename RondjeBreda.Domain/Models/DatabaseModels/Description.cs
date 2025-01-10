using RondjeBreda.Domain.Interfaces;
using SQLite;
using SQLiteNetExtensions.Attributes;
namespace RondjeBreda.Domain.Models.DatabaseModels;

/// <summary>
/// A dataclass for a description object
/// </summary>
[Table("Description")]
public class Description : IDatabaseTable
{
    [PrimaryKey]
    public string DescriptionNL { get; set; }

    public string DescriptionEN { get; set; }

    [ForeignKey(typeof(Location))]
    public double LocationLongitude { get; set; } // Part of foreign key

    [ForeignKey(typeof(Location))]
    public double LocationLatitude { get; set; } // Part of foreign key

    [OneToOne]
    public Location Location { get; set; }
}