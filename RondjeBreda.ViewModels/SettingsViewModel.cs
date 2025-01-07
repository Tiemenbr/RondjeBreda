using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using LocalizationResourceManager.Maui;

namespace RondjeBreda.ViewModels;

/// <summary>
/// The Viewmodel for the settingspage
/// </summary>
public partial class SettingsViewModel : ObservableObject
{
    private IPreferences preferences;
    private ILocalizationResourceManager localizationResourceManager; // To keep track of current language, and it's translations

    private bool textToSpeech;
    private string colorSetting;
    private string language;

    public SettingsViewModel(IPreferences preferences, ILocalizationResourceManager manager)
    {
        this.preferences = preferences;
        this.localizationResourceManager = manager;
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
    /// Changes the language in the app, and (automatically) updates the pages with the language
    /// </summary>
    /// <param name="language"></param>
    public void LanguageSettingChanged(string language)
    {
        switch (language) // All languages are listed here
        {
            case "English":
                if (localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName != "en") // Only change the language if it isn't already English 
                {
                    localizationResourceManager.CurrentCulture = new CultureInfo("en-US");
                }
                break;

            case "Nederlands":
                if (localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName != "nl") // Only change the language if it isn't already Dutch
                {
                    localizationResourceManager.CurrentCulture = new CultureInfo("nl-NL");
                }
                break;
        }
    }
}