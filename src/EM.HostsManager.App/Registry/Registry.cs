//
// Copyright © 2021-2022 Enda Mullally.
//

using Microsoft.Win32;
using Reg=Microsoft.Win32.Registry;

namespace EM.HostsManager.App.Registry
{
    public sealed class Registry
    {
        public static string GetCurrentUserRegString(string key, string keyName, string defaultString = "")
        {
            RegistryKey? regKey = null;
            try
            {
                regKey = Reg.CurrentUser.OpenSubKey(key);
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

        public static bool SetCurrentUserRegString(string key, string keyName, string value)
        {
            RegistryKey? regKey = null;
            try
            {
                regKey = Reg.CurrentUser.OpenSubKey(key, true);

                regKey!.SetValue(keyName, value, RegistryValueKind.String);

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
}
