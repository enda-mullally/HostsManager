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
        public static uint SendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam)
        {
            if (hWnd != IntPtr.Zero && msg > 0)
            {
                return sendMessage(hWnd, msg, wParam, lParam);
            }

            return 0;
        }

        public static IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert)
        {
            return hWnd != IntPtr.Zero ? getSystemMenu(hWnd, bRevert) : IntPtr.Zero;
        }

        public static bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem)
        {
            if (hMenu != IntPtr.Zero && uFlags > 0 && uIdNewItem > 0 && !string.IsNullOrWhiteSpace(lpNewItem))
            {
                return appendMenu(hMenu, uFlags, uIdNewItem, lpNewItem);
            }

            return false;
        }

        // P/Invoke declarations
        [DllImport("user32", EntryPoint = "SendMessage")]
        private static extern uint sendMessage(IntPtr hWnd, uint msg, uint wParam, uint lParam);

        [DllImport("user32", EntryPoint = "GetSystemMenu")]
        private static extern IntPtr getSystemMenu(IntPtr hWnd, bool bRevert);

        [DllImport("user32", EntryPoint = "AppendMenu")]
        private static extern bool appendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);
    }
}
