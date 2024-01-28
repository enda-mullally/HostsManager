//
// Copyright © 2021-2024 Enda Mullally.
//

using EM.HostsManager.Infrastructure.AutoStart;
using EM.HostsManager.Infrastructure.Registry;

namespace EM.HostsManager.App.Uninstall
{
    public class AppUninstaller(
        IRegistry registry,
        IAutoStartManager autoStartManager) : IAppUninstaller
    {
        public void Uninstall()
        {
            try
            {
                autoStartManager.DeleteAutoRunAtStartup();

                registry.DeleteRegString(
                    Microsoft.Win32.Registry.CurrentUser,
                    Consts.AppRegPath,
                    Consts.FirstRunShownForKey);
            }
            catch
            {
                // ignore
            }
        }
    }
}
