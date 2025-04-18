//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.Settings.Providers
{
    public class RegistrySettingsProvider(RegistryKey rootRegistryKey, string regPath) : ISettingsProvider
    {
        public T GetValue<T>(string key, T defaultValue = default!)
        {
            var reg = rootRegistryKey.OpenSubKey(regPath);
            try
            {
                if (reg == null)
                {
                    return defaultValue;
                }

                switch (typeof(T).Name)
                {
                    case nameof(String):
                    {
                        var registryValue = reg.GetValue(key, defaultValue as string ?? string.Empty);
                        return (T)registryValue;
                    }
                    default:
                        throw new NotImplementedException("Type not available yet");
                }
            }
            finally
            {
                reg?.Close();
            }
        }

        public bool SetValue<T>(string key, T value)
        {
            var reg =
                rootRegistryKey.OpenSubKey(regPath, true) ?? rootRegistryKey.CreateSubKey(regPath);

            try
            {
                switch (typeof(T).Name)
                {
                    case nameof(String):
                    {
                        reg.SetValue(key, value as string ?? string.Empty);
                        return true;
                    }
                    default:
                        throw new NotImplementedException("Type not available yet");
                }
            }
            finally
            {
                reg.Close();
            }
        }

        public bool DeleteValue(string key)
        {
            RegistryKey? reg = null;
            try
            {
                reg = rootRegistryKey.OpenSubKey(regPath, true);

                reg?.DeleteValue(key);

                return true;
            }
            catch
            {
                // ignore
            }
            finally
            {
                reg?.Close();
            }

            return false;
        }
    }
}