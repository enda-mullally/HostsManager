﻿using static EM.HostsManager.App.Process.WindowEnumerator;

namespace EM.HostsManager.App.Win32;

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
    public static uint SendMessage(IntPtr hWnd, int msg, uint wParam, uint lParam)
    {
        if (hWnd != IntPtr.Zero && msg > 0)
        {
            return sendMessage(hWnd, msg, wParam, lParam);
        }

        return 0;
    }

    public static IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert)
    {
        // ReSharper disable once ConvertIfStatementToReturnStatement
        if (hWnd != IntPtr.Zero)
        {
            return getSystemMenu(hWnd, bRevert);
        }

        return IntPtr.Zero;
    }

    public static bool AppendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem)
    {
        if (uFlags == MfString && string.IsNullOrWhiteSpace(lpNewItem))
        {
            return false;
        }
            
        if (hMenu != IntPtr.Zero && uFlags >= 0 && uIdNewItem >= 0)
        {
            return appendMenu(hMenu, uFlags, uIdNewItem, lpNewItem);
        }

        return false;
    }

    public static void PostMessage(IntPtr hWnd, int msg, uint wParam, uint lParam)
    {
        if (hWnd != IntPtr.Zero && msg > 0)
        {
            postMessage(hWnd, msg, wParam, lParam);
        }
    }

    public static bool EnumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i)
    {
        return enumChildWindows(window, callback, i);
    }

    public static bool ChangeWindowMessageFilterEx(IntPtr hWnd, int msg, ChangeWindowMessageFilterExAction action)
    {
        if (hWnd != IntPtr.Zero && msg > 0)
        {
            return changeWindowMessageFilterEx(hWnd, msg, action, IntPtr.Zero);
        }

        return false;
    }

    // P/Invoke declarations
    [DllImport("user32", EntryPoint = "SendMessage")]
    private static extern uint sendMessage(IntPtr hWnd, int msg, uint wParam, uint lParam);

    [DllImport("user32", EntryPoint = "GetSystemMenu")]
    private static extern IntPtr getSystemMenu(IntPtr hWnd, bool bRevert);

    [DllImport("user32", EntryPoint = "AppendMenu", CharSet = CharSet.Unicode)]
    private static extern bool appendMenu(IntPtr hMenu, int uFlags, int uIdNewItem, string lpNewItem);

    [DllImport("user32", EntryPoint = "PostMessage")]
    private static extern bool postMessage(IntPtr hWnd, int msg, uint wParam, uint lParam);

    [DllImport("user32", EntryPoint = "EnumChildWindows")]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool enumChildWindows(IntPtr window, EnumWindowProc callback, IntPtr i);

    [DllImport("user32", EntryPoint = "ChangeWindowMessageFilterEx")]
    private static extern bool changeWindowMessageFilterEx(IntPtr hWnd, int msg, ChangeWindowMessageFilterExAction action, IntPtr filterStatus);
}