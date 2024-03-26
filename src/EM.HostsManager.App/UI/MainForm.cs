//
// Copyright © 2021-2024 Enda Mullally.
//

using EM.HostsManager.App.Properties;
using EM.HostsManager.App.Uninstall;
using EM.HostsManager.Infrastructure.AutoStart;
using EM.HostsManager.Infrastructure.Hosts;
using EM.HostsManager.Infrastructure.PreferredEditor;
using EM.HostsManager.Infrastructure.Settings;
using EM.HostsManager.Infrastructure.UI.CustomForms;
using EM.HostsManager.Infrastructure.Version;
using EM.HostsManager.Infrastructure.Win32;

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

#pragma warning disable IDE1006 // Naming Styles

namespace EM.HostsManager.App.UI;

public partial class MainForm : AboutSysMenuForm
{
    private bool _aboutShown;
    private bool _requestingClose;

    private readonly IHostsFile _hostsFile;
    private readonly ISettingsProvider _settings;
    private readonly IAutoStartManager _autoStartManager;
    private readonly IAppUninstaller _uninstaller;
    private readonly IAppVersion _appVersion;
    private readonly IPreferredEditorManager _preferredEditorManager;

    public MainForm(
        IHostsFile hostsFile,
        ISettingsProvider settings,
        IAutoStartManager autoStartManager,
        IAppUninstaller uninstaller,
        IAppVersion appVersion,
        IPreferredEditorManager preferredEditorManager) :
        base(Resources.About_Label, App.WmActivateApp, App.WmQuitApp, App.WmUninstallApp)
    {
        _hostsFile = hostsFile;
        _settings = settings;
        _autoStartManager = autoStartManager;
        _uninstaller = uninstaller;
        _appVersion = appVersion;
        _preferredEditorManager = preferredEditorManager;

        InitializeComponent();

        SysAboutMenuClicked += OnSysAboutMenuClicked;
        CustomMessageReceived += OnCustomMessageReceived;

        UxSetupPreferredEditors();
        UxRefreshPreferredEditor();
        UxFixButtonText();
        UxFixHeight();
        UxRefresh();
    }

    #region Ux

    private void UxSetupPreferredEditors()
    {
        if (uxOpenWith.Items.Count != 0)
        {
            return;
        }

        var preferredEditors = _preferredEditorManager.GetEditors();

        foreach (var editor in preferredEditors)
        {
            var preferredEditorMenuItem = new ToolStripMenuItem
            {
                Checked = editor.IsSelected,
                CheckState = (editor.IsSelected) ? CheckState.Checked : CheckState.Unchecked,
                Name = $"uxOpenWith_{editor.Key}",
                Size = new System.Drawing.Size(180, 22),
                Tag = editor.Key,
                Text = editor.DisplayName
            };

            preferredEditorMenuItem.Click += uxOpenWith_Click;

            uxOpenWith.Items.Add(preferredEditorMenuItem);
        }
    }

    private void UxRefreshPreferredEditor()
    {
        var currentPreferredEditor = _preferredEditorManager.GetSelectedEditorKey();

        foreach (ToolStripMenuItem menuItem in uxOpenWith.Items)
        {
            menuItem.Checked =
                string.Equals(menuItem.Tag as string, currentPreferredEditor,
                    StringComparison.InvariantCultureIgnoreCase);
        }
    }

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
            uxbtnEdit.Text = Resources.MainForm_UxFixButtonText_;
        }

        uxbtnEdit.Text = uxbtnEdit.Text.Replace("|", Environment.NewLine);
        uxbtnDisableHostsFile.Text = uxbtnDisableHostsFile.Text.Replace("|", Environment.NewLine);
        uxbtnEnableHostsFile.Text = uxbtnEnableHostsFile.Text.Replace("|", Environment.NewLine);
        uxbtnFlushDNS.Text = uxbtnFlushDNS.Text.Replace("|", Environment.NewLine);
    }

    private void UxRefresh()
    {
        uxlblEnabled.Text = Resources.hosts_file_is + (_hostsFile.IsEnabled()
            ? Resources.enabled + "."
            : Resources.disabled + ".");

        uxlblHostsFileSize.Text = _hostsFile
            .HostsFileSize()
            .ToString("##,###0") + Resources.bytes;

        uxlblHostsCount.Text =
            _hostsFile
                .HostsCount()
                .ToString("##,###0");

        var hostsEnabled = _hostsFile.IsEnabled();

        uxMenuEnableHostsFile.Checked = hostsEnabled;

        var hostOrHosts = _hostsFile
            .HostsCount() == 1
            ? Resources.host
            : Resources.hosts;

        uxNotifyIcon.Text = Consts.ApplicationName + Environment.NewLine + (hostsEnabled
            ? "(" + uxlblHostsCount.Text + " " + hostOrHosts + " " + Resources.enabled + ")"
            : Resources.all_hosts_disabled);

        var runAtStartupCurrentlyEnabled =
            _autoStartManager
                .IsAutoRunEnabled();

        uxMenuRunAtStartup.Checked = runAtStartupCurrentlyEnabled;

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
            if (!Text.Contains(Resources.MainForm_UxRefresh___Administrator_.Trim()))
            {
                Text += Resources.MainForm_UxRefresh___Administrator_;
            }
        }
        else
        {
            // Not elevated, disable all except Run As Admin
            uxbtnDisableHostsFile.Enabled =
                uxbtnEnableHostsFile.Enabled =
                    uxMenuEnableHostsFile.Enabled =
                        uxbtnFlushDNS.Enabled = false;

            // When Not elevated "edit/open" should only be available when the
            // hosts file itself is enabled
            uxbtnEdit.Enabled = hostsEnabled;

            Elevated.AddShieldToButton(uxbtnRunAsAdmin);
        }
    }

    private void UxAbout()
    {
        _aboutShown = true;
        
        var aboutMessage =
            Resources.AboutMessage
                .Replace("{appVersion}", _appVersion.GetAppVersion())
                .Replace("{commitId}", _appVersion.GetCommitId())
                .Replace("{buildDate}", _appVersion.GetBuildDate());

        MessageBox.Show(
            this,
            aboutMessage,
            Resources.MainForm_About,
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        _aboutShown = false;
    }

    #endregion

    #region Button Events

    private void uxbtnRunAsAdmin_Click(object sender, EventArgs e)
    {
        Elevated.RestartElevated();
    }

    private void uxbtnDisableHostsFile_Click(object sender, EventArgs e)
    {
        _hostsFile.DisableHostsFile();

        UxRefresh();
    }

    private void uxbtnEnableHostsFile_Click(object sender, EventArgs e)
    {
        _hostsFile.EnableHostsFile();

        UxRefresh();
    }

    private void uxbtnEdit_Click(object sender, EventArgs e)
    {
        if (_preferredEditorManager.Open())
        {
            return;
        }

        // We failed to open with the selected editor (non default).
        // Reset to Default and try again...
        _preferredEditorManager.SaveSelectedEditor(
            _preferredEditorManager.GetDefaultEditorKey());
            
        UxRefreshPreferredEditor();

        uxbtnEdit_Click(sender, EventArgs.Empty);
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
                    Resources.MainForm_uxbtnFlushDNS_Click_Unknown_error_starting_process__ipconfig__flushdns__,
                    Resources.MainForm_uxbtnFlushDNS_Click_Flush_DNS,
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

    private void MainForm_Shown(object sender, EventArgs e)
    {
        ShowMessageOnFirstRun();
    }

    private void OnSysAboutMenuClicked(object? sender, EventArgs e)
    {
        uxMenuAbout_Click(null!, EventArgs.Empty);
    }

    private void OnCustomMessageReceived(object? sender, CustomMessageEventArgs e)
    {
        switch (e.Msg)
        {
            case App.WmActivateApp:
                DoShow(true);
                break;

            case App.WmQuitApp:
                uxMenuExit_Click(null!, EventArgs.Empty);
                break;

            case App.WmUninstallApp:
                {
                    _uninstaller.Uninstall();
                    uxMenuExit_Click(null!, EventArgs.Empty);
                    break;
                }
        }
    }

    #endregion

    #region Menu Events

    private void uxMenuItemShow_Click(object sender, EventArgs e)
    {
        DoShow();

        ShowMessageOnFirstRun();
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
            _hostsFile.EnableHostsFile();
        }
        else
        {
            _hostsFile.DisableHostsFile();
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

    private void uxTrayMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
    {
        UxRefresh();
    }

    private void uxOpenWith_Click(object? sender, EventArgs e)
    {
        foreach (ToolStripMenuItem menuItem in uxOpenWith.Items)
        {
            menuItem.Checked = false;
        }

        if (sender is not ToolStripMenuItem selectedOpenWith)
        {
            return;
        }

        selectedOpenWith.Checked = true;

        var preferredEditorKey = selectedOpenWith.Tag as string;
        
        if (!string.IsNullOrWhiteSpace(preferredEditorKey))
        {
            _preferredEditorManager.SaveSelectedEditor(preferredEditorKey);
        }
    }

    private void uxMenuRunAtStartup_Click(object sender, EventArgs e)
    {
        EnableRunAtStartup(!_autoStartManager.IsAutoRunEnabled());
    }

    #endregion

    #region Protected

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);

        var minimized =
            Environment.GetCommandLineArgs().Length > 1 &&
            Environment.GetCommandLineArgs()[1].Equals(
                Consts.MinArg, StringComparison.InvariantCultureIgnoreCase);

        if (!minimized)
        {
            ShowInTaskbar = true;

            return;
        }

        // Start minimized
        WindowState = FormWindowState.Minimized;
        Visible = false;
        ShowInTaskbar = false;
        Opacity = 0;
    }

    protected override void OnFormClosing(FormClosingEventArgs e)
    {
        // ReSharper disable once RedundantAssignment
        // ReSharper disable once ConvertToConstant.Local
        var isDebug = false;
#if DEBUG
        isDebug = true;
#endif

        if (e.CloseReason == CloseReason.WindowsShutDown ||
            e.CloseReason == CloseReason.TaskManagerClosing ||
            e.CloseReason == CloseReason.ApplicationExitCall ||
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

    #endregion

    #region Private

    private bool EnableRunAtStartup(bool enable)
    {
        if (enable)
        {
            _autoStartManager.SetupAutoRunAtStartup();
        }

        return _autoStartManager.EnableDisableAutoRun(enable);
    }

    private void DoShow(bool external = false)
    {
        // This is a trick to force the app back on top in some cases when
        // invoked via PostMessage/WndProc and our custom WmActivateApp
        // message

        if (external)
        {
            WindowState = FormWindowState.Minimized;
        }

        BringToFront();
        TopLevel = true;
        Activate();

        Visible =
            ShowIcon =
                ShowInTaskbar = true;

        Opacity = 100;
        WindowState = FormWindowState.Normal;
    }

    private void ShowMessageOnFirstRun()
    {
        if (WindowState != FormWindowState.Normal || Elevated.IsElevated())
        {
            return;
        }

        var appVersion = _appVersion.GetAppVersion();

        var firstRun =
            !_settings.GetValue(Consts.FirstRunShownForKey, string.Empty)
                .ToLowerInvariant()
                .Equals(appVersion);

        if (!firstRun)
        {
            return;
        }

        //
        // New install - Set Run at startup to true (default)
        //
        uxMenuRunAtStartup.Checked = EnableRunAtStartup(true);
        //..

        _settings.SetValue(Consts.FirstRunShownForKey, appVersion);

        MessageBox.Show(
            this,
            Resources.RunAtStartupMessage.Replace("{appVersion}", appVersion),
            Resources.MainForm_ShowMessageOnFirstRun_Welcome,
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    #endregion
}