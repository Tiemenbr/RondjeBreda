using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace RondjeBreda.Infrastructure.SettingsImplementation
{
    /// <summary>
    /// Provides culture to TextToSpeechSetting.
    /// </summary>
    public static class ServiceProviderHelper
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static T GetService<T>() => ServiceProvider.GetService<T>();
    }
}
 