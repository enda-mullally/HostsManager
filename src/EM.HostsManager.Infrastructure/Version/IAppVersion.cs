namespace EM.HostsManager.Infrastructure.Version;

public interface IAppVersion
{
    string GetAppVersion();

    string? GetBuildDate();

    string? GetCommitId();
}