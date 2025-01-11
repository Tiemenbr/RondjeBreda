﻿using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models.DatabaseModels;
using Location = RondjeBreda.Domain.Models.DatabaseModels.Location;
using Microsoft.Maui.Maps;

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
    private string route;
    private bool routePaused;
    public double userLat, userLon;
    private List<Domain.Models.DatabaseModels.Location> routePoints;
    private Location nextLocation;
    private int indexRoute = 0;

    //TODO: event aanroepen door update() wel methodes toevoegen
    public event Action UpdateMapSpan;
    public event Action UpdatePins;
    public event Action UpdateMapElements;

    [ObservableProperty] private ObservableCollection<Route> routes;
    [ObservableProperty] private ObservableCollection<Pin> pins;
    [ObservableProperty] private ObservableCollection<Polyline> polylines;
    [ObservableProperty] private Circle rangeCircle;
    [ObservableProperty] private MapSpan currentMapSpan;


    public HomePageViewModel(IDatabase database, IPreferences preferences, IMapsAPI mapsAPI, IGeolocation geolocation, IPopUp popUp)
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
                nextLocation.latitude, nextLocation.longitude, DistanceUnits.Kilometers) <= 0.02)
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

        await popUp.ShowPopUpAsync(this.nextLocation.name, 
            $"{this.nextLocation.name}, \n {this.nextLocation.latitude}, {this.nextLocation.longitude}");
        this.nextLocation = routePoints[this.indexRoute];

        // TODO: picker moet route inladen
        LoadRoute(new Route());
        DrawCircleNextLocation();
        SetMapSpan();
    }

    private async void LoadRoute(Route selectedRoute)
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
                Label = location.name,
                Location = new Microsoft.Maui.Devices.Sensors.Location(location.latitude, location.longitude)
            });
        }

        Polylines.Clear();
        // Route genereren tussen de punten
        foreach (var location in routePoints)
        {
            if (routePoints.IndexOf(location) == routePoints.Count - 1)
            {
                continue;
            }

            var locationCount = routePoints.IndexOf(location);
            var route = await mapsAPI.CreateRoute($"{location.latitude}", $"{location.longitude}",
                routePoints[locationCount + 1].latitude.ToString(),
                routePoints[locationCount + 1].longitude.ToString());

            Polyline polyline = new Polyline
            {
                StrokeColor = Colors.Blue,
                StrokeWidth = 12,
            };

            var locations = mapsAPI.Decode(route.routes[0].overview_polyline.points);
            foreach (var tempLocation in locations)
            {
                polyline.Geopath.Add(new Microsoft.Maui.Devices.Sensors.Location(tempLocation.latitude, tempLocation.longitude));
            }

            // Lijn toevoegen aan de map
            Polylines.Add(polyline);
        }

        // Route to the next point of the route
        var routeToFirstPoint = await mapsAPI.CreateRoute($"{userLat}", $"{userLon}",
            $"{nextLocation.latitude}", $"{nextLocation.longitude}");
        Polyline firstpolyline = new Polyline
        {
            StrokeColor = Colors.Chartreuse,
            StrokeWidth = 12,
        };
        var firstlocations = mapsAPI.Decode(routeToFirstPoint.routes[0].overview_polyline.points);
        foreach (var tempLocation in firstlocations)
        {
            firstpolyline.Geopath.Add(new Microsoft.Maui.Devices.Sensors.Location(tempLocation.latitude, tempLocation.longitude));
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
            Center = new Microsoft.Maui.Devices.Sensors.Location(nextLocation.latitude, nextLocation.longitude),
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
        var centerLat = (this.nextLocation.latitude + userLat) / 2;
        var centerLon = (this.nextLocation.longitude + userLon) / 2;

        var center = new Microsoft.Maui.Devices.Sensors.Location(centerLat, centerLon);

        var distance = Microsoft.Maui.Devices.Sensors.Location.CalculateDistance(new Microsoft.Maui.Devices.Sensors.Location(this.nextLocation.latitude, 
            this.nextLocation.longitude), center, DistanceUnits.Kilometers);

        MapSpan mapSpan = MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(distance * 1.5));

        CurrentMapSpan = mapSpan;
        UpdateMapSpan();
    }

    [RelayCommand]
    private void ImageButtonPressed()
    {
        Polylines.Clear();
        LoadRoute(new Route());
        DrawCircleNextLocation();
        SetMapSpan();
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

        UpdatePins();
        return testList;
    }
}