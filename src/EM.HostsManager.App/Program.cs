//
// Copyright © 2021-2022 Enda Mullally.
//

using EM.HostsManager.App.Process;
using EM.HostsManager.App.UI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace EM.HostsManager.App;

internal static class Program
{
    [STAThread]
    private static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.File("logs/Hosts Manager.log")
            .CreateLogger();

        var serviceProvider = new ServiceCollection()
            .AddLogging(builder => {
                builder.ClearProviders();
                builder.AddSerilog();
            })
            .BuildServiceProvider();

        var logger = serviceProvider.GetService<ILoggerFactory>()!.CreateLogger<MainForm>();
        
        using var si = new SingleInstance("Enda-Mullally|Hosts-Manager|2021-2022|V1|Single.Instance");
        
        var arg = args.Length > 0 ? args[0].ToLowerInvariant() : string.Empty;

        switch (arg)
        {
            case "/quit":
            {
                if (!si.IsSingleInstance())
                {
                    logger.LogInformation("Application quit message - /quit (WmQuitApp)");

                    si.QuitOtherProcess(MainForm.WmQuitApp);
                }

                return;
            }

            case "/min":
            {
                if (!si.IsSingleInstance())
                {
                    logger.LogInformation("Application started with /min witch but another instance is already active");

                    return;
                }

                break;
            }

            case "/elevate":
            {
                break;
            }

            default:
            {
                if (!si.IsSingleInstance())
                {
                    logger.LogInformation("Activating another instance of the application (WmActivateApp)");

                    si.ActivateOtherProcess(MainForm.WmActivateApp);

                    return;
                }

                break;
            }
        }

        logger.LogInformation("Starting new instance");

        Application.SetHighDpiMode(HighDpiMode.SystemAware);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Application.Run(new MainForm(logger));
    }
}