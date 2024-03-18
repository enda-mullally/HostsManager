namespace EM.HostsManager.Infrastructure.PreferredEditor
{
    public class PreferredEditorManager
    {
        private readonly Dictionary<string, IEditor> _editors = [];
        
        public void RegisterEditor(string key, IEditor editor)
        {
            _editors.TryAdd(key, editor);
        }

        public bool Open(string key)
        {
            if (_editors.TryGetValue(key, out var editor))
            {
                return editor.Open();
            };

            return false;
        }
    }
}
