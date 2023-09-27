namespace EM.HostsManager.App
{
    public static class Consts
    {
        public const string AppInstanceId = "Enda-Mullally|Hosts-Manager|2021-2023|V1|Single.Instance";

        public const string QuitArg = "/quit";

        public const string UninstallArg = "/uninstall";

        public const string MinArg = "/min";

        public const string ElevateArg = "/elevate";

        public const string AppRegPath = @"Software\Enda Mullally\Hosts Manager";

        public const string FirstRunShownForKey = @"FirstRunShownForVersion";

        public const string PreferredEditorKey = @"PreferredEditor";

        public const string ApplicationName = @"Hosts Manager";

        public const string
            RunAtStartupMessage = """
                  == Hosts Manager v{appVersion} ==
                  
                  Welcome to Hosts Manager! Just another Windows hosts file manager.
                  
                  Hosts Manager is a system tray application, it will automatically minimize to your system tray.

                  Startup app: By default, Hosts Manager will run at startup. You can disable auto start by un-checking 'Run at startup' in the system tray icon.

                  To exit Hosts Manager, right click the system tray icon and select Exit.

                  Thank you for installing Hosts Manager.

                  Enjoy!
                  """;

        public const string
            AboutMessage = """
                  == Hosts Manager ==
                  
                  https://github.com/enda-mullally/hostsmanager
                  
                  Version: {appVersion}
                  Commit: {commitId}
                  Date: {buildDate}
                  
                  Copyright © 2021-2023 Enda Mullally
                  """;
    }
}
