//
// Copyright © 2021-2023 Enda Mullally.
//

using EM.HostsManager.Infrastructure.Win32;

namespace EM.HostsManager.App.UI.CustomForms
{
    public partial class AboutSysMenuForm : Form
    {
        private readonly string _aboutMenuText = string.Empty;

        private const int SysMenuAboutId = 0x1;

        public event EventHandler? SysAboutMenuClicked;

        protected virtual void OnSysAboutMenuClicked(EventArgs e)
        {
            SysAboutMenuClicked?.Invoke(this, e);
        }

        // Note: We need an empty constructor to support the winforms designer
        public AboutSysMenuForm()
        {
            InitializeComponent();
        }

        public AboutSysMenuForm(string aboutMenuText)
        {
            _aboutMenuText = aboutMenuText;

            InitializeComponent();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            var systemMenuHandle = User32.GetSystemMenu(Handle, false);

            // Add a separator
            User32.AppendMenu(systemMenuHandle, User32.MfSeparator, 0, string.Empty);

            // Add the About menu item
            User32.AppendMenu(
                systemMenuHandle,
                User32.MfString,
                SysMenuAboutId,
                string.IsNullOrWhiteSpace(_aboutMenuText) ? "&About" : _aboutMenuText);
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case User32.WmSysCommand when (int)m.WParam == SysMenuAboutId:

                    OnSysAboutMenuClicked(EventArgs.Empty);

                    break;
            }
        }
    }
}
