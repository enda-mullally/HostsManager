using System.ComponentModel;
using EM.HostsManager.App.UI.Controls;

namespace EM.HostsManager.App.UI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new Container();
            ComponentResourceManager resources = new ComponentResourceManager(typeof(MainForm));
            uxbtnRunAsAdmin = new Button();
            uxNotifyIcon = new NotifyIcon(components);
            uxTrayMenu = new ContextMenuStrip(components);
            uxMenuItemShow = new ToolStripMenuItem();
            uxMenuSeperator1 = new ToolStripSeparator();
            uxMenuEnableHostsFile = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            uxMenuAutoStart = new ToolStripMenuItem();
            uxMenuRunAtStartup = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            uxMenuAbout = new ToolStripMenuItem();
            toolStripSeparator2 = new ToolStripSeparator();
            uxMenuExit = new ToolStripMenuItem();
            uxlblSep = new Label();
            uxAppIcon = new PictureBox();
            uxlblEnabled = new Label();
            uxlblHostNames = new Label();
            uxlblHostsCount = new Label();
            uxlblHostsFileSize = new Label();
            uxlblFileSize = new Label();
            uxbtnDisableHostsFile = new SplitButton();
            uxbtnEnableHostsFile = new SplitButton();
            uxbtnEdit = new SplitButton();
            uxOpenWith = new ContextMenuStrip(components);
            uxbtnFlushDNS = new SplitButton();
            uxTrayMenu.SuspendLayout();
            ((ISupportInitialize)uxAppIcon).BeginInit();
            SuspendLayout();
            // 
            // uxbtnRunAsAdmin
            // 
            uxbtnRunAsAdmin.Anchor = AnchorStyles.Left;
            uxbtnRunAsAdmin.BackColor = System.Drawing.SystemColors.Control;
            uxbtnRunAsAdmin.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            uxbtnRunAsAdmin.ForeColor = System.Drawing.SystemColors.ControlText;
            uxbtnRunAsAdmin.Location = new System.Drawing.Point(114, 277);
            uxbtnRunAsAdmin.Margin = new Padding(3, 2, 3, 2);
            uxbtnRunAsAdmin.Name = "uxbtnRunAsAdmin";
            uxbtnRunAsAdmin.Size = new System.Drawing.Size(175, 34);
            uxbtnRunAsAdmin.TabIndex = 10;
            uxbtnRunAsAdmin.Text = " Run as &Administrator";
            uxbtnRunAsAdmin.UseCompatibleTextRendering = true;
            uxbtnRunAsAdmin.UseVisualStyleBackColor = false;
            uxbtnRunAsAdmin.Click += uxbtnRunAsAdmin_Click;
            // 
            // uxNotifyIcon
            // 
            uxNotifyIcon.ContextMenuStrip = uxTrayMenu;
            uxNotifyIcon.Icon = (System.Drawing.Icon)resources.GetObject("uxNotifyIcon.Icon");
            uxNotifyIcon.Text = "Hosts Manager";
            uxNotifyIcon.Visible = true;
            uxNotifyIcon.DoubleClick += uxMenuItemShow_Click;
            // 
            // uxTrayMenu
            // 
            uxTrayMenu.Items.AddRange(new ToolStripItem[] { uxMenuItemShow, uxMenuSeperator1, uxMenuEnableHostsFile, toolStripSeparator1, uxMenuAutoStart, toolStripSeparator2, uxMenuExit });
            uxTrayMenu.Name = "uxTrayMenu";
            uxTrayMenu.Size = new System.Drawing.Size(160, 110);
            uxTrayMenu.Opening += uxTrayMenu_Opening;
            // 
            // uxMenuItemShow
            // 
            uxMenuItemShow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            uxMenuItemShow.Name = "uxMenuItemShow";
            uxMenuItemShow.Size = new System.Drawing.Size(159, 22);
            uxMenuItemShow.Text = "&Open";
            uxMenuItemShow.Click += uxMenuItemShow_Click;
            // 
            // uxMenuSeperator1
            // 
            uxMenuSeperator1.Name = "uxMenuSeperator1";
            uxMenuSeperator1.Size = new System.Drawing.Size(156, 6);
            // 
            // uxMenuEnableHostsFile
            // 
            uxMenuEnableHostsFile.Checked = true;
            uxMenuEnableHostsFile.CheckOnClick = true;
            uxMenuEnableHostsFile.CheckState = CheckState.Checked;
            uxMenuEnableHostsFile.Name = "uxMenuEnableHostsFile";
            uxMenuEnableHostsFile.Size = new System.Drawing.Size(159, 22);
            uxMenuEnableHostsFile.Text = "Enable hosts file";
            uxMenuEnableHostsFile.Click += uxMenuEnableHostsFile_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(156, 6);
            // 
            // uxMenuAutoStart
            // 
            uxMenuAutoStart.DropDownItems.AddRange(new ToolStripItem[] { uxMenuRunAtStartup, toolStripMenuItem1, uxMenuAbout });
            uxMenuAutoStart.Name = "uxMenuAutoStart";
            uxMenuAutoStart.Size = new System.Drawing.Size(159, 22);
            uxMenuAutoStart.Text = "Application";
            // 
            // uxMenuRunAtStartup
            // 
            uxMenuRunAtStartup.Checked = true;
            uxMenuRunAtStartup.CheckOnClick = true;
            uxMenuRunAtStartup.CheckState = CheckState.Checked;
            uxMenuRunAtStartup.Name = "uxMenuRunAtStartup";
            uxMenuRunAtStartup.Size = new System.Drawing.Size(190, 22);
            uxMenuRunAtStartup.Text = "&Run at startup";
            uxMenuRunAtStartup.Click += uxMenuRunAtStartup_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new System.Drawing.Size(187, 6);
            // 
            // uxMenuAbout
            // 
            uxMenuAbout.Font = new System.Drawing.Font("Segoe UI", 9F);
            uxMenuAbout.Name = "uxMenuAbout";
            uxMenuAbout.Size = new System.Drawing.Size(190, 22);
            uxMenuAbout.Text = "&About Hosts Manager";
            uxMenuAbout.Click += uxMenuAbout_Click;
            // 
            // toolStripSeparator2
            // 
            toolStripSeparator2.Name = "toolStripSeparator2";
            toolStripSeparator2.Size = new System.Drawing.Size(156, 6);
            // 
            // uxMenuExit
            // 
            uxMenuExit.Name = "uxMenuExit";
            uxMenuExit.Size = new System.Drawing.Size(159, 22);
            uxMenuExit.Text = "E&xit";
            uxMenuExit.Click += uxMenuExit_Click;
            // 
            // uxlblSep
            // 
            uxlblSep.Anchor = AnchorStyles.Left;
            uxlblSep.BorderStyle = BorderStyle.Fixed3D;
            uxlblSep.Location = new System.Drawing.Point(0, 261);
            uxlblSep.Name = "uxlblSep";
            uxlblSep.Size = new System.Drawing.Size(516, 3);
            uxlblSep.TabIndex = 9;
            // 
            // uxAppIcon
            // 
            uxAppIcon.Image = (System.Drawing.Image)resources.GetObject("uxAppIcon.Image");
            uxAppIcon.Location = new System.Drawing.Point(12, 15);
            uxAppIcon.Name = "uxAppIcon";
            uxAppIcon.Size = new System.Drawing.Size(48, 48);
            uxAppIcon.TabIndex = 3;
            uxAppIcon.TabStop = false;
            // 
            // uxlblEnabled
            // 
            uxlblEnabled.AutoSize = true;
            uxlblEnabled.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            uxlblEnabled.ForeColor = System.Drawing.Color.WhiteSmoke;
            uxlblEnabled.Location = new System.Drawing.Point(103, 14);
            uxlblEnabled.Name = "uxlblEnabled";
            uxlblEnabled.Size = new System.Drawing.Size(188, 25);
            uxlblEnabled.TabIndex = 0;
            uxlblEnabled.Text = "Hosts file is [status].";
            // 
            // uxlblHostNames
            // 
            uxlblHostNames.AutoSize = true;
            uxlblHostNames.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            uxlblHostNames.ForeColor = System.Drawing.Color.WhiteSmoke;
            uxlblHostNames.Location = new System.Drawing.Point(106, 54);
            uxlblHostNames.Name = "uxlblHostNames";
            uxlblHostNames.Size = new System.Drawing.Size(41, 15);
            uxlblHostNames.TabIndex = 1;
            uxlblHostNames.Text = "Hosts:";
            // 
            // uxlblHostsCount
            // 
            uxlblHostsCount.AutoSize = true;
            uxlblHostsCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            uxlblHostsCount.ForeColor = System.Drawing.Color.WhiteSmoke;
            uxlblHostsCount.Location = new System.Drawing.Point(201, 54);
            uxlblHostsCount.Name = "uxlblHostsCount";
            uxlblHostsCount.Size = new System.Drawing.Size(105, 15);
            uxlblHostsCount.TabIndex = 2;
            uxlblHostsCount.Text = "[hostname count]";
            // 
            // uxlblHostsFileSize
            // 
            uxlblHostsFileSize.AutoSize = true;
            uxlblHostsFileSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            uxlblHostsFileSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            uxlblHostsFileSize.Location = new System.Drawing.Point(201, 78);
            uxlblHostsFileSize.Name = "uxlblHostsFileSize";
            uxlblHostsFileSize.Size = new System.Drawing.Size(63, 15);
            uxlblHostsFileSize.TabIndex = 4;
            uxlblHostsFileSize.Text = "[0 byte(s)]";
            // 
            // uxlblFileSize
            // 
            uxlblFileSize.AutoSize = true;
            uxlblFileSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            uxlblFileSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            uxlblFileSize.Location = new System.Drawing.Point(106, 78);
            uxlblFileSize.Name = "uxlblFileSize";
            uxlblFileSize.Size = new System.Drawing.Size(53, 15);
            uxlblFileSize.TabIndex = 3;
            uxlblFileSize.Text = "File size:";
            // 
            // uxbtnDisableHostsFile
            // 
            uxbtnDisableHostsFile.BackColor = System.Drawing.SystemColors.Control;
            uxbtnDisableHostsFile.Enabled = false;
            uxbtnDisableHostsFile.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            uxbtnDisableHostsFile.ForeColor = System.Drawing.SystemColors.ControlText;
            uxbtnDisableHostsFile.Location = new System.Drawing.Point(132, 135);
            uxbtnDisableHostsFile.Margin = new Padding(3, 2, 3, 2);
            uxbtnDisableHostsFile.Name = "uxbtnDisableHostsFile";
            uxbtnDisableHostsFile.Size = new System.Drawing.Size(75, 75);
            uxbtnDisableHostsFile.TabIndex = 6;
            uxbtnDisableHostsFile.Text = "|&Disable hosts file";
            uxbtnDisableHostsFile.UseCompatibleTextRendering = true;
            uxbtnDisableHostsFile.UseVisualStyleBackColor = false;
            uxbtnDisableHostsFile.Click += uxbtnDisableHostsFile_Click;
            // 
            // uxbtnEnableHostsFile
            // 
            uxbtnEnableHostsFile.BackColor = System.Drawing.SystemColors.Control;
            uxbtnEnableHostsFile.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            uxbtnEnableHostsFile.ForeColor = System.Drawing.SystemColors.ControlText;
            uxbtnEnableHostsFile.Location = new System.Drawing.Point(214, 135);
            uxbtnEnableHostsFile.Margin = new Padding(3, 2, 3, 2);
            uxbtnEnableHostsFile.Name = "uxbtnEnableHostsFile";
            uxbtnEnableHostsFile.Size = new System.Drawing.Size(75, 75);
            uxbtnEnableHostsFile.TabIndex = 7;
            uxbtnEnableHostsFile.Text = "|&Enable hosts file";
            uxbtnEnableHostsFile.UseCompatibleTextRendering = true;
            uxbtnEnableHostsFile.UseVisualStyleBackColor = false;
            uxbtnEnableHostsFile.Click += uxbtnEnableHostsFile_Click;
            // 
            // uxbtnEdit
            // 
            uxbtnEdit.BackColor = System.Drawing.SystemColors.Control;
            uxbtnEdit.Enabled = false;
            uxbtnEdit.FlatAppearance.BorderSize = 0;
            uxbtnEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            uxbtnEdit.ForeColor = System.Drawing.SystemColors.ControlText;
            uxbtnEdit.Location = new System.Drawing.Point(32, 135);
            uxbtnEdit.Margin = new Padding(3, 2, 3, 2);
            uxbtnEdit.Name = "uxbtnEdit";
            uxbtnEdit.Size = new System.Drawing.Size(93, 75);
            uxbtnEdit.SplitMenu = uxOpenWith;
            uxbtnEdit.SplitMenuStrip = uxOpenWith;
            uxbtnEdit.TabIndex = 5;
            uxbtnEdit.Text = "| Edit &hosts file ";
            uxbtnEdit.UseCompatibleTextRendering = true;
            uxbtnEdit.UseVisualStyleBackColor = false;
            uxbtnEdit.Click += uxbtnEdit_Click;
            // 
            // uxOpenWith
            // 
            uxOpenWith.Name = "uxTrayMenu";
            uxOpenWith.Size = new System.Drawing.Size(61, 4);
            // 
            // uxbtnFlushDNS
            // 
            uxbtnFlushDNS.BackColor = System.Drawing.SystemColors.Control;
            uxbtnFlushDNS.Enabled = false;
            uxbtnFlushDNS.Font = new System.Drawing.Font("Segoe UI", 9.75F);
            uxbtnFlushDNS.ForeColor = System.Drawing.SystemColors.ControlText;
            uxbtnFlushDNS.Location = new System.Drawing.Point(297, 135);
            uxbtnFlushDNS.Margin = new Padding(3, 2, 3, 2);
            uxbtnFlushDNS.Name = "uxbtnFlushDNS";
            uxbtnFlushDNS.Size = new System.Drawing.Size(75, 75);
            uxbtnFlushDNS.TabIndex = 8;
            uxbtnFlushDNS.Text = "|&Flush DNS cache";
            uxbtnFlushDNS.UseCompatibleTextRendering = true;
            uxbtnFlushDNS.UseVisualStyleBackColor = false;
            uxbtnFlushDNS.Click += uxbtnFlushDNS_Click;
            // 
            // MainForm
            // 
            AutoScaleMode = AutoScaleMode.None;
            BackColor = System.Drawing.Color.FromArgb(64, 64, 64);
            ClientSize = new System.Drawing.Size(404, 324);
            Controls.Add(uxbtnFlushDNS);
            Controls.Add(uxbtnEdit);
            Controls.Add(uxbtnEnableHostsFile);
            Controls.Add(uxbtnDisableHostsFile);
            Controls.Add(uxlblHostsFileSize);
            Controls.Add(uxlblFileSize);
            Controls.Add(uxlblHostsCount);
            Controls.Add(uxlblHostNames);
            Controls.Add(uxlblEnabled);
            Controls.Add(uxAppIcon);
            Controls.Add(uxlblSep);
            Controls.Add(uxbtnRunAsAdmin);
            Font = new System.Drawing.Font("Segoe UI", 8.25F);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "MainForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Hosts Manager";
            Activated += MainForm_Activated;
            Shown += MainForm_Shown;
            Resize += MainForm_Resize;
            uxTrayMenu.ResumeLayout(false);
            ((ISupportInitialize)uxAppIcon).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button uxbtnRunAsAdmin;
        private NotifyIcon uxNotifyIcon;
        private Label uxlblSep;
        private PictureBox uxAppIcon;
        private Label uxlblEnabled;
        private Label uxlblHostNames;
        private Label uxlblHostsCount;
        private Label uxlblHostsFileSize;
        private Label uxlblFileSize;
        private SplitButton uxbtnDisableHostsFile;
        private SplitButton uxbtnEnableHostsFile;
        private SplitButton uxbtnEdit;
        private ContextMenuStrip uxTrayMenu;
        private ToolStripMenuItem uxMenuItemShow;
        private ToolStripMenuItem uxMenuExit;
        private ToolStripMenuItem uxMenuEnableHostsFile;
        private ToolStripSeparator uxMenuSeperator1;
        private SplitButton uxbtnFlushDNS;
        private ContextMenuStrip uxOpenWith;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem uxMenuAutoStart;
        private ToolStripSeparator toolStripSeparator2;
        private ToolStripMenuItem uxMenuRunAtStartup;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem uxMenuAbout;
    }
}