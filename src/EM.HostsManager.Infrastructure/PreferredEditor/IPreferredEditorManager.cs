//
// Copyright © 2024 Enda Mullally.
//

namespace EM.HostsManager.Infrastructure.PreferredEditor;

public interface IPreferredEditorManager
{
    bool Open();

    IReadOnlyList<IEditor> GetEditors();

    public string GetDefaultEditorKey();

    public void SaveSelectedEditor(string key);

    public string GetSelectedEditorKey();
}