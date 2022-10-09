using EM.HostsManager.App.Hosts;
using EM.HostsManager.App.Version;
using EM.HostsManager.App.Win32;
using Reg=EM.HostsManager.App.Registry.Registry;

namespace EM.HostsManager.App.UI;

using static User32;
using Process = System.Diagnostics.Process;

public partial class MainForm : Form
{
    private bool _aboutShown;
    private bool _requestingClose;

    private const int SysMenuAboutId = 0x1;

    private const int WmUser = 0x0400;
    public const int WmActivateApp = WmUser + 55;
    public const int WmQuitApp = WmUser + 56;

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
            .HostsCount() == 1 
            ? "host"
            : "hosts";
            
        uxNotifyIcon.Text = @"Hosts Manager" + Environment.NewLine + (hostsEnabled
            ? "(" + uxlblHostsCount.Text + " " + hostOrHosts + " enabled)"
            : "(all hosts disabled)");

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
            $@"Commit: { appVersion.GetCommitId() }{Environment.NewLine}" +
            $@"Date: { appVersion.GetBuildDate() }" +
            $@"{Environment.NewLine}{Environment.NewLine}" +
            @"Copyright © 2021-2022 Enda Mullally",
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

    [SuppressMessage("SonarCloud", "S4036", Justification = "Fixed command")]
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

    [SuppressMessage("SonarCloud", "S4036", Justification = "Fixed command")]
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
        DoShow();

        ShowMessageOnFirstRun();
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
            (Environment.GetCommandLineArgs()[1].ToLowerInvariant() == "/min");
                
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

    [SuppressMessage("SonarCloud", "S2589", Justification = "Using a compiler directive here so this will not always be false")]
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

    private void MainForm_Shown(object sender, EventArgs e)
    {
        ShowMessageOnFirstRun();
    }

    private void ShowMessageOnFirstRun()
    {
        const string firstRunRegPath = @"Software\Enda Mullally\Hosts Manager";
        const string firstRunRegKey = @"FirstRun";

        if (WindowState != FormWindowState.Normal)
        {
            return;
        }

        var firstRun =
            Reg.GetCurrentUserRegString(firstRunRegPath, firstRunRegKey, "false")
                .ToLowerInvariant().Equals("true");

        if (!firstRun)
        {
            return;
        }

        Reg.SetCurrentUserRegString(firstRunRegPath, firstRunRegKey, "false");

        var appVersion = new AppVersion(Assembly.GetExecutingAssembly());

        MessageBox.Show(
            this,
            $@"== Hosts Manager {appVersion.GetAppVersion()} ==" +
            $@"{Environment.NewLine}{Environment.NewLine}" +
            $@"Welcome!" +
            $@"{Environment.NewLine}{Environment.NewLine}" +
            @"Hosts Manager is a system tray application which will automatically minimize to your system tray when closed/minimized." +
            @$"{Environment.NewLine}{Environment.NewLine}Please note: By default, Hosts Manager will start (minimized) when you start Windows. You can disable auto start in Windows Task Manager (Startup apps). To exit Hosts Manager, right click the system tray icon and select Exit." +
            @$"{Environment.NewLine}{Environment.NewLine}Thank you for installing Hosts Manager. Enjoy!",
            @"Welcome",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);
    }
}