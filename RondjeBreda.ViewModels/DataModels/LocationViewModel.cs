using CommunityToolkit.Mvvm.ComponentModel;

namespace RondjeBreda.ViewModels.DataModels;

/// <summary>
/// A dataclass for a location object
/// </summary>


public partial class LocationViewModel : ObservableObject, IComparable
{
    [ObservableProperty] public double longitude;
    [ObservableProperty] public double latitude;
    [ObservableProperty] public string description;
    [ObservableProperty] public string imagePath;
    [ObservableProperty] public string name;
    public int RouteOrderNumber { get; set; }


    public int CompareTo(object? obj)
    {
        if (obj is not LocationViewModel other)
        {
            return 1;
        }
        
        return RouteOrderNumber.CompareTo(other.RouteOrderNumber);
    }
}