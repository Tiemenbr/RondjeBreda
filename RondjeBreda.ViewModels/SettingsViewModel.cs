using CommunityToolkit.Mvvm.ComponentModel;

namespace RondjeBreda.ViewModels;

/// <summary>
/// The Viewmodel for the settingspage
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    private IPreferences preferences;
    private bool textToSpeech;
    private string colorSetting;
    private string language;

    public SettingsViewModel(IPreferences preferences)
    {
        this.preferences = preferences;
    }

    /// <summary>
    /// Saves the value for the text to speech option as a boolean in the preferences.
    /// </summary>
    /// <param name="isChecked"></param>
    public void TextToSpeechChecked(bool isChecked)
    {
        preferences.Set("TextToSpeech", isChecked);
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="colorSetting"></param>
    public void ColorSettingChanged(string colorSetting)
    {
        // TODO
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="language"></param>
    public void LanguageSettingChanged(string language)
    {
        // TODO
    }
}