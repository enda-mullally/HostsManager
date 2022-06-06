using EM.HostsManager.App.UI;

namespace EM.HostsManager.App;

internal static class Program
{
    private static readonly Mutex AppMutex = new(false, "Enda-Mullally|Hosts-Manager|2021-2022|V1|Single.Instance");

    [STAThread]
    private static void Main()
    {
        if (!AppMutex.WaitOne(TimeSpan.FromSeconds(2), false))
        {
            return;
        }

        try
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
        finally
        {
            AppMutex.ReleaseMutex();
        }
    }
}