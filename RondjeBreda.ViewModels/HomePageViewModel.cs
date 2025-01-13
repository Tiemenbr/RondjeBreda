using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models.DatabaseModels;
using Location = RondjeBreda.ViewModels.DataModels.LocationViewModel;
using Microsoft.Maui.Maps;
using RondjeBreda.Domain.Models;
using Distance = Microsoft.Maui.Maps.Distance;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;

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
    private IPopUp popUp;
    private Domain.Models.DatabaseModels.Route selectedDatabaseRoute;
    private bool routePaused;
    public double userLat, userLon;
    private List<ViewModels.DataModels.LocationViewModel> routePoints;
    private Location nextLocation;
    private int indexRoute = 0;

    //TODO: event aanroepen door update() wel methodes toevoegen
    public event Action UpdateMapSpan;
    public event Action UpdatePins;
    public event Action UpdateMapElements;

    [ObservableProperty] private ObservableCollection<Domain.Models.DatabaseModels.Route> routes;
    [ObservableProperty] private ObservableCollection<Pin> pins;
    [ObservableProperty] private ObservableCollection<Polyline> polylines;
    [ObservableProperty] private Circle rangeCircle;
    [ObservableProperty] private MapSpan currentMapSpan;


    public HomePageViewModel(IDatabase database, IPreferences preferences, IMapsAPI mapsAPI, IGeolocation geolocation,
        IPopUp popUp)
    {
        pins = new ObservableCollection<Pin>();
        polylines = new ObservableCollection<Polyline>();

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
        this.popUp = popUp;
    }


    private void LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        this.userLat = e.Location.Latitude;
        this.userLon = e.Location.Longitude;

        if (nextLocation == null)
        {
            return;
        }

        if (Microsoft.Maui.Devices.Sensors.Location.CalculateDistance(userLat, userLon,
                nextLocation.Latitude, nextLocation.Longitude, DistanceUnits.Kilometers) <= 0.02)
        {
            LocationReached();
        }
    }

    private async Task LocationReached()
    {
        if (routePoints.Count == 0)
        {
            return;
        }

        this.indexRoute++;
        if (indexRoute >= routePoints.Count)
        {
            indexRoute = 0;
        }

        await popUp.ShowPopUpAsync(nextLocation.ImagePath,
            nextLocation.Name,
            $"{nextLocation.Latitude},{nextLocation.Longitude}");

        this.nextLocation = routePoints[this.indexRoute];

        // TODO: picker moet route inladen
        LoadRoute();
        DrawCircleNextLocation();
        SetMapSpan();
    }

    private async void LoadRoute()
    {
        // Punten van de geselecteerde route laden
        this.routePoints = await LoadPoints();
        if (nextLocation == null)
        {
            this.nextLocation = routePoints[0];
        }

        // Punten toevoegen aan de map
        Pins.Clear();
        foreach (var location in routePoints)
        {
            Pins.Add(new Pin
            {
                Label = location.Name,
                Location = new Microsoft.Maui.Devices.Sensors.Location(location.Latitude, location.Longitude)
            });
        }

        Polylines.Clear();
        // DatabaseRoute genereren tussen de punten
        foreach (var location in routePoints)
        {
            if (routePoints.IndexOf(location) == routePoints.Count - 1)
            {
                continue;
            }

            var locationCount = routePoints.IndexOf(location);
            var route = await mapsAPI.CreateRoute($"{location.Latitude}", $"{location.Longitude}",
                routePoints[locationCount + 1].Latitude.ToString(),
                routePoints[locationCount + 1].Longitude.ToString());

            Polyline polyline = new Polyline
            {
                StrokeColor = Colors.Blue,
                StrokeWidth = 12,
            };

            var locations = mapsAPI.Decode(route.routes[0].overview_polyline.points);
            foreach (var tempLocation in locations)
            {
                polyline.Geopath.Add(
                    new Microsoft.Maui.Devices.Sensors.Location(tempLocation.Latitude, tempLocation.Longitude));
            }

            // Lijn toevoegen aan de map
            Polylines.Add(polyline);
        }

        // DatabaseRoute to the next point of the route
        var routeToFirstPoint = await mapsAPI.CreateRoute($"{userLat}", $"{userLon}",
            $"{nextLocation.Latitude}", $"{nextLocation.Longitude}");
        Polyline firstpolyline = new Polyline
        {
            StrokeColor = Colors.Chartreuse,
            StrokeWidth = 12,
        };
        var firstlocations = mapsAPI.Decode(routeToFirstPoint.routes[0].overview_polyline.points);
        foreach (var tempLocation in firstlocations)
        {
            firstpolyline.Geopath.Add(
                new Microsoft.Maui.Devices.Sensors.Location(tempLocation.Latitude, tempLocation.Longitude));
        }

        Polylines.Add(firstpolyline);

        UpdateMapElements();
        UpdatePins();
    }

    private void DrawCircleNextLocation()
    {
        if (this.routePoints == null || this.routePoints.Count == 0)
        {
            return;
        }


        foreach (var location in routePoints)
        {
            // TODO: Check if location is visited, first one that don't need to be set
        }

        Circle circle = new Circle
        {
            Center = new Microsoft.Maui.Devices.Sensors.Location(nextLocation.Latitude, nextLocation.Longitude),
            Radius = new Distance(20),
            StrokeColor = Color.FromArgb("#CFffc61e"),
            StrokeWidth = 8,
            FillColor = Color.FromArgb("#CFffc61e")
        };

        this.rangeCircle = circle;
        UpdateMapElements();
    }

    private void SetMapSpan()
    {
        var centerLat = (this.nextLocation.Latitude + userLat) / 2;
        var centerLon = (this.nextLocation.Longitude + userLon) / 2;

        var center = new Microsoft.Maui.Devices.Sensors.Location(centerLat, centerLon);

        var distance = Microsoft.Maui.Devices.Sensors.Location.CalculateDistance(
            new Microsoft.Maui.Devices.Sensors.Location(this.nextLocation.Latitude,
                this.nextLocation.Longitude), center, DistanceUnits.Kilometers);

        MapSpan mapSpan = MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(distance * 1.5));

        CurrentMapSpan = mapSpan;
        UpdateMapSpan();
    }

    private void SetOverviewMapSpan()
    {
        var minLatitude = Pins.Min(p => p.Location.Latitude);
        var maxLatitude = Pins.Max(p => p.Location.Latitude);
        var minLongitude = Pins.Min(p => p.Location.Longitude);
        var maxLongitude = Pins.Max(p => p.Location.Longitude);

        var centerLatitude = (minLatitude + maxLatitude) / 2;
        var centerLongitude = (minLongitude + maxLongitude) / 2;
        var center = new Microsoft.Maui.Devices.Sensors.Location(centerLatitude, centerLongitude);

        var distance = Math.Max(
            Microsoft.Maui.Devices.Sensors.Location.CalculateDistance(
                minLatitude, minLongitude, maxLatitude, maxLongitude, DistanceUnits.Kilometers), 1
        );

        MapSpan mapSpan = MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(distance));
        CurrentMapSpan = mapSpan;
        UpdateMapSpan();
        UpdatePins();
        UpdateMapElements();
    }

    [RelayCommand]
    private void ImageButtonPressed()
    {
        if (routePaused)
        {
            Polylines.Clear();
            LoadRoute();
            DrawCircleNextLocation();
            SetMapSpan();
        }
        else
        {
            Polylines.Clear();
            selectedDatabaseRoute = new Domain.Models.DatabaseModels.Route();
            LoadRoute();
            SetOverviewMapSpan();
        }
        routePaused = !routePaused;
    }

    public async Task<List<Location>> LoadPoints()
    {
        // TODO: DatabaseRoute component tabel goed ophalen
        // database.GetDatabaseTableAsync();

        // Testdata
        var testList = new List<Location>();
        testList.Add(new Location
            { Latitude = 51.594445, Longitude = 4.779417, Name = "Oude VVV-pand", ImagePath = "dotnet_bot.png" });
        testList.Add(new Location
            { Latitude = 51.593278, Longitude = 4.779388, Name = "Liefdeszuster", ImagePath = "location_3.png" });
        testList.Add(new Location
        {
            Latitude = 51.592500, Longitude = 4.779695, Name = "Nassau Baronie Monument", ImagePath = "location_4.png"
        });
        testList.Add(new Location
            { Latitude = 51.592833, Longitude = 4.778472, Name = "The Light House", ImagePath = "location_5.png" });

        UpdatePins();
        return testList;
    }

    public async Task LoadRouteFromDatabase()
    {
        // TODO: DatabaseRoute tabel goed ophalen
        // database.GetDatabaseTableAsync();

        // Testdata
        var testList = new ObservableCollection<Domain.Models.DatabaseModels.Route>();
        testList.Add(new Domain.Models.DatabaseModels.Route
        {
            Name = "Historische Kilometer",
            Active = false
        });

        Routes = testList;
        this.routePaused = true;
    }

    public void routeSelected()
    {
        Polylines.Clear();
        selectedDatabaseRoute = new Domain.Models.DatabaseModels.Route();
        LoadRoute();
        SetOverviewMapSpan();
    }
}