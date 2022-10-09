//
// Copyright © 2009-2022 Enda Mullally.
//


using EM.HostsManager.App.Win32;

namespace EM.HostsManager.App.Process
{
    public sealed class WindowEnumerator
    {
        public delegate bool EnumWindowProc(IntPtr hWnd, IntPtr parameter);

        public static IList<IntPtr> GetChildWindows(IntPtr parent)
        {
            var result = new List<IntPtr>();
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

        private static bool EnumWindow(IntPtr handle, IntPtr pointer)
        {
            var gch = GCHandle.FromIntPtr(pointer);

            if (gch.Target is not List<IntPtr> list)
            {
                throw new InvalidCastException("GCHandle Target could not be cast as List<IntPtr>");
            }

            list.Add(handle);

            return true;
        }
    }
}
