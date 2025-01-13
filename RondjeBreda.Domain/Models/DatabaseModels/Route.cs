namespace RondjeBreda.Domain.Models.DatabaseModels;

/// <summary>
/// A dataclass for a route object
/// </summary>
public class Route
{
    public string name;
    public bool active;

    public override string ToString()
    {
        return name;
    }
}