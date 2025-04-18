//
// Copyright © 2021-2025 Enda Mullally.
//

using EM.HostsManager.App.DI;
using Microsoft.Extensions.DependencyInjection;

namespace EM.HostsManager.App;

internal static class Program
{
    private static readonly IServiceProvider ServiceProvider =
        Container
            .Create()
            .BuildServiceProvider();

    [STAThread]
    private static void Main(string[] args)
    {
        Application.SetHighDpiMode(HighDpiMode.DpiUnawareGdiScaled);
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        
        var app =
            ServiceProvider.GetService<App>() ??
                throw new NullReferenceException("Could not create " + nameof(App));
        
        app.Start(args);
    }
}