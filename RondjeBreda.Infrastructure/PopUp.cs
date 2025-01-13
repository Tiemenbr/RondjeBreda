using RondjeBreda.Domain.Interfaces;
using Microsoft.Maui.Controls;
using CommunityToolkit.Maui.Views;
using RondjeBreda;

namespace RondjeBreda.Infrastructure;

public class PopUp : IPopUp
{
    public async Task ShowPopUpAsync(string imagepath, string name, string location, string localizedButtonText)
    {
        var popup = new CustomPopUp(imagepath, name, location, localizedButtonText);
        await Application.Current.MainPage.ShowPopupAsync(popup);
    }
}