using CommunityToolkit.Mvvm.ComponentModel;

namespace RondjeBreda.ViewModels;

/// <summary>
/// The Viewmodel for the settingspage
/// </summary>
public class SettingsViewModel : ObservableObject
{
    private IPreferences preferences;
    private bool textToSpeech;
    private string colorSetting;
    private string language;
}