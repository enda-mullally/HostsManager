//
// Copyright © 2021-2024 Enda Mullally.
//

using EM.HostsManager.App.UI;
using EM.HostsManager.App.Uninstall;
using EM.HostsManager.Process;

namespace EM.HostsManager.App
{
    public partial class App(
        IAppUninstaller uninstaller,
        IAppSingleInstance singleInstance,
        // ReSharper disable once SuggestBaseTypeForParameterInConstructor
        MainForm mainForm)
    {
        public void Start(IReadOnlyList<string> args)
        {
            var arg = args.Count > 0
                ? args[0].ToLowerInvariant()
                : string.Empty;

            switch (arg)
            {
                case Consts.QuitArg:
                {
                    if (!singleInstance.IsSingleInstance())
                    {
                        singleInstance.QuitOtherProcess(WmQuitApp);
                    }

                    return;
                }

                case Consts.UninstallArg:
                {
                    if (!singleInstance.IsSingleInstance())
                    {
                        singleInstance.QuitOtherProcess(WmUninstallApp);
                    }
                    else
                    {
                        uninstaller.Uninstall();
                    }

                    return;
                }

                case Consts.MinArg:
                {
                    if (!singleInstance.IsSingleInstance())
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
                    if (!singleInstance.IsSingleInstance())
                    {
                        singleInstance.ActivateOtherProcess(WmActivateApp);

                        return;
                    }

                    break;
                }
            }
            
            Application.Run(mainForm);
        }
    }
}