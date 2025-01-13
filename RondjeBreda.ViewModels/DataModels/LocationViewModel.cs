using CommunityToolkit.Mvvm.ComponentModel;

namespace RondjeBreda.ViewModels.DataModels;

/// <summary>
/// A dataclass for a location object
/// </summary>


public partial class LocationViewModel : ObservableObject
{
    [ObservableProperty] public double longitude;
    [ObservableProperty] public double latitude;
    [ObservableProperty] public string description;
    [ObservableProperty] public string imagePath;
    [ObservableProperty] public string name;
}