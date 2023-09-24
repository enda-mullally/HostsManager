//
// Copyright © 2021-2023 Enda Mullally.
//

using EM.HostsManager.Infrastructure.Hosts;
using EM.HostsManager.Infrastructure.Version;
using EM.HostsManager.Infrastructure.Win32;
using Reg = EM.HostsManager.Infrastructure.Registry.Registry;

#pragma warning disable IDE1006

// ReSharper disable IdentifierTypo
// ReSharper disable InconsistentNaming

namespace EM.HostsManager.App.UI;

using static User32;

public partial class MainForm : Form
{
    private bool _aboutShown;
    private bool _requestingClose;

    private const int SysMenuAboutId = 0x1;
    private const int WmUser = 0x0400;
    public const int WmActivateApp = WmUser + 55;
    public const int WmQuitApp = WmUser + 56;

    private enum PreferredEditor { Default, NotepadPP, VSCode }

    public MainForm()
    {
        InitializeComponent();

        UxGetPreferredEditor();
        UxFixButtonText();
        UxFixHeight();
        UxRefresh();
    }

    #region Ux

    private void UxGetPreferredEditor()
    {
        uxOpenWithDefault.Tag = nameof(PreferredEditor.Default);
        uxOpenWithNotepadpp.Tag = nameof(PreferredEditor.NotepadPP);
        uxOpenWithVSCode.Tag = nameof(PreferredEditor.VSCode);

        var currentPreferredEditor = Reg.GetRegString(
            Microsoft.Win32.Registry.CurrentUser,
            Consts.AppRegPath,
            Consts.PreferredEditorKey,
            nameof(PreferredEditor.Default));

        foreach (ToolStripMenuItem menuItem in uxOpenWith.Items)
        {
            menuItem.Checked =
                string.Equals(menuItem.Tag as string, currentPreferredEditor, StringComparison.InvariantCultureIgnoreCase);
        }

        // Write it again anyway, if it was defaulted above.
        Reg.SetRegString(
            Microsoft.Win32.Registry.CurrentUser,
            Consts.AppRegPath,
            Consts.PreferredEditorKey,
            currentPreferredEditor);
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
            uxbtnEdit.Text = @"| Open &hosts file";
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
            .HostsCount() == 1
            ? "host"
            : "hosts";

        uxNotifyIcon.Text = @"Hosts Manager" + Environment.NewLine + (hostsEnabled
            ? "(" + uxlblHostsCount.Text + " " + hostOrHosts + " enabled)"
            : "(all hosts disabled)");

        var runAtStartupCurrentlyEnabled =
            !string.IsNullOrWhiteSpace(Reg.GetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                Consts.RunAtStartupRegPath,
                Consts.RunAtStartupKey,
                string.Empty).Trim());

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

        var aboutMessage =
            Consts.AboutMessage
                .Replace("{appVersion}", appVersion.GetAppVersion())
                .Replace("{commitId}", appVersion.GetCommitId())
                .Replace("{buildDate}", appVersion.GetBuildDate());

        MessageBox.Show(
            this,
            aboutMessage,
            @"About",
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

        var editor = Enum.Parse<PreferredEditor>(
            Reg.GetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                Consts.AppRegPath,
                Consts.PreferredEditorKey,
                nameof(PreferredEditor.Default)));

        var fileName = editor switch
        {
            PreferredEditor.Default => "notepad.exe",
            PreferredEditor.NotepadPP => "notepad++",
            PreferredEditor.VSCode => "code",
            _ => "notepad.exe"
        };

        var startInfo = new ProcessStartInfo
        {
            UseShellExecute = true,
            WorkingDirectory = workingDirectory,
            FileName = fileName,
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

            if (editor == PreferredEditor.Default)
            {
                return;
            }

            // We failed to open with the selected editor (non default) so reset to Default and try again...

            Reg.SetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                Consts.AppRegPath,
                Consts.PreferredEditorKey,
                nameof(PreferredEditor.Default));

            UxGetPreferredEditor();

            uxbtnEdit_Click(sender, EventArgs.Empty);
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

    private void MainForm_Shown(object sender, EventArgs e)
    {
        ShowMessageOnFirstRun();
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
        base.OnLoad(e);

        var minimized =
            Environment.GetCommandLineArgs().Length > 1 &&
            Environment.GetCommandLineArgs()[1].Equals(Consts.MinArg, StringComparison.InvariantCultureIgnoreCase);

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

        if (Elevated.IsElevated())
        {
            // UIPI bypass for our custom messages if the app is elevated.
            User32.ChangeWindowMessageFilterEx(Handle,
                WmActivateApp,
                ChangeWindowMessageFilterExAction.Allow);

            User32.ChangeWindowMessageFilterEx(Handle,
                WmQuitApp,
                ChangeWindowMessageFilterExAction.Allow);
        }
    }

    protected override void WndProc(ref Message m)
    {
        base.WndProc(ref m);

        switch (m.Msg)
        {
            case User32.WmSysCommand when (int)m.WParam == SysMenuAboutId:
                uxMenuAbout_Click(null!, EventArgs.Empty);
                break;

            case WmActivateApp:
                DoShow(true);
                break;

            case WmQuitApp:
                uxMenuExit_Click(null!, EventArgs.Empty);
                break;
        }
    }

    #endregion

    #region Private

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

        var appVersion = new AppVersion(Assembly.GetExecutingAssembly()).GetAppVersion();

        var firstRun =
            !Reg.GetRegString(
                    Microsoft.Win32.Registry.CurrentUser,
                    Consts.AppRegPath,
                    Consts.FirstRunShownForKey,
                    string.Empty)
                .ToLowerInvariant().Equals(appVersion);

        if (!firstRun)
        {
            return;
        }

        Reg.SetRegString(
            Microsoft.Win32.Registry.CurrentUser,
            Consts.AppRegPath,
            Consts.FirstRunShownForKey,
            appVersion);

        MessageBox.Show(
            this,
            Consts.RunAtStartupMessage.Replace("{appVersion}", appVersion),
            @"Welcome",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }

    private void uxOpenWith_Click(object sender, EventArgs e)
    {
        foreach (ToolStripMenuItem menuItem in uxOpenWith.Items)
        {
            menuItem.Checked = false;
        }

        var selectedOpenWith = (ToolStripMenuItem)sender;

        selectedOpenWith.Checked = true;

        var preferredEditor = selectedOpenWith.Tag as string;

        Reg.SetRegString(
            Microsoft.Win32.Registry.CurrentUser,
            Consts.AppRegPath,
            Consts.PreferredEditorKey,
            preferredEditor ?? nameof(PreferredEditor.Default));
    }

    private void uxMenuRunAtStartup_Click(object sender, EventArgs e)
    {
        var currentlyEnabled =
            !string.IsNullOrWhiteSpace(Reg.GetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                Consts.RunAtStartupRegPath,
                Consts.RunAtStartupKey,
                string.Empty).Trim());

        if (currentlyEnabled)
        {
             Reg.DeleteRegString(
                Microsoft.Win32.Registry.CurrentUser,
                Consts.RunAtStartupRegPath,
                Consts.RunAtStartupKey);
        }
        else
        {
            var exe = Application
                .ExecutablePath
                .Replace(".dll", ".exe", StringComparison.InvariantCultureIgnoreCase);

            const string doubleQuote = "\"";

            Reg.SetRegString(
                Microsoft.Win32.Registry.CurrentUser,
                Consts.RunAtStartupRegPath,
                Consts.RunAtStartupKey,
                $"{doubleQuote}{exe}{doubleQuote} /min");
        }
    }

    #endregion

}