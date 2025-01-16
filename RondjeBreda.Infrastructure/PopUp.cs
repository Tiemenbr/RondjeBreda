using RondjeBreda.Domain.Interfaces;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using RondjeBreda;

namespace RondjeBreda.Infrastructure;

public class PopUp : IPopUp
{
    /// <summary>
    /// Shows Popup on Screen.
    /// </summary>
    public async Task ShowPopUpAsync(string imagepath, string name, string description, string location, string localizedButtonText)
    {
        var popup = new CustomPopUp(imagepath, name, description,location, localizedButtonText);
        await Application.Current.MainPage.ShowPopupAsync(popup);
    }
}