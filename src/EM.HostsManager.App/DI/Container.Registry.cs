using EM.HostsManager.App.UI;
using EM.HostsManager.App.Uninstall;
using EM.HostsManager.Infrastructure.AutoStart;
using EM.HostsManager.Infrastructure.Hosts;
using EM.HostsManager.Infrastructure.IO;
using EM.HostsManager.Infrastructure.Registry;
using EM.HostsManager.Infrastructure.Version;
using Microsoft.Extensions.DependencyInjection;
using File = EM.HostsManager.Infrastructure.IO.File;

namespace EM.HostsManager.App.DI
{
    public partial class Container
    {
        private Container RegisterServices()
        {
            _container.AddScoped<App>();
            _container.AddScoped<MainForm>();
            _container.AddScoped<IFile, File>();
            _container.AddScoped<IHostsFile, HostsFile>();
            _container.AddScoped<IRegistry, Registry>();
            _container.AddScoped<IAppUninstaller, AppUninstaller>();

            var exe = Application
                .ExecutablePath
                .Replace(".dll", ".exe", StringComparison.InvariantCultureIgnoreCase);

            _container.AddScoped<IAutoStartManager, AutoStartManager>(provider =>
                new AutoStartManager(provider.GetRequiredService<IRegistry>(), exe, Consts.MinArg,
                    Consts.ApplicationName));

            _container.AddSingleton<IAppVersion, AppVersion>(provider =>
                new AppVersion(Assembly.GetExecutingAssembly()));

            return this;
        }
    }
}
