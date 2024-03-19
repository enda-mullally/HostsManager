using NotImplementedException = System.NotImplementedException;

namespace EM.HostsManager.Infrastructure.PreferredEditor
{
    public abstract class BaseEditor(bool isDefault = false) : IEditor
    {
        public virtual string Key => "";

        public virtual string DisplayName => "";

        public virtual bool IsDefault { get; } = isDefault;

        public virtual bool IsSelected { get; set; }

        public virtual bool Open()
        {
            throw new NotImplementedException();
        }
    }
}