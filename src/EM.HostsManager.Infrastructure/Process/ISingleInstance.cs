namespace EM.HostsManager.Infrastructure.Process;

public interface ISingleInstance : IDisposable
{
    bool IsSingleInstance();

    void ActivateOtherProcess(int message);

    void QuitOtherProcess(int message);
}