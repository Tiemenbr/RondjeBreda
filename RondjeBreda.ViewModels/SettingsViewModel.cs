using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using LocalizationResourceManager.Maui;
using RondjeBreda.Infrastructure.SettingsImplementation;

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
    private bool startup = true;

    [ObservableProperty]
    private ObservableCollection<string> colorModes;

    [ObservableProperty]
    private string colorMode1;

    [ObservableProperty]
    private string colorMode2;

    public SettingsViewModel(IPreferences preferences, ILocalizationResourceManager manager)
    {
        this.preferences = preferences;
        this.localizationResourceManager = manager;
        this.colorModes = new ObservableCollection<string>();

        // Get the current language, and update pickers based on it
        string culture = localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName;
        if (culture == "en")
        {
            LanguageSettingChanged("English");
        } 
        else if (culture == "nl")
        {
            LanguageSettingChanged("Nederlands");
        }
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
        switch (colorSetting)
        {
            case "Standard Colors" or "Standaard Kleuren": // Each language
                if (Application.Current != null)
                {
                    Application.Current.UserAppTheme = AppTheme.Light;
                    preferences.Set("Color Mode", "Standard");
                } 
                return;

            case "Black-White" or "Zwart-Wit": // Each language
                if (Application.Current != null)
                {
                    Application.Current.UserAppTheme = AppTheme.Dark;
                    preferences.Set("Color Mode", "Black-White");
                }
                return;
        }
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
                preferences.Set("Language", language);
                SetLanguageEnglish();
                return;

            case "Nederlands":
                preferences.Set("Language", language);
                SetLanguageDutch();
                return;
        }
    }

    private void SetLanguageEnglish()
    {
        if (localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName != "en" || startup) // Only change the language if it isn't already English 
        {
            localizationResourceManager.CurrentCulture = new CultureInfo("en-US");
            // Change color mode language
            ColorMode1 = string.Format(localizationResourceManager["ColorMode1"], "Standard Colors");
            ColorMode2 = string.Format(localizationResourceManager["ColorMode2"], "Black-White");
            RefreshColorModesList();
            startup = false;
        }
    }
    private void SetLanguageDutch()
    {
        if (localizationResourceManager.CurrentCulture.TwoLetterISOLanguageName != "nl" || startup) // Only change the language if it isn't already Dutch
        {
            localizationResourceManager.CurrentCulture = new CultureInfo("nl-NL");
            // Change color mode language
            ColorMode1 = string.Format(localizationResourceManager["ColorMode1"], "Standaard Kleuren");
            ColorMode2 = string.Format(localizationResourceManager["ColorMode2"], "Zwart-Wit");
            RefreshColorModesList();
            startup = false;
        }
    }

    private void RefreshColorModesList()
    {
        if (ColorModes.Count != 0)
        {
            // Remove first 2 options
            ColorModes.RemoveAt(0);
            ColorModes.RemoveAt(0);
        }
        ColorModes.Add(ColorMode1);
        ColorModes.Add(ColorMode2);
    }
}