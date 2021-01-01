using System;
using System.Threading;
using System.Windows.Forms;
using HostsManager.Attributes;
using HostsManager.Forms;

// BuildDate
[assembly: BuildDate("BUILD-DATE-ATTRIBUTE")]

namespace HostsManager
{
    internal static class Program
    {
        private static readonly Mutex AppMutex = new Mutex(false, "Enda-Mullally|Hosts-Manager|2021|V1|Single.Instance");

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
}
