namespace EM.HostsManager.Infrastructure.PreferredEditor
{
    public class PreferredEditorManager : IPreferredEditorManager
    {
        private readonly List<IEditor?> _editors = [];

        public void RegisterEditor(IEditor? editor)
        {
            if (_editors.Contains(editor))
            {
                return;
            }

            _editors.Add(editor);
        }

        public bool Open(string key)
        {
            var editor = _editors.FirstOrDefault(e => e != null && e.Key.Equals(key));

            return editor != null && editor.Open();
        }

        public IReadOnlyList<IEditor?> GetEditors()
        {
            return _editors;
        }

        public string GetDefaultEditorKey()
        {
            return _editors.FirstOrDefault(e => e is { IsDefault: true })?.Key ?? string.Empty;
        }
    }
}