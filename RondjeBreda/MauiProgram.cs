﻿using CommunityToolkit.Maui;
using LocalizationResourceManager.Maui;
using Microsoft.Extensions.Logging;
using RondjeBreda.Domain.Interfaces;
using RondjeBreda.Infrastructure.DatabaseImplementation;
using RondjeBreda.Pages;
using RondjeBreda.Resources.Languages;
using RondjeBreda.ViewModels;
using System.Globalization;

namespace RondjeBreda
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiMaps()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseLocalizationResourceManager(settings => {
                    settings.RestoreLatestCulture(true); // Saves the state of the app when closed, so no Preferences are needed
                    settings.AddResource(AppResource.ResourceManager); // Adds a ResourceManager to keep track of language
                    settings.InitialCulture(new CultureInfo("en-US")); // Set the initial languag always to English
                })
                .UseMauiCommunityToolkit();

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<HomePageViewModel>();

            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsViewModel>();

            builder.Services.AddTransient<VisitedLocationsPage>();
            builder.Services.AddTransient<VisitedLocationsViewModel>();

            builder.Services.AddSingleton<IPreferences>(o => Preferences.Default);
            builder.Services.AddSingleton<IDatabase, SQLiteDatabase>(o => {
                return new SQLiteDatabase();
            });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
