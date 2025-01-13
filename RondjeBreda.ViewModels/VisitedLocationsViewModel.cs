using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RondjeBreda.Domain.Interfaces;
using System.Collections.ObjectModel;
using RondjeBreda.ViewModels.DataModels;

namespace RondjeBreda.ViewModels;

/// <summary>
/// The Viewmodel for the visitedlocationpage
/// </summary>
public partial class VisitedLocationsViewModel : ObservableObject
{
    private IDatabase database;
    private IPreferences preferences;

    [ObservableProperty]
    private ObservableCollection<LocationViewModel> visitedLocations;

    public VisitedLocationsViewModel(IDatabase database, IPreferences preferences) {
        this.database = database;
        this.preferences = preferences;
        VisitedLocations = new ObservableCollection<LocationViewModel>();
    }

    public async Task LoadTestData() {
        VisitedLocations.Add(new LocationViewModel
        {
            Name = "Avans Hogeschool",
            ImagePath = "avans.jpg",
            Latitude = 51.58595100591575,
            Longitude = 4.792345494769091
        });
    }

    [RelayCommand]
    private void LocationTapped(LocationViewModel location) {
        // TODO
        Console.WriteLine($"Location tapped: " +
            $"\n{location.Name}" +
            $"\n\t{location.Longitude}" +
            $"\n\t{location.Latitude}");
    }
}