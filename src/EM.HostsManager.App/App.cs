using System.Collections.Generic;
using EM.HostsManager.App.UI;
using EM.HostsManager.App.Uninstall;
using EM.HostsManager.Infrastructure.Process;

namespace EM.HostsManager.App
{
    public partial class App(
        IAppUninstaller uninstaller,
        // ReSharper disable once SuggestBaseTypeForParameterInConstructor
        MainForm mainForm)
    {
        public void Start(IReadOnlyList<string> args)
        {
            using var si = new SingleInstance(Consts.AppInstanceId);

            var arg = args.Count > 0
                ? args[0].ToLowerInvariant()
                : string.Empty;

            switch (arg)
            {
                case Consts.QuitArg:
                {
                    if (!si.IsSingleInstance())
                    {
                        si.QuitOtherProcess(WmQuitApp);
                    }

                    return;
                }

                case Consts.UninstallArg:
                {
                    if (!si.IsSingleInstance())
                    {
                        si.QuitOtherProcess(WmUninstallApp);
                    }
                    else
                    {
                        uninstaller.Uninstall();
                    }

                    return;
                }

                case Consts.MinArg:
                {
                    if (!si.IsSingleInstance())
                    {
                        // If started with the /min switch, but another instance is already
                        // active, we can ignore here.

                        return;
                    }

                    break; // continue to start minimized
                }

                case Consts.ElevateArg:
                {
                    break;
                }

                default:
                {
                    if (!si.IsSingleInstance())
                    {
                        si.ActivateOtherProcess(WmActivateApp);

                        return;
                    }

                    break;
                }
            }
            
            Application.Run(mainForm);
        }
    }
}
