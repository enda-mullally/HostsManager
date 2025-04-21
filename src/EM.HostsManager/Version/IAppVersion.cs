//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.Version;

public interface IAppVersion
{
    string GetAppVersion();

    string? GetBuildDate();

    string? GetCommitId();
}