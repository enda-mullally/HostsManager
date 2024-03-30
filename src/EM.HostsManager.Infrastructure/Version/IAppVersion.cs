//
// Copyright © 2021-2024 Enda Mullally.
//

namespace EM.HostsManager.Infrastructure.Version;

public interface IAppVersion
{
    string GetAppVersion();

    string? GetBuildDate();

    string? GetCommitId();
}