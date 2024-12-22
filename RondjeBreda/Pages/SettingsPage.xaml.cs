using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RondjeBreda.ViewModels;

namespace RondjeBreda;

/// <summary>
/// The class for the settingspage with the viewmodel for it.
/// </summary>
public partial class SettingsPage : ContentPage
{
    private SettingsViewModel settingsViewModel;
    public SettingsPage(SettingsViewModel settingsViewModel)
    {
        InitializeComponent();
        this.settingsViewModel = settingsViewModel;
    }
}