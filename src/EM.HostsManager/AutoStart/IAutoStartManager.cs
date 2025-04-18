//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.AutoStart;

public interface IAutoStartManager
{
    bool SetupAutoRunAtStartup();

    bool DeleteAutoRunAtStartup();

    bool EnableDisableAutoRun(bool enable);

    bool IsAutoRunEnabled();
}