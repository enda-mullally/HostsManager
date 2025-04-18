//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.Registry;

public interface IRegistry
{
    string GetRegString(RegistryKey rootRegistryKey, string key, string keyName, string defaultString = "");

    bool SetRegString(RegistryKey rootRegistryKey, string key, string keyName, string value);

    bool DeleteRegString(RegistryKey rootRegistryKey, string path, string key);
}