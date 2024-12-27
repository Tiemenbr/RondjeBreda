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

    /// <summary>
    /// Saves the value for the text to speech option as a boolean in the preferences.
    /// </summary>
    /// <param name="isChecked"></param>
    public void TextToSpeechChecked(bool isChecked)
    {
        preferences.Set("TextToSpeech", isChecked);
    }
}