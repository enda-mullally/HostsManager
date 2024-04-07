//
// Copyright © 2021-2024 Enda Mullally.
//

using EM.HostsManager.Win32;
using static EM.HostsManager.Win32.User32;

namespace EM.HostsManager.UI.CustomForms
{
    public partial class AboutSysMenuForm : Form
    {
        private readonly string _aboutMenuText = string.Empty;

        private const int SysMenuAboutId = 0x1;

        public event EventHandler? SysAboutMenuClicked;

        public event EventHandler<CustomMessageEventArgs>? CustomMessageReceived;

        protected virtual void OnSysAboutMenuClicked(EventArgs e)
        {
            SysAboutMenuClicked?.Invoke(this, e);
        }

        protected virtual void OnCustomMessageReceived(CustomMessageEventArgs e)
        {
            CustomMessageReceived?.Invoke(this, e);
        }

        private readonly List<int> _customMessages = [];

        // Note: We need an empty constructor to support the winforms designer
        public AboutSysMenuForm()
        {
            InitializeComponent();
        }

        public AboutSysMenuForm(string aboutMenuText, params int[] customMessages)
        {
            _aboutMenuText = aboutMenuText;

            _customMessages.AddRange(customMessages);

            InitializeComponent();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            var systemMenuHandle = GetSystemMenu(Handle, false);

            // Add a separator
            AppendMenu(systemMenuHandle, MfSeparator, 0, string.Empty);

            // Add the About menu item
            AppendMenu(
                systemMenuHandle,
                MfString,
                SysMenuAboutId,
                string.IsNullOrWhiteSpace(_aboutMenuText) ? "&About" : _aboutMenuText);

            if (!Elevated.IsElevated())
            {
                return;
            }

            // UIPI bypass for our custom messages if the app is elevated.

            foreach (var customMessage in _customMessages)
            {
                ChangeWindowMessageFilterEx(
                    Handle,
                    customMessage,
                    ChangeWindowMessageFilterExAction.Allow);
            }
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            switch (m.Msg)
            {
                case WmSysCommand when (int)m.WParam == SysMenuAboutId:

                    OnSysAboutMenuClicked(EventArgs.Empty);

                    break;

                default:
                {
                    foreach (var customMessage in _customMessages)
                    {
                        if (m.Msg != customMessage)
                        {
                            continue;
                        }

                        OnCustomMessageReceived(
                            new CustomMessageEventArgs
                            {
                                Msg = m.Msg
                            });

                        // exit foreach early, there is no need to continue
                        // searching, we'll only ever match one custom message
                        // per each WndProc event.
                        break;
                    }

                    break;
                }
            }
        }
    }
}
