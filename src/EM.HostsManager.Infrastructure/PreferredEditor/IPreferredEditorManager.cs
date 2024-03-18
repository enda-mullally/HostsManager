namespace EM.HostsManager.Infrastructure.PreferredEditor;

public interface IPreferredEditorManager
{
    void RegisterEditor(IEditor? editor);

    bool Open(string key);

    IReadOnlyList<IEditor?> GetEditors();

    public string GetDefaultEditorKey();
}