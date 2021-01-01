using System;
using System.Runtime.InteropServices;

namespace HostsManager.Shell
{
    public static class User32
    {
        // P/Invoke constants
        public const int WmSysCommand = 0x112;
        public const int MfString = 0x0;
        public const int MfSeparator = 0x800;

        // P/Invoke declarations
        [DllImport("user32")]
        public static extern uint SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32")]
        public static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32")]
        public static extern bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);
    }
}
