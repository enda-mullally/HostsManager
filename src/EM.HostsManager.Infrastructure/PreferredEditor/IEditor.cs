namespace EM.HostsManager.Infrastructure.PreferredEditor;

public interface IEditor
{
    public string Key { get; }

    public bool Open();
}