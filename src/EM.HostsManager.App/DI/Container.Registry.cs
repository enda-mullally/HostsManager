using EM.HostsManager.App.UI;
using EM.HostsManager.App.Uninstall;
using EM.HostsManager.Infrastructure.AutoStart;
using EM.HostsManager.Infrastructure.Hosts;
using EM.HostsManager.Infrastructure.IO;
using EM.HostsManager.Infrastructure.PreferredEditor;
using EM.HostsManager.Infrastructure.PreferredEditor.Editors;
using EM.HostsManager.Infrastructure.Process;
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
            _container.AddTransient<App>();
            _container.AddTransient<MainForm>();
            _container.AddTransient<IFile, File>();
            _container.AddTransient<IHostsFile, HostsFile>();
            _container.AddTransient<IRegistry, Registry>();
            _container.AddTransient<IAppUninstaller, AppUninstaller>();

            var exe = Application
                .ExecutablePath
                .Replace(".dll", ".exe", StringComparison.InvariantCultureIgnoreCase);

            _container.AddTransient<IAutoStartManager, AutoStartManager>(provider =>
                new AutoStartManager(provider.GetRequiredService<IRegistry>(), exe, Consts.MinArg,
                    Consts.ApplicationName));

            _container.AddSingleton<IAppVersion, AppVersion>(_ => new AppVersion(Assembly.GetExecutingAssembly()));

            _container.AddSingleton<ISingleInstance, SingleInstance>(_ => new SingleInstance(Consts.AppInstanceId));

            // Preferred Editor Manager
            _container.AddTransient<IPreferredEditorManager, PreferredEditorManager>(CreatePreferredEditorManager);

            return this;
        }

        private static PreferredEditorManager CreatePreferredEditorManager(IServiceProvider provider)
        {
            var fileName = HostsFile.GetHostsFilename();

            return
                new PreferredEditorManager(provider.GetRequiredService<IRegistry>(),
                    Consts.AppRegPath,
                    Consts.PreferredEditorKey).RegisterEditors(
                    [
                        new Default(fileName, true),
                        new NotepadPP(fileName),
                        new VSCode(fileName)
                    ]
                );
        }
    }
}