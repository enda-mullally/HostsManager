namespace EM.HostsManager.Infrastructure.Hosts;

public interface IHostsFile
{
    bool IsEnabled();

    int HostsCount();

    bool HostsContainsDisabledMarker();

    long HostsFileSize();

    bool DisableHostsFile();

    bool EnableHostsFile();
}