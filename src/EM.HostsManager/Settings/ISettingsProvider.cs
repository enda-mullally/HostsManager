//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.Settings
{
    public interface ISettingsProvider
    {
        T GetValue<T>(string key, T defaultValue = default!);

        bool SetValue<T>(string key, T value);

        bool DeleteValue(string key);
    }
}