//
// Copyright © 2021-2024 Enda Mullally.
//

using EM.HostsManager.Infrastructure.AutoStart;
using EM.HostsManager.Infrastructure.Settings;

namespace EM.HostsManager.App.Uninstall
{
    public class AppUninstaller(
        ISettingsProvider settings,
        IAutoStartManager autoStartManager) : IAppUninstaller
    {
        public const string FirstRunShownForKey = @"FirstRunShownForVersion";

        public void Uninstall()
        {
            try
            {
                autoStartManager.DeleteAutoRunAtStartup();

                settings.DeleteValue(FirstRunShownForKey);
            }
            catch
            {
                // ignore
            }
        }
    }
}
