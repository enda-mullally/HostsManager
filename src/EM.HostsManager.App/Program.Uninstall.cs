using EM.HostsManager.Infrastructure.AutoStart;
using Reg = EM.HostsManager.Infrastructure.Registry.Registry;

namespace EM.HostsManager.App
{
    internal static partial class Program
    {
        public static void Uninstall()
        {
            try
            {
                var exe = Application
                    .ExecutablePath
                    .Replace(".dll", ".exe", StringComparison.InvariantCultureIgnoreCase);

                new AutoStartManager(exe, Consts.MinArg, Consts.ApplicationName)
                    .DeleteAutoRunAtStartup();

                Reg.DeleteRegString(
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
