//
// Copyright © 2024 Enda Mullally.
//

// ReSharper disable InconsistentNaming

using Proc = System.Diagnostics.Process;

namespace EM.HostsManager.DNS
{
    public  class DNSHelper : IDNSHelper
    {
        public bool FlushDns()
        {
            try
            {
                // run "ipconfig /flushdns"

                var startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System),
                    FileName = "ipconfig.exe",
                    Arguments = "/flushdns",
                    Verb = "open"
                };

                var process = Proc.Start(startInfo);

                var error = true;

                if (process == null)
                {
                    return error;
                }

                process.WaitForExit();

                var exitCode = process.ExitCode;

                error = exitCode != 0;

                return error;
            }
            catch
            {
                // ignored
            }

            return false;
        }
    }
}
