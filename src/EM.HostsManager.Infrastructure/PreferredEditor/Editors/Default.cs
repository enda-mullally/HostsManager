using EM.HostsManager.Infrastructure.Hosts;

namespace EM.HostsManager.Infrastructure.PreferredEditor.Editors
{
    public class Default(bool isDefault = false) : BaseEditor(isDefault)
    {
        public override string Key => "Default";

        public override string DisplayName => "Default";

        public override bool Open()
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