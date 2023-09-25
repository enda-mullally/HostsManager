//
// Copyright © 2009-2023 Enda Mullally.
//

using EM.HostsManager.Infrastructure.Win32;

namespace EM.HostsManager.Infrastructure.Process;

using Procs = System.Diagnostics.Process;

public sealed class SingleInstance : IDisposable
{
    private readonly Mutex _processSync;
    private bool _owned;

    public SingleInstance(string identifier)
    {
        _processSync = new Mutex(
            true,
            Assembly.GetCallingAssembly().GetName().Name + identifier, 
            out _owned);
    }

    ~SingleInstance()
    {
        Release();
    }

    public bool IsSingleInstance()
    {
        return _owned;
    }

    public void ActivateOtherProcess(int message)
    {
        if (message > 0)
        {
            PostMessageToOtherProcess(message);
        }
    }

    public void QuitOtherProcess(int message)
    {
        if (message > 0)
        {
            PostMessageToOtherProcess(message);
        }
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
            User32.PostMessage(childWindow, message, 0, 0);
        }

        User32.PostMessage(windowToShow, message, 0, 0);
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