using Microsoft.Extensions.Logging;
using RondjeBreda.Pages;
using RondjeBreda.ViewModels;

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
                });

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<HomePageViewModel>();
            
            builder.Services.AddTransient<SettingsPage>();
            builder.Services.AddTransient<SettingsViewModel>();
            builder.Services.AddSingleton<IPreferences>(o => Preferences.Default);

            builder.Services.AddTransient<VisitedLocationsPage>();
            builder.Services.AddTransient<VisitedLocationsViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
