using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using LocalizationResourceManager.Maui;
using Microsoft.Maui.Controls.Maps;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models.DatabaseModels;
using Location = RondjeBreda.ViewModels.DataModels.LocationViewModel;
using Microsoft.Maui.Maps;
using RondjeBreda.Domain.Models;
using Distance = Microsoft.Maui.Maps.Distance;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;
using RondjeBreda.Infrastructure;
using RondjeBreda.Infrastructure.SettingsImplementation;

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
    private ILocalizationResourceManager localizationResourceManager;

    private Domain.Models.DatabaseModels.Route selectedDatabaseRoute;
    [ObservableProperty] private bool routePaused = true;
    public double userLat, userLon;
    private List<ViewModels.DataModels.LocationViewModel> routePoints;
    private Location nextLocation;
    private int indexRoute = 0;
    private bool isListening = false;
    private Polyline currentPolyline = null;
    private bool popupOffTrackActive = false;

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
        IPopUp popUp, ILocalizationResourceManager localizationResourceManager)
    {
        pins = new ObservableCollection<Pin>();
        polylines = new ObservableCollection<Polyline>();

        this.database = database;
        this.preferences = preferences;
        this.mapsAPI = mapsAPI;
        this.geolocation = geolocation;
        this.popUp = popUp;
        this.localizationResourceManager = localizationResourceManager;
    }

    /// <summary>
    /// Starts listener for location changes.
    /// </summary>
    public async Task StartListening()
    {
        if (isListening)
            return;

        try
        {
            await geolocation.StartListeningForegroundAsync(new GeolocationListeningRequest
            {
                MinimumTime = TimeSpan.FromSeconds(10),
                DesiredAccuracy = GeolocationAccuracy.Best
            });
            geolocation.LocationChanged += LocationChanged;
            isListening = true;
        }
        catch (Exception)
        {
            Debug.WriteLine("Geolocation not available");
            await popUp.ShowPopUpAsync("",
                "",
                "Location not available!",
                "Please turn on your location or give the app permissions!",
                localizationResourceManager["popupButton"]);
        }
    }
    /// <summary>
    /// Handles geolocation location changed event.
    /// </summary>
    private void LocationChanged(object? sender, GeolocationLocationChangedEventArgs e)
    {
        Debug.WriteLine($"Location Changed");
        this.userLat = e.Location.Latitude;
        this.userLon = e.Location.Longitude;

        if (Polylines.Count == 0)
        {
            return;
        }

        // Check if the user is near the current polyline (in green)
        Microsoft.Maui.Devices.Sensors.Location userLocation = new Microsoft.Maui.Devices.Sensors.Location(userLat, userLon);
        bool isOnPolyline = currentPolyline.Geopath.Any(location => 
            Microsoft.Maui.Devices.Sensors.Location.CalculateDistance(userLocation, location, DistanceUnits.Kilometers) <= 0.025); // Detect 25 meters or more from the polyline
        if (!isOnPolyline)
        {
            if (!popupOffTrackActive)
            {
                popUp.ShowPopUpAsync(
                    "", 
                    "Hey!", 
                    localizationResourceManager["OffTrack"], 
                    "", 
                    localizationResourceManager["popupButton"]);

                popupOffTrackActive = true;
            }
        } else
        {
            popupOffTrackActive = false;
        }

        if (nextLocation == null)
        {
            Debug.WriteLine("Next Location is null");
            return;
        }

        CheckLocationReached();
    }

    private void CheckLocationReached()
    {
        if (Microsoft.Maui.Devices.Sensors.Location.CalculateDistance(userLat, userLon,
                nextLocation.Latitude, nextLocation.Longitude, DistanceUnits.Kilometers) <= 0.02)
        {
            Debug.WriteLine("Location Reached");
            LocationReached();
        }
    }

    /// <summary>
    /// Shows popup after a location has been reached.
    /// </summary>
    private async Task LocationReached()
    {
        if (routePoints.Count == 0)
        {
            return;
        }

        if (routePaused)
        {
            return;
        }
        
        this.indexRoute++;
        if (indexRoute >= routePoints.Count)
        {
            indexRoute = 0;
            foreach (var location in routePoints)
            {
                database.UpdateRouteComponent("Historische Kilometer", location.longitude, location.latitude, false);
            }
            return;
        }

        if (nextLocation.Description.StartsWith("NoDescription"))
        {
            nextLocation.Description = localizationResourceManager["NoDescription"];
        }
        else
        {
            var lan = preferences.Get("Language", "Nederlands");
            if (lan == "English")
            {
                var tempDescription = await database.GetDescriptionAsync(nextLocation.Description);
                nextLocation.Description = tempDescription.DescriptionEN;
            }
        }

        if (nextLocation.Name.Contains("(rechter zijde)"))
        {
            nextLocation.Name = nextLocation.Name.Replace("rechter zijde", localizationResourceManager["RightSide"]);
        } else if (nextLocation.Name.Contains("(kunst)"))
        {
            nextLocation.Name = nextLocation.Name.Replace("kunst", localizationResourceManager["Art"]);
        } else if (nextLocation.Name.Contains("Einde stadswandeling"))
        {
            nextLocation.Name = nextLocation.Name.Replace("Einde stadswandeling", localizationResourceManager["End"]);
        }

        var textToSpeech = new TextToSpeechSetting();
        textToSpeech.Speak(nextLocation.Name);
        textToSpeech.Speak(nextLocation.Description);

        await popUp.ShowPopUpAsync(
            nextLocation.ImagePath,
            nextLocation.Name,
            nextLocation.Description,
            $"{nextLocation.Latitude},{nextLocation.Longitude}",
            localizationResourceManager["popupButton"]
            );

        await database.UpdateRouteComponent("Historische Kilometer", nextLocation.Longitude, nextLocation.Latitude, true);
        this.nextLocation = routePoints[this.indexRoute];
        Debug.WriteLine($"Next Location: {nextLocation.Name}, {nextLocation.Description}, {nextLocation.Longitude}, {nextLocation.Latitude}");


        await ReadyNextLine();
        DrawCircleNextLocation();
        SetMapSpan();
    }

    /// <summary>
    /// When nextLocation name is null method is called to ensure next line is readied.
    /// </summary>
    private async Task SkipLocation()
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
        this.nextLocation = routePoints[this.indexRoute];
        Debug.WriteLine($"Next Location: {nextLocation.Name}, {nextLocation.Description}, {nextLocation.Longitude}, {nextLocation.Latitude}");

        await ReadyNextLine();
        DrawCircleNextLocation();
        SetMapSpan();
    }

    /// <summary>
    /// Loads route after route has been selected and shows the route on map.
    /// </summary>
    private async Task LoadRoute()
    {
        // Punten van de geselecteerde route laden
        this.routePoints = await LoadPoints();

        if (routePoints.Count == 0)
        {
            return;
        }

        if (nextLocation == null)
        {
            this.nextLocation = routePoints[0];
        }

        // Punten toevoegen aan de map
        Pins.Clear();
        foreach (var location in routePoints)
        {
            if (location.Name == null)
            {
                continue;
            }
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
        currentPolyline = firstpolyline;

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

    /// <summary>
    /// Readies next line the route goes to.
    /// </summary>
    private async Task ReadyNextLine()
    {
        // DatabaseRoute to the next point of the route
        try {
            if (selectedDatabaseRoute.Name == null)
                return;
        } catch (NullReferenceException exception) {
            return;
        }

        if (nextLocation.Name == null) {
            await SkipLocation();
        }
        var routeToFirstPoint = await mapsAPI.CreateRoute($"{userLat}", $"{userLon}",
            $"{nextLocation.Latitude}", $"{nextLocation.Longitude}");
        Polyline firstpolyline = new Polyline
        {
            StrokeColor = Colors.Chartreuse,
            StrokeWidth = 12,
        };
        currentPolyline = firstpolyline;

        var firstlocations = mapsAPI.Decode(routeToFirstPoint.routes[0].overview_polyline.points);
        foreach (var tempLocation in firstlocations)
        {
            firstpolyline.Geopath.Add(
                new Microsoft.Maui.Devices.Sensors.Location(tempLocation.Latitude, tempLocation.Longitude));
        }

        Polylines.Add(firstpolyline);

        UpdateMapElements();
    }

    /// <summary>
    /// Draws circle around the next location with a radius of 20 metres.
    /// </summary>
    private void DrawCircleNextLocation()
    {
        if (this.routePoints == null || this.routePoints.Count == 0)
        {
            return;
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

    /// <summary>
    /// Sets visible area of the map around the next location.
    /// </summary>
    private void SetMapSpan()
    {
        if (this.nextLocation == null)
        {
            return;
        }

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

    /// <summary>
    /// Sets visible area of the map around the route.
    /// </summary>
    private void SetOverviewMapSpan()
    {
        if (Pins.Count == 0)
        {
            return;
        }

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

    /// <summary>
    /// Pauses or continues route after pause button is pressed.
    /// </summary>
    [RelayCommand]
    private async Task ImageButtonPressed()
    {
        if (RoutePaused)
        {
            await ReadyNextLine();
            DrawCircleNextLocation();
            SetMapSpan();
        }
        else
        {
            selectedDatabaseRoute = new Domain.Models.DatabaseModels.Route();
            SetOverviewMapSpan();
        }
        RoutePaused = !RoutePaused;
        CheckLocationReached();
    }

    /// <summary>
    /// Load points of selected route.
    /// </summary>
    public async Task<List<Location>> LoadPoints()
    {
        // TODO: DatabaseRoute component tabel goed ophalen
        var routeComponents = await database.GetRouteComponentFromRouteAsync("Historische Kilometer");
        Debug.WriteLine($"Size: {routeComponents.Length}");

        if (routeComponents.Length == 0)
        {
            await popUp.ShowPopUpAsync(
                "", 
                "No route components found", 
                "", 
                "",
                localizationResourceManager["popupButton"]);
            return new List<Location>();
        }

        var locations = new List<Location>();
        foreach (var component in routeComponents)
        {
            Domain.Models.DatabaseModels.Location location =
                await database.GetLocationAsync(component.LocationLongitude, component.LocationLatitude);

            if (location == null)
            {
                continue;
            }
            
            locations.Add(new Location
            {
                Name = location.Name,
                Latitude = location.Latitude,
                Description = location.Description,
                Longitude = location.Longitude,
                ImagePath = location.ImagePath,
                RouteOrderNumber = component.RouteOrderNumber
            });
            Debug.WriteLine($"Name: {location.Name}, Latitude: {location.Latitude}, Longtitude: {location.Longitude}, Image: {location.ImagePath}");
        }

       
        UpdatePins();
        //sort locations (implemented with comparable)
        locations.Sort();
        return locations;
    }

    /// <summary>
    /// Gets routes from database.
    /// </summary>
    public async Task LoadRouteFromDatabase()
    {
        var routes = await database.GetRouteTableAsync();
        var routeList = new ObservableCollection<Domain.Models.DatabaseModels.Route>();
        foreach (var route in routes)
        {
            routeList.Add(route);
        }

        Routes = routeList;

        this.RoutePaused = true;
    }

    /// <summary>
    /// Loads selected route and shows it on screen.
    /// </summary>
    public async Task routeSelected(string routeName)
    {
        Polylines.Clear();
        selectedDatabaseRoute = new Domain.Models.DatabaseModels.Route();
        selectedDatabaseRoute.Name = routeName;
        await LoadRoute();
        SetOverviewMapSpan();
    }
}