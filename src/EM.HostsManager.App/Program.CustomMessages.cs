namespace EM.HostsManager.App
{
    internal static partial class Program
    {
        private const int WmUser = 0x0400;

        public const int WmActivateApp = WmUser + 55;

        public const int WmQuitApp = WmUser + 56;

        public const int WmUninstallApp = WmUser + 57;
    }
}
