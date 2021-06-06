using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Forms;
using EM.HostsManager.App.Hosts;
using EM.HostsManager.App.Shell;
using EM.HostsManager.App.Version;

namespace EM.HostsManager.App.UI
{
    public partial class MainForm : Form
    {
        private static bool _aboutShown;
        private static bool _requestingClose;

        // Id for the About menu item in the System Menu
        public static int SysMenuAboutId = 0x1; 

        public MainForm()
        {
            InitializeComponent();

            UxFixButtonText();
            UxFixHeight();
            UxRefresh();
        }

        #region Ux

        private void UxFixHeight()
        {
            if (Elevated.IsElevated())
            {
                Height -= 45;
            }
        }
        
        private void UxFixButtonText()
        {
            if (!Elevated.IsElevated())
            {
                uxbtnEdit.Text = @"|Open &Hosts File";
            }

            uxbtnEdit.Text = uxbtnEdit.Text.Replace("|", Environment.NewLine);
            uxbtnDisableHostsFile.Text = uxbtnDisableHostsFile.Text.Replace("|", Environment.NewLine);
            uxbtnEnableHostsFile.Text = uxbtnEnableHostsFile.Text.Replace("|", Environment.NewLine);
            uxbtnFlushDNS.Text = uxbtnFlushDNS.Text.Replace("|", Environment.NewLine);
        }

        private void UxRefresh()
        {
            uxlblEnabled.Text = @"Hosts file is " + (HostsFile.IsEnabled()
                ? "enabled."
                : "disabled.");

            uxlblHostsFileSize.Text = HostsFile
                .HostsFileSize()
                .ToString("##,###0") + @" byte(s)";

            uxlblHostsCount.Text =
                HostsFile
                    .HostsCount()
                    .ToString("##,###0");

            var hostsEnabled = HostsFile.IsEnabled();

            uxMenuEnableHostsFile.Checked = hostsEnabled;

            var hostOrHosts = HostsFile
                .HostsCount() > 1 
                ? "hosts"
                : "host";
            
            uxNotifyIcon.Text = @"Hosts Manager" + (hostsEnabled
                ? " (" + uxlblHostsCount.Text + " " + hostOrHosts + " enabled)"
                : " (disabled)");

            // button state
            if (Elevated.IsElevated())
            {
                // Elevated, set button state appropriately
                uxbtnRunAsAdmin.Enabled =
                    uxbtnRunAsAdmin.Visible =
                        uxlblSep.Visible = false;

                uxbtnEdit.Enabled =
                    uxbtnDisableHostsFile.Enabled = hostsEnabled;
                
                uxbtnEnableHostsFile.Enabled = !hostsEnabled;

                uxbtnFlushDNS.Enabled = true;

                // Indicate that we are running as admin.
                if (!Text.Contains("[Administrator]"))
                {
                    Text += @" [Administrator]";
                }
            }
            else
            {
                // Not elevated, disable all except Run As Admin
                uxbtnDisableHostsFile.Enabled =
                        uxbtnEnableHostsFile.Enabled =
                            uxMenuEnableHostsFile.Enabled =
                                uxbtnFlushDNS.Enabled = false;

                // When Not elevated "edit/open" should only be available when the hosts file itself is enabled
                uxbtnEdit.Enabled = hostsEnabled;

                Elevated.AddShieldToButton(uxbtnRunAsAdmin);
            }
        }

        private void UxAbout()
        {
            _aboutShown = true;

            var appVersion = new AppVersion(Assembly.GetExecutingAssembly());

            MessageBox.Show(
                this,
                $@"== Hosts Manager =={Environment.NewLine}{Environment.NewLine}" +
                @"https://github.com/enda-mullally/hostsmanager" +
                $@"{Environment.NewLine}{Environment.NewLine}" +
                $@"Version: { appVersion.GetAppVersion() }{Environment.NewLine}" +
                $@"Build Date: { appVersion.GetBuildDate() }" +
                $@"{Environment.NewLine}{Environment.NewLine}" +
                @"Copyright © 2021 Enda Mullally",
                @"About",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information);

            _aboutShown = false;
        }

        #endregion

        #region Button Events

        private void uxbtnRunAsAdmin_Click(object sender, EventArgs e)
        {
            Elevated.RestartElevated("/uac");
        }

        private void uxbtnDisableHostsFile_Click(object sender, EventArgs e)
        {
            HostsFile.DisableHostsFile();
            
            UxRefresh();
        }

        private void uxbtnEnableHostsFile_Click(object sender, EventArgs e)
        {
            HostsFile.EnableHostsFile();
            
            UxRefresh();
        }

        private void uxbtnEdit_Click(object sender, EventArgs e)
        {
            var workingDirectory = Directory.GetParent(HostsFile.GetHostsFilename())?.FullName;

            if (workingDirectory == null)
            {
                return;
            }
            
            var startInfo = new ProcessStartInfo
            {
                UseShellExecute = true,
                WorkingDirectory = workingDirectory,
                FileName = "notepad.exe",
                Arguments = HostsFile.GetHostsFilename(),
                Verb = "open"
            };

            try
            {
                Process.Start(startInfo);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
        }

        private void uxbtnFlushDNS_Click(object sender, EventArgs e)
        {
            try
            {
                uxbtnFlushDNS.Enabled = false;

                // run "ipconfig /flushdns"
                var startInfo = new ProcessStartInfo
                {
                    UseShellExecute = true,
                    WindowStyle = ProcessWindowStyle.Hidden,
                    CreateNoWindow = true,
                    WorkingDirectory = Environment.GetFolderPath(Environment.SpecialFolder.System),
                    FileName = "ipconfig.exe",
                    Arguments = "/flushdns",
                    Verb = "open"
                };

                var process = Process.Start(startInfo);
                var error = true;
                if (process != null)
                {
                    process.WaitForExit();
                    var exitCode = process.ExitCode;
                    error = exitCode != 0;
                }

                if (error)
                {
                    MessageBox.Show(this,
                        @"Unknown error starting process 'ipconfig /flushdns'.",
                        @"Flush DNS",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Error);
                }
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception.Message);
            }
            finally
            {
                uxbtnFlushDNS.Enabled = true;
            }
        }
        
        #endregion

        #region Form Events

        private void MainForm_Activated(object sender, EventArgs e)
        {
            UxRefresh();
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            ShowInTaskbar = WindowState != FormWindowState.Minimized;
        }

        #endregion

        #region Menu Events

        private void uxMenuItemShow_Click(object sender, EventArgs e)
        {
            Visible =
                ShowIcon =
                    ShowInTaskbar = true;
            
            Opacity = 100;
            WindowState = FormWindowState.Normal;
        }
        
        private void uxMenuExit_Click(object sender, EventArgs e)
        {
            _requestingClose = true;

            Close();
        }

        private void uxMenuEnableHostsFile_Click(object sender, EventArgs e)
        {
            if (!Elevated.IsElevated())
            {
                return;
            }
            
            if (uxMenuEnableHostsFile.Checked)
            {
                HostsFile.EnableHostsFile();
            }
            else
            {
                HostsFile.DisableHostsFile();
            }

            if (Visible)
            {
                UxRefresh();
            }
        }

        private void uxMenuAbout_Click(object sender, EventArgs e)
        {
            if (!_aboutShown)
            {
                UxAbout();
            }
        }

        #endregion

        #region Protected

        protected override void OnLoad(EventArgs e)
        {
            var restarting =
                Environment.GetCommandLineArgs().Length > 1 &&
                (Environment.GetCommandLineArgs()[1].ToLowerInvariant() == "/show" ||
                 Environment.GetCommandLineArgs()[1].ToLowerInvariant() == "/uac");
                
            if (restarting)
            {
                ShowInTaskbar = true;
                
                return;
            }

            // Start minimized
            WindowState = FormWindowState.Minimized;
            Visible = false;
            ShowInTaskbar = false;
            Opacity = 0;
            base.OnLoad(e);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // ReSharper disable once RedundantAssignment
            var isDebug = false;
#if DEBUG
            isDebug = true;
#endif

            if (e.CloseReason == CloseReason.WindowsShutDown ||
                e.CloseReason == CloseReason.TaskManagerClosing ||
                _requestingClose ||
                isDebug)
            {
                base.OnFormClosing(e);
                
                return; // Windows shutdown, Explicit user exit or Debugging 
            }

            e.Cancel = true;
            WindowState = FormWindowState.Minimized;
            base.OnFormClosing(e);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            // Get a handle to a copy of this form's system (window) menu
            var hSysMenu = User32.GetSystemMenu(Handle, false);

            // Add a separator
            User32.AppendMenu(hSysMenu, User32.MfSeparator, 0, string.Empty);

            // Add the About menu item
            User32.AppendMenu(hSysMenu, User32.MfString, SysMenuAboutId, "&About");
        }

        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            // Test if the About item was selected from the system menu
            if (m.Msg == User32.WmSysCommand && (int)m.WParam == SysMenuAboutId)
            {
                uxMenuAbout_Click(null!, EventArgs.Empty);
            }
        }

        #endregion
    }
}
