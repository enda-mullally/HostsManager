// ReSharper disable InconsistentNaming

namespace EM.HostsManager.Infrastructure.PreferredEditor.Editors
{
    public class NotepadPP(string fileName) : BaseEditor(fileName)
    {
        public override string Key => "NotepadPP";

        public override string DisplayName => "Notepad++";

        public override bool Open()
        {
            var workingDirectory = Directory.GetParent(FileName)?.FullName;

            const string applicationName = "notepad++.exe";

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = workingDirectory,
                FileName = applicationName,
                Arguments = FileName,
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