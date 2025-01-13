namespace RondjeBreda.Domain.Interfaces;

public interface IPopUp
{
    Task ShowPopUpAsync(string imagepath, string name, string description, string location, string localizedButtonText);
}