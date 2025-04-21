//
// Copyright © 2009-2025 Enda Mullally.
//

#pragma warning disable SYSLIB1054

namespace EM.HostsManager.Win32;

public static class User32
{
    // P/Invoke constants
    public const int WmSysCommand = 0x112;
    public const int MfString = 0x0;
    public const int MfSeparator = 0x800;

    public enum ChangeWindowMessageFilterExAction : uint
    {
        Allow = 1,
    };

    // Public
    public static uint SendMessage(nint hWnd, int msg, uint wParam, uint lParam)
    {
        if (hWnd != nint.Zero && msg > 0)
        {
            return sendMessage(hWnd, msg, wParam, lParam);
        }

        return 0;
    }

    public static nint GetSystemMenu(nint hWnd, bool bRevert)
    {
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (hWnd != nint.Zero)
        {
            return getSystemMenu(hWnd, bRevert);
        }

        return nint.Zero;
    }

    public static bool AppendMenu(nint hMenu, int uFlags, int uIdNewItem, string lpNewItem)
    {
        if (uFlags == MfString && string.IsNullOrWhiteSpace(lpNewItem))
        {
            return false;
        }
            
        if (hMenu != nint.Zero && uFlags >= 0 && uIdNewItem >= 0)
        {
            return appendMenu(hMenu, uFlags, uIdNewItem, lpNewItem);
        }

        return false;
    }

    public static void PostMessage(nint hWnd, int msg, uint wParam, uint lParam)
    {
        if (hWnd != nint.Zero && msg > 0)
        {
            postMessage(hWnd, msg, wParam, lParam);
        }
    }

    public static bool EnumChildWindows(nint window, WindowEnumerator.EnumWindowProc callback, nint i)
    {
        return enumChildWindows(window, callback, i);
    }

    public static bool ChangeWindowMessageFilterEx(nint hWnd, int msg, ChangeWindowMessageFilterExAction action)
    {
        if (hWnd != nint.Zero && msg > 0)
        {
            return changeWindowMessageFilterEx(hWnd, msg, action, nint.Zero);
        }

        return false;
    }

    // P/Invoke declarations
    [DllImport("user32", EntryPoint = "SendMessage")]
    private static extern uint sendMessage(nint hWnd, int msg, uint wParam, uint lParam);

    [DllImport("user32", EntryPoint = "GetSystemMenu")]
    private static extern nint getSystemMenu(nint hWnd, bool bRevert);

    [DllImport("user32", EntryPoint = "AppendMenu", CharSet = CharSet.Unicode)]
    private static extern bool appendMenu(nint hMenu, int uFlags, int uIdNewItem, string lpNewItem);

    [DllImport("user32", EntryPoint = "PostMessage")]
    private static extern bool postMessage(nint hWnd, int msg, uint wParam, uint lParam);

    [DllImport("user32", EntryPoint = "EnumChildWindows")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool enumChildWindows(nint window, WindowEnumerator.EnumWindowProc callback, nint i);

    [DllImport("user32", EntryPoint = "ChangeWindowMessageFilterEx")]
    private static extern bool changeWindowMessageFilterEx(nint hWnd, int msg, ChangeWindowMessageFilterExAction action, nint filterStatus);
}