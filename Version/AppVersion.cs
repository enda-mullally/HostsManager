using System;
using System.Linq;
using System.Reflection;
using HostsManager.Attributes;

namespace HostsManager.Version
{
    public class AppVersion
    {
        private readonly Assembly _assembly;
        
        public AppVersion(Assembly assembly) =>
            _assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));

        public string GetAppVersion()
        {
            var version = _assembly
                .GetName()
                .Version;

            return version!.Major + "." +
                   version.Minor + "." +
                   version.Build;
        }

        public string? GetBuildDate()
        {
            var buildDateAttr = _assembly
                .GetCustomAttributes(typeof(BuildDateAttribute), false)
                .Cast<BuildDateAttribute>()
                .FirstOrDefault();

            return buildDateAttr?.BuildDate;
        }
    }
}
