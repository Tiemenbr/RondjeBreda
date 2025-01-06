namespace RondjeBreda.Domain.Models.DatabaseModels;

/// <summary>
/// A dataclass for a location object
/// </summary>
public class Location
{
    public double longitude {  get; set; }
    public double latitude {  get; set; }
    public string imagePath {  get; set; }
    public string name { get; set; }
}