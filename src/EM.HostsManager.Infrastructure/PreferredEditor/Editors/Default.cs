using EM.HostsManager.Infrastructure.Hosts;

namespace EM.HostsManager.Infrastructure.PreferredEditor.Editors
{
    public class Default : IEditor
    {
        public string Key => "Default";

        public bool Open()
        {
            var workingDirectory = Directory.GetParent(HostsFile.GetHostsFilename())?.FullName;

            if (workingDirectory == null)
            {
                return false;
            }

            const string fileName = "notepad.exe";

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = workingDirectory,
                FileName = fileName,
                Arguments = HostsFile.GetHostsFilename(),
                Verb = "open"
            };

            try
            {
                System.Diagnostics.Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);

                return false;
            }

            return true;
        }
    }
}