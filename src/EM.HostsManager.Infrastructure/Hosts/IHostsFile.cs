namespace EM.HostsManager.Infrastructure.Hosts;

public interface IHostsFile
{
    bool IsEnabled();

    int HostsCount();

    long HostsFileSize();

    bool DisableHostsFile();

    bool EnableHostsFile();
}