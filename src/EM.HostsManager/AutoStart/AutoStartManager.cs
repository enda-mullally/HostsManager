//
// Copyright © 2024 Enda Mullally.
//

using EM.HostsManager.Registry;

namespace EM.HostsManager.AutoStart
{
    public class AutoStartManager(
        IRegistry registry,
        string applicationExePath,
        string startupArgs,
        string applicationName) : IAutoStartManager
    {
        private const string RunAtStartupMainRegPath =
            @"Software\Microsoft\Windows\CurrentVersion\Run";

        private const string RunAtStartupApprovedRegPath =
            @"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

        private const string DoubleQuote = "\"";

        public bool SetupAutoRunAtStartup()
        {
            registry.SetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                RunAtStartupMainRegPath,
                applicationName,
                $"{DoubleQuote}{applicationExePath}{DoubleQuote} {startupArgs}");

            return EnableDisableAutoRun(true);
        }

        public bool DeleteAutoRunAtStartup()
        {
            var result = registry.DeleteRegString(
                Microsoft.Win32.Registry.CurrentUser,
                RunAtStartupMainRegPath,
                applicationName) && registry.DeleteRegString(
                Microsoft.Win32.Registry.CurrentUser,
                RunAtStartupApprovedRegPath,
                applicationName);

            return result;
        }

        public bool EnableDisableAutoRun(bool enable)
        {
            var result = false;

            try
            {
                bool isBinaryKey;

                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RunAtStartupApprovedRegPath))
                {
                    if (key != null)
                    {
                        var valueType = key.GetValueKind(applicationName);

                        isBinaryKey = valueType == RegistryValueKind.Binary;
                    }
                    else
                    {
                        isBinaryKey = true; // Key not found, on new machines, default to Binary key
                    }
                }

                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RunAtStartupApprovedRegPath, true))
                {
                    if (key == null)
                    {
                        return result;
                    }

                    switch (isBinaryKey)
                    {
                        case true:
                        {
                            if (key.GetValue(applicationName) is byte[] binaryData)
                            {
                                // Modify the binary data to enable the entry (exact bit manipulation depends on the format)
                                // Example: Set the first byte to 0x02 to enable 0x03 to disable
                                binaryData[0] = (byte)(enable ? 0x02 : 0x03);

                                // Write the modified binary data back to the registry
                                key.SetValue(applicationName, binaryData, RegistryValueKind.Binary);

                                result = true;
                            }

                            break;
                        }
                        case false:
                        {
                            if (key.GetValue(applicationName) is RegistryValueKind.DWord)
                            {
                                // Set the value to 3 to disable startup
                                key.SetValue(applicationName, enable ? 2 : 3, RegistryValueKind.DWord);

                                result = true;
                            }

                            break;
                        }
                    }

                }
            }
            catch
            {
                if (enable)
                {
                    // Create the key if needed (only on enable)
                    using var keyCreate =
                        Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RunAtStartupApprovedRegPath);
                    {
                        var binaryData = new byte[] { 0x02 };

                        // Write the modified binary data back to the registry
                        keyCreate.SetValue(applicationName, binaryData, RegistryValueKind.Binary);
                    }

                    return true;
                }
            }

            return result;
        }

        public bool IsAutoRunEnabled()
        {
            try
            {
                var mainRegValue = registry.GetRegString(
                    Microsoft.Win32.Registry.CurrentUser,
                    RunAtStartupMainRegPath,
                    applicationName);

                var mainAutoRunKeyExistsAndIsValid =
                    mainRegValue.Contains(applicationExePath, StringComparison.InvariantCultureIgnoreCase);

                if (!mainAutoRunKeyExistsAndIsValid)
                {
                    return false;
                }

                bool isBinaryKey;

                // Now get the approved key value [It will be either a dword or binary value key]

                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RunAtStartupApprovedRegPath))
                {
                    if (key != null)
                    {
                        var valueType = key.GetValueKind(applicationName);

                        isBinaryKey = valueType == RegistryValueKind.Binary;
                    }
                    else
                    {
                        isBinaryKey = true; // Key not found, on new machines, default to Binary key
                    }
                }

                var enableResult = false;

                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RunAtStartupApprovedRegPath, true))
                {
                    if (key == null)
                    {
                        return false;
                    }

                    switch (isBinaryKey)
                    {
                        case true:
                        {
                            if (key.GetValue(applicationName) is byte[] binaryData)
                            {
                                enableResult = binaryData[0] == 0x02;
                            }

                            break;
                        }
                        case false:
                        {
                            if (key.GetValue(applicationName) is long longVal)
                            {
                                enableResult = longVal == 2;
                            }

                            break;
                        }
                    }
                }

                return mainAutoRunKeyExistsAndIsValid && enableResult;
            }
            catch
            {
                return false;
            }
        }
    }
}