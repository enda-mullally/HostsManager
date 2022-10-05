using EM.HostsManager.App.Process;
using EM.HostsManager.App.UI;

namespace EM.HostsManager.App;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        using var si = new SingleInstance("Enda-Mullally|Hosts-Manager|2021-2022|V1|Single.Instance");
        
        var arg = args.Length > 0 ? args[0].ToLowerInvariant() : string.Empty;

        switch (arg)
        {
            case "/quit":
            {
                if (!si.IsSingleInstance())
                {
                    si.QuitOtherProcess();
                }

                return;
            }

            case "/elevate":
            {
                break;
            }

            default:
            {
                if (!si.IsSingleInstance())
                {
                    si.ActivateOtherProcess();

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