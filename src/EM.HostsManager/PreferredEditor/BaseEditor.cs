//
// Copyright © 2021-2025 Enda Mullally.
//

using NotImplementedException = System.NotImplementedException;

namespace EM.HostsManager.PreferredEditor
{
    public abstract class BaseEditor(string fileName, bool isDefault = false) : IEditor
    {
        public virtual string FileName
        {
            get => fileName;
            set
            {
                if (string.IsNullOrWhiteSpace(fileName))
                {
                    throw new ArgumentNullException(nameof(fileName));
                }

                if (File.Exists(fileName))
                {
                    throw new FileNotFoundException(nameof(fileName), fileName);
                }

                fileName = value;
            }
        }

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