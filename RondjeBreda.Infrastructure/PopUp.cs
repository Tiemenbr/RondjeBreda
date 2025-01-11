using RondjeBreda.Domain.Interfaces;
using Microsoft.Maui.Controls;

namespace RondjeBreda.Infrastructure;

public class PopUp : IPopUp
{
    public async Task ShowPopUpAsync(string title, string message)
    {
        await Application.Current.MainPage.DisplayAlert(title, message, "OK");
    }
}