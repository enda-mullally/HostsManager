//
// Copyright © 2024 Enda Mullally.
//

// ReSharper disable InconsistentNaming

namespace EM.HostsManager.PreferredEditor.Editors
{
    public class VSCode(string fileName) : BaseEditor(fileName)
    {
        public override string Key => "VSCode";

        public override string DisplayName => "Visual Studio Code";

        public override bool Open()
        {
            var workingDirectory = Directory.GetParent(FileName)?.FullName;

            const string applicationName = "code";

            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = workingDirectory,
                FileName = applicationName,
                Arguments = FileName,
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