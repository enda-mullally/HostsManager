//
// Copyright © 2024 Enda Mullally.
//

namespace EM.HostsManager.AutoStart;

public interface IAutoStartManager
{
    bool SetupAutoRunAtStartup();

    bool DeleteAutoRunAtStartup();

    bool EnableDisableAutoRun(bool enable);

    bool IsAutoRunEnabled();
}