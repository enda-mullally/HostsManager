using NotImplementedException = System.NotImplementedException;

namespace EM.HostsManager.Infrastructure.PreferredEditor.Editors
{
    public abstract class BaseEditor(bool isDefault = false) : IEditor
    {
        public virtual string Key => "";
        
        public virtual string DisplayName => "";

        public virtual bool IsDefault { get; } = isDefault;

        public virtual bool IsSelected { get; private set; } = false;

        protected void SetSelected(bool selected)
        {
            IsSelected = selected;
        }

        public virtual bool Open()
        {
            throw new NotImplementedException();
        }
    }
}