namespace EM.HostsManager.Infrastructure.Process;

public interface IAppSingleInstance : IDisposable
{
    bool IsSingleInstance();

    void ActivateOtherProcess(int message);

    void QuitOtherProcess(int message);
}