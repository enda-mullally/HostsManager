//
// Copyright © 2021-2024 Enda Mullally.
//

using EM.HostsManager.Attributes;

namespace EM.HostsManager.Version;

public class AppVersion(Assembly assembly) : IAppVersion
{
    private readonly Assembly _assembly =
        assembly ?? throw new ArgumentNullException(nameof(assembly));

    public string GetAppVersion()
    {
        var version = _assembly
            .GetName()
            .Version;

        return version!.Major + "." +
               version.Minor + "." +
               version.Build;
    }

    public string? GetBuildDate()
    {
        var buildDateAttr = _assembly
            .GetCustomAttributes(typeof(BuildDateAttribute), false)
            .Cast<BuildDateAttribute>()
            .FirstOrDefault();

        return buildDateAttr?.BuildDate;
    }

    public string? GetCommitId()
    {
        var commitIdAttr = _assembly
            .GetCustomAttributes(typeof(CommitIdAttribute), false)
            .Cast<CommitIdAttribute>()
            .FirstOrDefault();

        return commitIdAttr?.CommitId;
    }
}