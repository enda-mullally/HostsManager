//
// Copyright © 2021-2024 Enda Mullally.
//

using EM.HostsManager.Infrastructure.AutoStart;
using EM.HostsManager.Infrastructure.Registry;

namespace EM.HostsManager.App.Uninstall
{
    public class ProgramUninstaller(
        IRegistry registry,
        IAutoStartManager autoStartManager) : IProgramUninstaller
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
