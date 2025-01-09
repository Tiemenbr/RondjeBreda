using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace RondjeBreda.Infrastructure.SettingsImplementation
{
    public static class ServiceProviderHelper
    {
        public static IServiceProvider ServiceProvider { get; set; }

        public static T GetService<T>() => ServiceProvider.GetService<T>();
    }
}
 