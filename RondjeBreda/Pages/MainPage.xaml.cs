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

            // Route genereren tussen de punten

            // Lijn toevoegen aan de map
        }
    }
}
