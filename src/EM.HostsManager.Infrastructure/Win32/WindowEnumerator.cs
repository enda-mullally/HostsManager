//
// Copyright © 2009-2023 Enda Mullally.
//

namespace EM.HostsManager.Infrastructure.Win32;

public sealed class WindowEnumerator
{
    public delegate bool EnumWindowProc(nint hWnd, nint parameter);

    public static IList<nint> GetChildWindows(nint parent)
    {
        var result = new List<nint>();
        var listHandle = GCHandle.Alloc(result);

        try
        {
            var childProc = new EnumWindowProc(EnumWindow);

            User32.EnumChildWindows(parent, childProc, GCHandle.ToIntPtr(listHandle));
        }
        finally
        {
            if (listHandle.IsAllocated)
            {
                listHandle.Free();
            }
        }

        return result;
    }

    private static bool EnumWindow(nint handle, nint pointer)
    {
        var gch = GCHandle.FromIntPtr(pointer);

        if (gch.Target is not List<nint> list)
        {
            throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
        }

        list.Add(handle);

        return true;
    }
}