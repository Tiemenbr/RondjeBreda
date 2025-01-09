﻿using System.ComponentModel;
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

            BindingContext = homePageViewModel;

            this.homePageViewModel.UpdatePins += UpdatePins;
            this.homePageViewModel.UpdateMapSpan += UpdateMapSpan;
            this.homePageViewModel.UpdateMapElements += UpdateMapElements;    
        }

        private void UpdateMapSpan()
        {
            Debug.WriteLine("Update MapSpan");
            if (homePageViewModel.CurrentMapSpan == null)
            {
                return;
            }
            Map.MoveToRegion(homePageViewModel.CurrentMapSpan);
        }

        private void UpdatePins()
        {
            Debug.WriteLine("Update Pins");
            Map.Pins.Clear();
            foreach (var pin in homePageViewModel.Pins)
            {
                Map.Pins.Add(pin);
            }
        }

        private void UpdateMapElements()
        {
            Debug.WriteLine("Update MapElements");
            Map.MapElements.Clear();
            Map.MapElements.Add(homePageViewModel.RangeCircle);

            Debug.WriteLine($"Polylines amount: {homePageViewModel.Polylines.Count}");
            foreach (var polyline in homePageViewModel.Polylines)
            {
                Debug.WriteLine($"Polyline adding: {polyline}");
                Map.MapElements.Add(polyline);
            }
            
        }
    }
}