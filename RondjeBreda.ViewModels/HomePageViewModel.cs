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
    private IGeolocation geolocation;
    private string route;
    // private Map map; TODO
    private bool routePaused;

    [ObservableProperty] private ObservableCollection<Route> routes;

    public HomePageViewModel(IDatabase database, IPreferences preferences, IGeolocation geolocation)
    {
        this.database = database;
        this.preferences = preferences;
        this.geolocation = geolocation;
    }

    public async Task<List<Location>> LoadPoints()
    {   // TODO: Route component tabel goed ophalen
        // database.GetDatabaseTableAsync();

        // Testdata
        var testList = new List<Location>();
        testList.Add(new Location{latitude = 51.594445, longitude = 4.779417, name = "Oude VVV-pand", imagePath = "location_2.png"});
        testList.Add(new Location{latitude = 51.593278, longitude = 4.779388, name = "Liefdeszuster", imagePath = "location_3.png"});
        testList.Add(new Location{latitude = 51.592500, longitude = 4.779695, name = "Nassau Baronie Monument", imagePath = "location_4.png"});

        return testList;
    }
}