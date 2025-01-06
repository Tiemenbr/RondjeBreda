using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Domain.Models.DatabaseModels;

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
}