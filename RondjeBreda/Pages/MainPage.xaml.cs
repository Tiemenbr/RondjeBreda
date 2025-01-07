using System.Diagnostics;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Maui.Controls.Maps;
using RondjeBreda.Domain.Models.DatabaseModels;
using RondjeBreda.ViewModels;
using Location = Microsoft.Maui.Devices.Sensors.Location;

namespace RondjeBreda.Pages
{
    /// <summary>
    /// The class for the mainpage with the viewmodel for it.
    /// </summary>
    public partial class MainPage : ContentPage
    {
        private HomePageViewModel homePageViewModel;

        public MainPage(HomePageViewModel homePageViewModel)
        {
            InitializeComponent();
            this.homePageViewModel = homePageViewModel;
            //BindingContext = homePageViewModel;
        }

        private async void LoadRoute(Route selectedRoute)
        {
            // Punten van de geselecteerde route laden
            var routePoints = await homePageViewModel.LoadPoints();

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
                    routePoints[locationCount+1].latitude.ToString(), routePoints[locationCount + 1].longitude.ToString());

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
            LoadRoute(new Route());
        }
    }
}