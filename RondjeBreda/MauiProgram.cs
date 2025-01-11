using Microsoft.Extensions.Logging;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Pages;
using RondjeBreda.ViewModels;
using RondjeBreda.Infrastructure.DatabaseImplementation;
using RondjeBreda.Infrastructure.SettingsImplementation;
using CommunityToolkit.Maui;
using LocalizationResourceManager.Maui;
using RondjeBreda.Resources.Languages;
using System.Globalization;
using RondjeBreda.Infrastructure;

namespace RondjeBreda
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseLocalizationResourceManager(settings =>
                {
                    settings.RestoreLatestCulture(true); // Saves the state of the app when closed, so no Preferences are needed
                    settings.AddResource(AppResource.ResourceManager); // Adds a ResourceManager to keep track of language
                    settings.InitialCulture(new CultureInfo("en-US")); // Set the initial languag always to English
                })
                .UseMauiCommunityToolkit();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<HomePageViewModel>();
            
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddSingleton<IPreferences>(o => Preferences.Default);

            builder.Services.AddTransient<VisitedLocationsPage>();
            builder.Services.AddTransient<VisitedLocationsViewModel>();

            builder.Services.AddSingleton<IPreferences>(o => Preferences.Default);
            builder.Services.AddSingleton<IDatabase, SQLiteDatabase>(o =>
            {
                return new SQLiteDatabase();
            });

            builder.Services.AddTransient<IMapsAPI, MapsAPI>();
            builder.Services.AddTransient<IKeyReaderMaps, KeyReaderMaps>();

            builder.Services.AddSingleton<IGeolocation>(o => Geolocation.Default);

            builder.Services.AddTransient<IPopUp, PopUp>();

#if DEBUG
    		builder.Logging.AddDebug(); 
#endif

            var app = builder.Build();

            ServiceProviderHelper.ServiceProvider = app.Services;

            return app;
        }
    }
}
