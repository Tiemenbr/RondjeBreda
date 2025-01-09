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
    public Location Location { get; set; }
}