using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models.DatabaseModels;
using Location = RondjeBreda.Domain.Models.DatabaseModels.Location;

namespace RondjeBreda.ViewModels;

/// <summary>
/// The Viewmodel for the homepage
/// </summary>
public partial class HomePageViewModel : ObservableObject
{
    private IDatabase database;
    private IPreferences preferences;
    public IGeolocation geolocation;
    public IMapsAPI mapsAPI;
    private string route;
    // private Map map; TODO
    private bool routePaused;
    public double userLat, userLon;

    [ObservableProperty] private ObservableCollection<Route> routes;

    public HomePageViewModel(IDatabase database, IPreferences preferences, IMapsAPI mapsAPI, IGeolocation geolocation)
    {
        this.database = database;
        this.preferences = preferences;
        this.mapsAPI = mapsAPI;
        this.geolocation = geolocation;

        this.geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest
        {
            MinimumTime = TimeSpan.FromSeconds(10),
            DesiredAccuracy = GeolocationAccuracy.Best
        });

        this.geolocation.LocationChanged += LocationChanged;
    }

    private void LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        this.userLat = e.Location.Latitude;
        this.userLon = e.Location.Longitude;
    }


    public async Task<List<Location>> LoadPoints()
    {   // TODO: Route component tabel goed ophalen
        // database.GetDatabaseTableAsync();

        // Testdata
        var testList = new List<Location>();
        testList.Add(new Location{latitude = 51.594445, longitude = 4.779417, name = "Oude VVV-pand", imagePath = "location_2.png"});
        testList.Add(new Location{latitude = 51.593278, longitude = 4.779388, name = "Liefdeszuster", imagePath = "location_3.png"});
        testList.Add(new Location{latitude = 51.592500, longitude = 4.779695, name = "Nassau Baronie Monument", imagePath = "location_4.png"});
        testList.Add(new Location{latitude = 51.592833, longitude = 4.778472, name = "The Light House", imagePath = "location_5.png"});

        return testList;
    }
}