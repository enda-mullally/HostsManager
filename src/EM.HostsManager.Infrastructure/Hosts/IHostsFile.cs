namespace EM.HostsManager.Infrastructure.Hosts;

public interface IHostsFile
{
    public string GetHostsFilename();

    bool IsEnabled();

    int HostsCount();

    long HostsFileSize();

    bool DisableHostsFile();

    bool EnableHostsFile();
}