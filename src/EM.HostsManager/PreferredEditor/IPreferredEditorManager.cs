//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.PreferredEditor;

public interface IPreferredEditorManager
{
    bool Open();

    IReadOnlyList<IEditor> GetEditors();

    public string GetDefaultEditorKey();

    public void SaveSelectedEditor(string key);

    public string GetSelectedEditorKey();
}