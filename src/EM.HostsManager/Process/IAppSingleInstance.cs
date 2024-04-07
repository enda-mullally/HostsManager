//
// Copyright © 2021-2024 Enda Mullally.
//

namespace EM.HostsManager.Process;

public interface IAppSingleInstance : IDisposable
{
    bool IsSingleInstance();

    void ActivateOtherProcess(int message);

    void QuitOtherProcess(int message);
}