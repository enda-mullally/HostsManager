using EM.HostsManager.Infrastructure.Registry;

namespace EM.HostsManager.Infrastructure.PreferredEditor
{
    public class PreferredEditorManager(IRegistry registry, string appRegPath, string preferredEditorKey)
        : IPreferredEditorManager
    {
        private readonly List<IEditor> _editors = [];

        public PreferredEditorManager RegisterEditors(IEditor[] editors)
        {
            if (editors.Length == 0)
            {
                return this;
            }

            _editors.Clear();
            _editors.AddRange(editors);

            LoadSelectedEditor();

            return this;
        }

        public bool Open()
        {
            var editor = _editors.FirstOrDefault(e => e.Key.Equals(GetSelectedEditorKey()));

            return editor != null && editor.Open();
        }

        public IReadOnlyList<IEditor> GetEditors()
        {
            return _editors;
        }

        public string GetDefaultEditorKey()
        {
            return _editors.FirstOrDefault(e => e is { IsDefault: true })?.Key ?? string.Empty;
        }

        public string GetSelectedEditorKey()
        {
            return _editors.FirstOrDefault(e => e is { IsSelected: true })?.Key ?? string.Empty;
        }

        public void SaveSelectedEditor(string key)
        {
            foreach (var editor in _editors)
            {
                editor.IsSelected = editor.Key.Equals(key);
            }

            registry.SetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                appRegPath,
                preferredEditorKey,
                key);
        }

        private void LoadSelectedEditor()
        {
            var key = registry.GetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                appRegPath,
                preferredEditorKey,
                GetDefaultEditorKey());

            foreach (var editor in _editors)
            {
                editor.IsSelected = editor.Key.Equals(key);
            }
        }
    }
}