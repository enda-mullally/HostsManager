using System;
using System.Runtime.InteropServices;

namespace EM.HostsManager.App.Shell
{
    public static class User32
    {
        // P/Invoke constants
        public const int WmSysCommand = 0x112;
        public const int MfString = 0x0;
        public const int MfSeparator = 0x800;

        // Public
        public static uint SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam) =>
            sendMessage(hWnd, msg, wParam, lParam);

        public static IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert) => getSystemMenu(hWnd, bRevert);

        public static bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem) =>
            appendMenu(hMenu, uFlags, uIdNewItem, lpNewItem);

        // P/Invoke declarations
        [DllImport("user32", EntryPoint = "SendMessage")]
        private static extern uint sendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr getSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32", EntryPoint = "AppendMenu")]
        private static extern bool appendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);
    }
}
