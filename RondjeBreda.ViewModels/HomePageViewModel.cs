using CommunityToolkit.Mvvm.ComponentModel;
using RondjeBreda.Domain.Interfaces;

namespace RondjeBreda.ViewModels;

/// <summary>
/// The Viewmodel for the homepage
/// </summary>
public class HomePageViewModel : ObservableObject
{
    private IDatabase database;
    private IPreferences preferences;
    private IGeolocation geolocation;
    private string route;
    // private Map map; TODO
    private bool routePaused;
}