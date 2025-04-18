//
// Copyright © 2021-2025 Enda Mullally.
//

using EM.HostsManager.AutoStart;
using EM.HostsManager.Settings;

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