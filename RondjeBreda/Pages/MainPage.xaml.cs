using System.ComponentModel;
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
        
        public MainPage(HomePageViewModel homePageViewModel)
        {
            InitializeComponent();
            this.homePageViewModel = homePageViewModel;
            homePageViewModel.PropertyChanged += MoveToRegion();

            BindingContext = homePageViewModel;
        }

        private PropertyChangedEventHandler? MoveToRegion()
        {
            if (Map == null || homePageViewModel == null || homePageViewModel.mapSpan == null)
            {
                return null;
            }
            Map.MoveToRegion(homePageViewModel.mapSpan);
            return null;
        }

        private void ImageButton_OnPressed(object? sender, EventArgs e)
        {

            Debug.WriteLine("PauseButton!!!");

            
        }
    }
}