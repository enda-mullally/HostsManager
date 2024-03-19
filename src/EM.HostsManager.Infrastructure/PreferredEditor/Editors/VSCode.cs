using EM.HostsManager.Infrastructure.Hosts;

// ReSharper disable InconsistentNaming

namespace EM.HostsManager.Infrastructure.PreferredEditor.Editors
{
    public class VSCode : BaseEditor
    {
        public override string Key => "VSCode";

        public override string DisplayName => "Visual Studio Code";

        public override bool Open()
        {
            var workingDirectory = Directory.GetParent(HostsFile.GetHostsFilename())?.FullName;

            if (workingDirectory == null)
            {
                return false;
            }

            const string fileName = "code";

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = workingDirectory,
                FileName = fileName,
                Arguments = HostsFile.GetHostsFilename(),
                Verb = "open",
                WindowStyle = ProcessWindowStyle.Hidden, // Fix for VSCode, prevent code console window from displaying
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