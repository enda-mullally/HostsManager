namespace EM.HostsManager.Infrastructure.AutoStart;

public interface IAutoStartManager
{
    bool SetupAutoRunAtStartup();

    bool DeleteAutoRunAtStartup();

    bool EnableDisableAutoRun(bool enable);

    bool IsAutoRunEnabled();
}