using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls;

namespace RondjeBreda.Infrastructure;

public class CustomPopUp : Popup
{
    public CustomPopUp(string imagePath, string name, string description, string location, string localizedButtonText)
    {
        Content = new ScrollView
        {
            Content = new VerticalStackLayout
            {
                BackgroundColor = Colors.White,
                Padding = new Thickness(20),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 10,
                Children =
                {
                    new Image
                    {
                        Source = imagePath,
                        WidthRequest = 200,
                        HeightRequest = 200,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = name,
                        TextColor = Colors.Black,
                        FontSize = 16,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = description,
                        TextColor = Colors.Black,
                        FontSize = 16,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Label
                    {
                        Text = location,
                        TextColor = Colors.Black,
                        FontSize = 16,
                        HorizontalOptions = LayoutOptions.Center
                    },
                    new Button
                    {
                        Text = localizedButtonText,
                        Command = new Command(() => Close()),
                        HorizontalOptions = LayoutOptions.Center
                    }
                }
            }
        };
    }
}