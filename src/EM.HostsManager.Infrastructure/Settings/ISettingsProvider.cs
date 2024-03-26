//
// Copyright © 2024 Enda Mullally.
//

namespace EM.HostsManager.Infrastructure.Settings
{
    public interface ISettingsProvider
    {
        T GetValue<T>(string key, T defaultValue = default!);

        bool SetValue<T>(string key, T value);

        bool DeleteValue(string key);
    }
}
