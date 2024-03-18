using EM.HostsManager.Infrastructure.Hosts;

// ReSharper disable InconsistentNaming

namespace EM.HostsManager.Infrastructure.PreferredEditor.Editors
{
    public class NotepadPP() : BaseEditor()
    {
        public override string Key => "NotepadPP";

        public override string DisplayName => "Notepad++";

        public override bool Open()
        {
            var workingDirectory = Directory.GetParent(HostsFile.GetHostsFilename())?.FullName;

            if (workingDirectory == null)
            {
                return false;
            }

            const string fileName = "notepad++.exe";

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