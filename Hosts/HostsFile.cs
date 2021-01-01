using System;
using System.IO;

namespace HostsManager.Hosts
{
    public static class HostsFile
    {
        private const string HostsDirLoc = @"drivers\etc\";
        private const string HostsFileName = @"hosts";
        private const string DisabledHostFileName = @"hosts.disabled";
        private const string DisabledHostFileEntry1 = @"# Hosts Manager";
        private const string DisabledHostFileEntry2 = @"# Disabled Hosts File";

        public static string GetHostsFilename()
        {
            return GetEnabledOrDisabledHostsFilename();
        }

        public static bool IsEnabled()
        {
            return !(HostsCount() == 0 &&
                     HostsContainsDisabledMarker());
        }

        public static int HostsCount()
        {
            var hostsFile = GetHostsFilename();

            if (!File.Exists(hostsFile))
            {
                return -1;
            }

            var hostNameCount = 0;

            using StreamReader sr = new StreamReader(hostsFile);
            while (sr.Peek() >= 0)
            {
                var nextLine = sr.ReadLine();

                if (nextLine!.TrimStart().StartsWith("#") ||
                    string.IsNullOrEmpty(nextLine.Trim()))
                {
                    // this line this looks like a comment or is empty, skip it
                }
                else
                {
                    if (nextLine.Contains(".")) // this line looks like a host name
                    {
                        hostNameCount++;
                    }
                }
            }

            return hostNameCount;
        }

        public static bool HostsContainsDisabledMarker()
        {
            var hostsFile = GetHostsFilename();

            if (!File.Exists(hostsFile))
            {
                return false;
            }

            var markerFound = false;

            using StreamReader sr = new StreamReader(hostsFile);
            while (sr.Peek() >= 0 && !markerFound)
            {
                var nextLine = sr.ReadLine();

                // this line doesn't look like a comment, skip it
                if (!nextLine!
                    .TrimStart()
                    .StartsWith("#"))
                {
                    continue;
                }
                
                if (nextLine
                    .Trim()
                    .ToLowerInvariant()
                    .Equals(DisabledHostFileEntry2.ToLowerInvariant()))
                {
                    markerFound = true;
                }
            }

            return markerFound;
        }

        public static long HostsFileSize()
        {
            return IO.File.FileSize(GetHostsFilename());
        }

        public static bool DisableHostsFile()
        {
            if (!IsEnabled())
            {
                return true;
            }

            var hostsFileName = GetHostsFilename();
            IO.File.CopyFileTo(hostsFileName, GetDisabledHostsFilename());
            IO.File.ReplaceContentWith(hostsFileName,
                DisabledHostFileEntry1 + Environment.NewLine + DisabledHostFileEntry2);

            return true;
        }

        public static bool EnableHostsFile()
        {
            if (IsEnabled())
            {
                return true;
            }
            
            var disabledHostsFileName = GetDisabledHostsFilename();
            IO.File.CopyFileTo(disabledHostsFileName, GetHostsFilename());
            IO.File.DeleteIfExists(disabledHostsFileName);

            return true;
        }

        #region Private

        private static string GetEnabledOrDisabledHostsFilename(bool enabledHostsFile = true)
        {
            var windowsSysDir =
                Environment.GetFolderPath(
                    Environment.SpecialFolder.System);
            
            return Path.Combine(windowsSysDir, HostsDirLoc,
                   enabledHostsFile
                       ? HostsFileName
                       : DisabledHostFileName);
        }

        private static string GetDisabledHostsFilename()
        {
            return GetEnabledOrDisabledHostsFilename(false);
        }
        
        #endregion
    }
}
