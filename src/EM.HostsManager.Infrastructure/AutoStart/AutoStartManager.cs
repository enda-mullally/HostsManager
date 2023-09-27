using Reg = EM.HostsManager.Infrastructure.Registry.Registry;

namespace EM.HostsManager.Infrastructure.AutoStart
{
    public class AutoStartManager
    {
        private readonly string _applicationExePath;
        private readonly string _startupArgs;
        private readonly string _applicationName;

        public AutoStartManager(
            string applicationExePath,
            string startupArgs,
            string applicationName)
        {
            _applicationExePath = applicationExePath;
            _startupArgs = startupArgs;
            _applicationName = applicationName;
        }

        private const string RunAtStartupMainRegPath =
            @"Software\Microsoft\Windows\CurrentVersion\Run";

        private const string RunAtStartupApprovedRegPath =
            @"Software\Microsoft\Windows\CurrentVersion\Explorer\StartupApproved\Run";

        private const string DoubleQuote = "\"";

        public bool SetupAutoRunAtStartup()
        {
            Reg.SetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                RunAtStartupMainRegPath,
                _applicationName,
                $"{DoubleQuote}{_applicationExePath}{DoubleQuote} {_startupArgs}");

            return EnableDisableAutoRun(true);
        }

        public bool DeleteAutoRunAtStartup()
        {
            var result = Reg.DeleteRegString(
                Microsoft.Win32.Registry.CurrentUser,
                RunAtStartupMainRegPath,
                _applicationName) && Reg.DeleteRegString(
                    Microsoft.Win32.Registry.CurrentUser,
                    RunAtStartupApprovedRegPath,
                    _applicationName);

            return result;
        }

        public bool EnableDisableAutoRun(bool enable)
        {
            var result = false;

            try
            {
                var isBinaryKey = false;

                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RunAtStartupApprovedRegPath))
                {
                    if (key != null)
                    {
                        var valueType = key.GetValueKind(_applicationName);

                        isBinaryKey = valueType == RegistryValueKind.Binary;
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
                            if (key.GetValue(_applicationName) is byte[] binaryData)
                            {
                                // Modify the binary data to enable the entry (exact bit manipulation depends on the format)
                                // Example: Set the first byte to 0x02 to enable 0x03 to disable
                                binaryData[0] = (byte)(enable ? 0x02 : 0x03);

                                // Write the modified binary data back to the registry
                                key.SetValue(_applicationName, binaryData, RegistryValueKind.Binary);

                                result = true;
                            }

                            break;
                        }
                        case false:
                        {
                            if (key.GetValue(_applicationName) is RegistryValueKind.DWord)
                            {
                                // Set the value to 3 to disable startup
                                key.SetValue(_applicationName, enable ? 2 : 3, RegistryValueKind.DWord);

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
                    using var keyCreate = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(RunAtStartupApprovedRegPath);
                    {
                        var binaryData = new byte[] { 0x02 };

                        // Write the modified binary data back to the registry
                        keyCreate.SetValue(_applicationName, binaryData, RegistryValueKind.Binary);
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
                var mainRegValue = Reg.GetRegString(
                    Microsoft.Win32.Registry.CurrentUser,
                    RunAtStartupMainRegPath,
                    _applicationName);

                var mainAutoRunKeyExistsAndIsValid =
                    mainRegValue.Contains(_applicationExePath, StringComparison.InvariantCultureIgnoreCase);

                if (!mainAutoRunKeyExistsAndIsValid)
                {
                    return false;
                }

                var isBinaryKey = false;

                // Now get the approved key value [It will be either a dword or binary value key]

                using (var key = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(RunAtStartupApprovedRegPath))
                {
                    if (key != null)
                    {
                        var valueType = key.GetValueKind(_applicationName);

                        isBinaryKey = valueType == RegistryValueKind.Binary;
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
                            if (key.GetValue(_applicationName) is byte[] binaryData)
                            {
                                enableResult = binaryData[0] == 0x02;
                            }

                            break;
                        }
                        case false:
                        {
                            if (key.GetValue(_applicationName) is long longVal)
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
