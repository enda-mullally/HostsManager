//
// Copyright © 2009-2022 Enda Mullally.
//

using EM.HostsManager.App.Win32;

namespace EM.HostsManager.App.Process;

using Procs=System.Diagnostics.Process;

public sealed class SingleInstance : IDisposable
{
    public const int WmUser = 0x0400;
    public const int WmActivateApp = WmUser + 55;
    public const int WmQuitApp = WmUser + 56;

    private readonly Mutex _processSync;
    private bool _owned;

    public SingleInstance(string identifier)
    {
        _processSync = new Mutex(
            true,
            Assembly.GetExecutingAssembly().GetName().Name + identifier,
            out _owned);
    }

    ~SingleInstance()
    {
        Release();
    }

    public bool IsSingleInstance(bool autoActivateOtherApp = false)
    {
        var result = _owned;

        if (autoActivateOtherApp && !_owned)
        {
            ActivateOtherProcess();
        }

        return result;
    }

    public void ActivateOtherProcess()
    {
        PostMessageToOtherProcess(WmActivateApp);
    }

    public void QuitOtherProcess()
    {
        PostMessageToOtherProcess(WmQuitApp);
    }

    private static void PostMessageToOtherProcess(int message)
    {
        var current = Procs.GetCurrentProcess();

        foreach (var process in Procs.GetProcessesByName(current.ProcessName))
        {
            if (process.Id == current.Id)
            {
                continue;
            }

            PostMessageToProcessWindows(process.MainWindowHandle, message);
            
            break;
        }
    }

    private static void PostMessageToProcessWindows(IntPtr windowToShow, int message)
    {
        foreach (var childWindow in WindowEnumerator.GetChildWindows(windowToShow))
        {
            User32.PostMessage(childWindow, message, IntPtr.Zero, IntPtr.Zero);
        }

        User32.PostMessage(windowToShow, message, IntPtr.Zero, IntPtr.Zero);
    }

    private void Release()
    {
        if (!_owned)
        {
            return;
        }
            
        _processSync.ReleaseMutex();
            
        _owned = false;
    }

    #region Implementation of IDisposable

    public void Dispose()
    {
        Release();

        GC.SuppressFinalize(this);
    }

    #endregion
}