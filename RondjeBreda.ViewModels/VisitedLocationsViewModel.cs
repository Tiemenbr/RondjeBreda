using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using RondjeBreda.Domain.Interfaces;
using System.Collections.ObjectModel;
using RondjeBreda.ViewModels.DataModels;
using System.ComponentModel;
using LocalizationResourceManager.Maui;

namespace RondjeBreda.ViewModels;

/// <summary>
/// The Viewmodel for the visitedlocationpage
/// </summary>
public partial class VisitedLocationsViewModel : ObservableObject
{
    private IDatabase database;
    private IPreferences preferences;
    private IPopUp popUp;
    private ILocalizationResourceManager resourceManager;

    [ObservableProperty]
    private ObservableCollection<LocationViewModel> visitedLocations;

    public VisitedLocationsViewModel(IDatabase database, IPreferences preferences, IPopUp popUp,
        ILocalizationResourceManager resourceManager) {
        this.database = database;
        this.preferences = preferences;
        this.resourceManager = resourceManager;
        VisitedLocations = new ObservableCollection<LocationViewModel>();
        this.popUp = popUp;
    }

    public async Task LoadTestData()
    {
        VisitedLocations.Clear();
        var locations = await database.GetVisitedRouteComponentsFromRouteAsync("Historische Kilometer");
        foreach (var component in locations)
        {
            Domain.Models.DatabaseModels.Location location =
            await database.GetLocationAsync(component.LocationLongitude, component.LocationLatitude);

            VisitedLocations.Add(new LocationViewModel()
            {
                Name = location.Name,
                Latitude = location.Latitude,
                Longitude = location.Longitude,
                ImagePath = location.ImagePath,
                RouteOrderNumber = component.RouteOrderNumber
            });
        }
    }

    [RelayCommand]
    private void LocationTapped(LocationViewModel location)
    {
        popUp.ShowPopUpAsync(
            location.ImagePath, 
            location.Name, 
            location.Description,
            $"{location.Latitude},{location.Longitude}",
            resourceManager["popupButton"]);
    }
}