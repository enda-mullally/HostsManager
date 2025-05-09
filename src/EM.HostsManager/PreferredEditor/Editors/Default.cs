﻿//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.PreferredEditor.Editors
{
    public class Default(string fileName, bool isDefault = false) : BaseEditor(fileName, isDefault)
    {
        public override string Key => "Default";

        public override string DisplayName => "Default";

        public override bool Open()
        {
            var workingDirectory = Directory.GetParent(FileName)?.FullName;
            
            const string applicationName = "notepad.exe";

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