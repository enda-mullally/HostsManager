//
// Copyright © 2021-2024 Enda Mullally.
//

namespace EM.HostsManager.Hosts;

public interface IHostsFile
{
    string GetHostsFilename();

    bool IsEnabled();

    int HostsCount();

    long HostsFileSize();

    bool DisableHostsFile();

    bool EnableHostsFile();
}