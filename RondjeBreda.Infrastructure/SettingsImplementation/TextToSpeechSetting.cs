using Microsoft.Maui.Media;
using Microsoft.Maui.Storage;
using LocalizationResourceManager.Maui;

namespace RondjeBreda.Infrastructure.SettingsImplementation
{
    public class TextToSpeechSetting
    {
        private static ILocalizationResourceManager LocalizationResourceManager =>
            ServiceProviderHelper.GetService<ILocalizationResourceManager>();
        public async void Speak(string text)
        {
            bool ttsEnabled = ServiceProviderHelper.GetService<IPreferences>().Get("TextToSpeech", false);
            if (ttsEnabled)
            {
                var culture = LocalizationResourceManager.CurrentCulture;
                var locales = await TextToSpeech.GetLocalesAsync();
                var locale = locales.FirstOrDefault(l => l.Language == culture.TwoLetterISOLanguageName);

                var settings = new SpeechOptions
                {
                    Volume = 1.0f,
                    Pitch = 1.0f,
                    Locale = locale
                };

                await TextToSpeech.SpeakAsync(text, settings);
            }
        }
    }
}
