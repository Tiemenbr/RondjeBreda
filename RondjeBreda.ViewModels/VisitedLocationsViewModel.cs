using CommunityToolkit.Mvvm.ComponentModel;
using RondjeBreda.Domain.Interfaces;

namespace RondjeBreda.ViewModels;

/// <summary>
/// The Viewmodel for the visitedlocationpage
/// </summary>
public class VisitedLocationsViewModel : ObservableObject
{
    private IDatabase database;
    private IPreferences preferences;
    // private ObservableList<Location> visitedLocations;
}