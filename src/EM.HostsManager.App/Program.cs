using EM.HostsManager.App.Process;
using EM.HostsManager.App.UI;

namespace EM.HostsManager.App;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        using var si = new SingleInstance("Enda-Mullally|Hosts-Manager|2021-2022|V1|Single.Instance");
        if (!si.IsSingleInstance())
        {
            var requestingQuit =
                args.Length > 0 && args[0].ToLowerInvariant().Equals("/quit");  // uninstall script quit request

            if (requestingQuit)
            {
                si.QuitOtherProcess();

                return;
            }

            si.ActivateOtherProcess();

            return;
        }

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm());
    }
}