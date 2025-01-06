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

    [Column("DescriptionNL")]
    [PrimaryKey]
    public string DescriptionNL { get; set; }

    [Column("DescriptionEN")]
    public string DescriptionEN { get; set; }

    [Column("Location")]
    [ForeignKey(typeof(Location))]
    public Location Location { get; set; }
}