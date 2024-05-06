//
// Copyright © 2024 Enda Mullally.
//

using EM.HostsManager.App.UI;
using EM.HostsManager.App.Uninstall;
using EM.HostsManager.AutoStart;
using EM.HostsManager.DNS;
using EM.HostsManager.Hosts;
using EM.HostsManager.IO;
using EM.HostsManager.PreferredEditor;
using EM.HostsManager.PreferredEditor.Editors;
using EM.HostsManager.Process;
using EM.HostsManager.Registry;
using EM.HostsManager.Settings;
using EM.HostsManager.Settings.Providers;
using EM.HostsManager.Version;
using Microsoft.Extensions.DependencyInjection;
using File = EM.HostsManager.IO.File;

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
            _container.AddTransient<IRegistry, Registry.Registry>();
            _container.AddTransient<IAppUninstaller, AppUninstaller>();
            _container.AddTransient<IDNSHelper, DNSHelper>();

            _container.AddTransient<ISettingsProvider, RegistrySettingsProvider>(_ =>
                new RegistrySettingsProvider(Microsoft.Win32.Registry.CurrentUser, Consts.AppRegPath));
            
            var exe = Application
                .ExecutablePath
                .Replace(".dll", ".exe", StringComparison.InvariantCultureIgnoreCase);

            _container.AddTransient<IAutoStartManager, AutoStartManager>(provider =>
                new AutoStartManager(provider.GetRequiredService<IRegistry>(), exe, Consts.MinArg,
                    Consts.ApplicationName));

            _container.AddSingleton<IAppVersion, AppVersion>(_ => new AppVersion(Assembly.GetExecutingAssembly()));

            _container.AddSingleton<IAppSingleInstance, AppSingleInstance>(_ => new AppSingleInstance(Consts.AppInstanceId));

            // Preferred Editor Manager
            _container.AddTransient<IPreferredEditorManager, PreferredEditorManager>(CreatePreferredEditorManager);

            return this;
        }

        private static PreferredEditorManager CreatePreferredEditorManager(IServiceProvider provider)
        {
            var fileName = provider
                .GetRequiredService<IHostsFile>()
                .GetHostsFilename();

            return
                new PreferredEditorManager(provider.GetRequiredService<ISettingsProvider>())
                    .RegisterEditors(
                        [
                            new Default(fileName, true),
                            new NotepadPP(fileName),
                            new VSCode(fileName)
                        ]
                    );
        }
    }
}
