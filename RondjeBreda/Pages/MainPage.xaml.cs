using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using RondjeBreda.Domain.Models;
using RondjeBreda.Domain.Models.DatabaseModels;
using RondjeBreda.ViewModels;
using Distance = Microsoft.Maui.Maps.Distance;
using Location = Microsoft.Maui.Devices.Sensors.Location;
using Polyline = Microsoft.Maui.Controls.Maps.Polyline;
using Route = RondjeBreda.Domain.Models.DatabaseModels.Route;

namespace RondjeBreda.Pages
{
    /// <summary>
    /// The class for the mainpage with the viewmodel for it.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private HomePageViewModel homePageViewModel;
        private List<Domain.Models.DatabaseModels.Location> routePoints;
        private Domain.Models.DatabaseModels.Location nextLocation;

        public MainPage(HomePageViewModel homePageViewModel)
        {
            InitializeComponent();
            this.homePageViewModel = homePageViewModel;
            //BindingContext = homePageViewModel;
        }

        private async void LoadRoute(Route selectedRoute)
        {
            // Punten van de geselecteerde route laden
            this.routePoints = await homePageViewModel.LoadPoints();

            // Punten toevoegen aan de map
            Map.Pins.Clear();
            foreach (var location in routePoints)
            {
                Map.Pins.Add(new Pin
                {
                    Label = location.name,
                    Location = new Location(location.latitude, location.longitude)
                });
            }

            Debug.WriteLine($"Punten: {routePoints.Count}");

            // Route to the start point of the route
            var routeToFirstPoint = await homePageViewModel.mapsAPI.CreateRoute($"{homePageViewModel.userLat}", $"{homePageViewModel.userLon}",
                $"{routePoints[0].latitude}", $"{routePoints[0].longitude}");
            Polyline firstpolyline = new Polyline
            {
                StrokeColor = Colors.Chartreuse,
                StrokeWidth = 12,
            };
            var firstlocations = homePageViewModel.mapsAPI.Decode(routeToFirstPoint.routes[0].overview_polyline.points);
            foreach (var tempLocation in firstlocations)
            {
                firstpolyline.Geopath.Add(new Location(tempLocation.latitude, tempLocation.longitude));
            }
            Map.MapElements.Add(firstpolyline);

            // Route genereren tussen de punten
            foreach (var location in routePoints)
            {
                if (routePoints.IndexOf(location) == routePoints.Count - 1)
                {
                    continue;
                }

                var locationCount = routePoints.IndexOf(location);
                Debug.WriteLine($"Lat: {location.latitude}, Long: {location.longitude}");
                var route = await homePageViewModel.mapsAPI.CreateRoute($"{location.latitude}", $"{location.longitude}",
                    routePoints[locationCount + 1].latitude.ToString(),
                    routePoints[locationCount + 1].longitude.ToString());


                Debug.WriteLine($"Aantal routes: {route.routes.Length}");

                Polyline polyline = new Polyline
                {
                    StrokeColor = Colors.Blue,
                    StrokeWidth = 12,
                };

                var locations = homePageViewModel.mapsAPI.Decode(route.routes[0].overview_polyline.points);
                foreach (var tempLocation in locations)
                {
                    polyline.Geopath.Add(new Location(tempLocation.latitude, tempLocation.longitude));
                }

                // Lijn toevoegen aan de map
                Map.MapElements.Add(polyline);
            }
        }

        private void ImageButton_OnPressed(object? sender, EventArgs e)
        {
            Debug.WriteLine("PauseButton!!!");

            Map.MapElements.Clear();
            LoadRoute(new Route());
            DrawCircleNextLocation();
            SetMapSpan();
        }

        private void DrawCircleNextLocation()
        {
            if (this.routePoints == null || this.routePoints.Count == 0)
            {
                return;
            }

            this.nextLocation = routePoints[0];

            foreach (var location in routePoints)
            {
                // TODO: Check if location is visited, first one that don't need to be set
            }

            Circle circle = new Circle
            {
                Center = new Location(nextLocation.latitude, nextLocation.longitude),
                Radius = new Distance(20),
                StrokeColor = Color.FromArgb("#CFffc61e"),
                StrokeWidth = 8,
                FillColor = Color.FromArgb("#CFffc61e")
            };

            Map.MapElements.Add(circle);
        }

        private void SetMapSpan()
        {
            var centerLat = (this.nextLocation.latitude + homePageViewModel.userLat) / 2;
            var centerLon = (this.nextLocation.longitude + homePageViewModel.userLon) / 2;

            var center = new Location(centerLat, centerLon);

            var distance = Location.CalculateDistance(new Location(this.nextLocation.latitude, this.nextLocation.longitude), center, DistanceUnits.Kilometers);

            MapSpan mapSpan = MapSpan.FromCenterAndRadius(center, Distance.FromKilometers(distance * 1.5));

            Map.MoveToRegion(mapSpan);
        }
    }
}