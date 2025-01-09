using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RondjeBreda.ViewModels;

namespace RondjeBreda.Pages;

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
        //BindingContext = settingsViewModel
    }

    /// <summary>
    /// Event handler for the text to speech checkbox
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void CheckBox_OnCheckedChanged(object? sender, CheckedChangedEventArgs e)
    {
        if (settingsViewModel is SettingsViewModel)
            settingsViewModel.TextToSpeechChecked(e.Value);
    }

    /// <summary>
    /// Event handler for the color setting picker
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PickerColorSetting_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        if (settingsViewModel is SettingsViewModel)
        {
            var picker = (Picker)sender;
            settingsViewModel.ColorSettingChanged(picker.SelectedItem.ToString());
        }
    }

    /// <summary>
    /// Event handler for the language setting picker
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void PickerLanguage_OnSelectedIndexChanged(object? sender, EventArgs e)
    {
        if (settingsViewModel is SettingsViewModel)
        {
            var picker = (Picker)sender;
            settingsViewModel.LanguageSettingChanged(picker.SelectedItem.ToString());
        }
    }
}