using System;
using System.Linq;
using System.Reflection;
using EM.HostsManager.App.Attributes;

namespace EM.HostsManager.App.Version
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

        public string? GetCommitId()
        {
            var commitIdAttr = _assembly
                .GetCustomAttributes(typeof(CommitIdAttribute), false)
                .Cast<CommitIdAttribute>()
                .FirstOrDefault();

            return commitIdAttr?.CommitId;
        }
    }
}
