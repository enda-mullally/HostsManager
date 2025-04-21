//
// Copyright © 2021-2025 Enda Mullally.
//

namespace EM.HostsManager.PreferredEditor;

public interface IEditor
{
    public string Key { get; }

    public string DisplayName { get; }

    public bool IsDefault { get; }

    public bool IsSelected { get; set; }

    public bool Open();
}