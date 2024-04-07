//
// Copyright © 2024 Enda Mullally.
//

using EM.HostsManager.Settings;

namespace EM.HostsManager.PreferredEditor
{
    public class PreferredEditorManager(ISettingsProvider settings) : IPreferredEditorManager
    {
        private readonly List<IEditor> _editors = [];
        private const string PreferredEditorKey = @"PreferredEditor";

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

            settings.SetValue(PreferredEditorKey, key);
        }

        private void LoadSelectedEditor()
        {
            var key = settings.GetValue(PreferredEditorKey, GetDefaultEditorKey());

            foreach (var editor in _editors)
            {
                editor.IsSelected = editor.Key.Equals(key);
            }
        }
    }
}