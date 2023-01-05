//
// Copyright © 2021-2023 Enda Mullally.
//

using EM.HostsManager.App.UI;
using EM.HostsManager.Infrastructure.Process;

namespace EM.HostsManager.App;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        using var si = new SingleInstance("Enda-Mullally|Hosts-Manager|2021-2023|V1|Single.Instance");
        
        var arg = args.Length > 0 ? args[0].ToLowerInvariant() : string.Empty;

        switch (arg)
        {
            case "/quit":
            {
                if (!si.IsSingleInstance())
                {
                    si.QuitOtherProcess(MainForm.WmQuitApp);
                }

                return;
            }

            case "/min":
            {
                if (!si.IsSingleInstance())
                {
                    // If started with the /min switch, but another instance is already active, we can ignore here.

                    return;
                }

                break;  // continue to start minimized
            }

            case "/elevate":
            {
                break;
            }

            default:
            {
                if (!si.IsSingleInstance())
                {
                    si.ActivateOtherProcess(MainForm.WmActivateApp);

                    return;
                }

                break;
            }
        }

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}