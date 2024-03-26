//
// Copyright © 2021-2024 Enda Mullally.
//

namespace EM.HostsManager.Infrastructure.Registry;

public sealed class Registry : IRegistry
{
    public string GetRegString(RegistryKey rootRegistryKey, string key, string keyName, string defaultString = "")
    {
        RegistryKey? regKey = null;
        try
        {
            regKey = rootRegistryKey.OpenSubKey(key);
            if (regKey == null)
            {
                return defaultString;
            }

            var o = regKey.GetValue(keyName);

            if (o != null)
            {
                var value = (o as string);

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value;
                }
            }
        }
        catch
        {
            // ignore
        }
        finally
        {
            regKey?.Close();
        }

        return defaultString;
    }

    public bool SetRegString(RegistryKey rootRegistryKey, string key, string keyName, string value)
    {
        RegistryKey? regKey = null;
        try
        {
            regKey = rootRegistryKey.OpenSubKey(key, true) ??
                     rootRegistryKey.CreateSubKey(key);

            regKey.SetValue(keyName, value, RegistryValueKind.String);

            return true;
        }
        catch
        {
            // ignore
        }
        finally
        {
            regKey?.Close();
        }

        return false;
    }

    public bool DeleteRegString(RegistryKey rootRegistryKey, string path, string key)
    {
        RegistryKey? regKey = null;
        try
        {
            regKey = rootRegistryKey.OpenSubKey(path, true);

            regKey?.DeleteValue(key);

            return true;
        }
        catch
        {
            // ignore
        }
        finally
        {
            regKey?.Close();
        }

        return false;
    }
}