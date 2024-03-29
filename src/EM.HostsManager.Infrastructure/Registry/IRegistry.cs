﻿namespace EM.HostsManager.Infrastructure.Registry;

public interface IRegistry
{
    string GetRegString(RegistryKey rootRegistryKey, string key, string keyName, string defaultString = "");

    bool SetRegString(RegistryKey rootRegistryKey, string key, string keyName, string value);

    bool DeleteRegString(RegistryKey rootRegistryKey, string path, string key);
}