//
// Copyright © 2021-2024 Enda Mullally.
//

using EM.HostsManager.App.DI;
using EM.HostsManager.App.UI;
using EM.HostsManager.App.Uninstall;
using EM.HostsManager.Infrastructure.Process;
using Microsoft.Extensions.DependencyInjection;

namespace EM.HostsManager.App;

internal static partial class Program
{
    private static readonly IServiceProvider ServiceProvider =
        Container
            .Create()
            .BuildServiceProvider();

    [STAThread]
    private static void Main(string[] args)
    {
        using var si = new SingleInstance(Consts.AppInstanceId);

        var arg = args.Length > 0
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
                    var uninstaller =
                        ServiceProvider.GetService<ProgramUninstaller>() ??
                        throw new NullReferenceException("Could not create " + nameof(ProgramUninstaller));

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

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        var mainForm =
            ServiceProvider.GetService<MainForm>() ??
                throw new NullReferenceException("Could not create " + nameof(Main));

        Application.Run(mainForm);
    }
}