namespace RondjeBreda.Domain.Interfaces;

public interface IPopUp
{
    Task ShowPopUpAsync(string title, string message);
}