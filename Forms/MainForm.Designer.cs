
using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace HostsManager.Forms
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.uxbtnRunAsAdmin = new System.Windows.Forms.Button();
            this.uxNotifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.uxTrayMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.uxMenuItemShow = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMenuSeperator1 = new System.Windows.Forms.ToolStripSeparator();
            this.uxMenuEnableHostsFile = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMenuSeperator3 = new System.Windows.Forms.ToolStripSeparator();
            this.uxMenuAbout = new System.Windows.Forms.ToolStripMenuItem();
            this.uxMenuExit = new System.Windows.Forms.ToolStripMenuItem();
            this.uxlblSep = new System.Windows.Forms.Label();
            this.uxAppIcon = new System.Windows.Forms.PictureBox();
            this.uxlblEnabled = new System.Windows.Forms.Label();
            this.uxlblHostNames = new System.Windows.Forms.Label();
            this.uxlblHostsCount = new System.Windows.Forms.Label();
            this.uxlblHostsFileSize = new System.Windows.Forms.Label();
            this.uxlblFileSize = new System.Windows.Forms.Label();
            this.uxbtnDisableHostsFile = new System.Windows.Forms.Button();
            this.uxbtnEnableHostsFile = new System.Windows.Forms.Button();
            this.uxbtnEdit = new System.Windows.Forms.Button();
            this.uxbtnFlushDNS = new System.Windows.Forms.Button();
            this.uxTrayMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.uxAppIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // uxbtnRunAsAdmin
            // 
            this.uxbtnRunAsAdmin.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.uxbtnRunAsAdmin.BackColor = System.Drawing.SystemColors.Control;
            this.uxbtnRunAsAdmin.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uxbtnRunAsAdmin.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uxbtnRunAsAdmin.Location = new System.Drawing.Point(114, 277);
            this.uxbtnRunAsAdmin.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uxbtnRunAsAdmin.Name = "uxbtnRunAsAdmin";
            this.uxbtnRunAsAdmin.Size = new System.Drawing.Size(175, 34);
            this.uxbtnRunAsAdmin.TabIndex = 4;
            this.uxbtnRunAsAdmin.Text = " Run as &Administrator";
            this.uxbtnRunAsAdmin.UseCompatibleTextRendering = true;
            this.uxbtnRunAsAdmin.UseVisualStyleBackColor = false;
            this.uxbtnRunAsAdmin.Click += new System.EventHandler(this.uxbtnRunAsAdmin_Click);
            // 
            // uxNotifyIcon
            // 
            this.uxNotifyIcon.ContextMenuStrip = this.uxTrayMenu;
            this.uxNotifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("uxNotifyIcon.Icon")));
            this.uxNotifyIcon.Text = "Hosts Manager";
            this.uxNotifyIcon.Visible = true;
            this.uxNotifyIcon.DoubleClick += new System.EventHandler(this.uxMenuItemShow_Click);
            // 
            // uxTrayMenu
            // 
            this.uxTrayMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.uxMenuItemShow,
            this.uxMenuSeperator1,
            this.uxMenuEnableHostsFile,
            this.uxMenuSeperator3,
            this.uxMenuAbout,
            this.uxMenuExit});
            this.uxTrayMenu.Name = "uxTrayMenu";
            this.uxTrayMenu.Size = new System.Drawing.Size(191, 104);
            // 
            // uxMenuItemShow
            // 
            this.uxMenuItemShow.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.uxMenuItemShow.Name = "uxMenuItemShow";
            this.uxMenuItemShow.Size = new System.Drawing.Size(190, 22);
            this.uxMenuItemShow.Text = "&Open";
            this.uxMenuItemShow.Click += new System.EventHandler(this.uxMenuItemShow_Click);
            // 
            // uxMenuSeperator1
            // 
            this.uxMenuSeperator1.Name = "uxMenuSeperator1";
            this.uxMenuSeperator1.Size = new System.Drawing.Size(187, 6);
            // 
            // uxMenuEnableHostsFile
            // 
            this.uxMenuEnableHostsFile.Checked = true;
            this.uxMenuEnableHostsFile.CheckOnClick = true;
            this.uxMenuEnableHostsFile.CheckState = System.Windows.Forms.CheckState.Checked;
            this.uxMenuEnableHostsFile.Name = "uxMenuEnableHostsFile";
            this.uxMenuEnableHostsFile.Size = new System.Drawing.Size(190, 22);
            this.uxMenuEnableHostsFile.Text = "Enable Hosts File";
            this.uxMenuEnableHostsFile.Click += new System.EventHandler(this.uxMenuEnableHostsFile_Click);
            // 
            // uxMenuSeperator3
            // 
            this.uxMenuSeperator3.Name = "uxMenuSeperator3";
            this.uxMenuSeperator3.Size = new System.Drawing.Size(187, 6);
            // 
            // uxMenuAbout
            // 
            this.uxMenuAbout.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uxMenuAbout.Name = "uxMenuAbout";
            this.uxMenuAbout.Size = new System.Drawing.Size(190, 22);
            this.uxMenuAbout.Text = "&About Hosts Manager";
            this.uxMenuAbout.Click += new System.EventHandler(this.uxMenuAbout_Click);
            // 
            // uxMenuExit
            // 
            this.uxMenuExit.Name = "uxMenuExit";
            this.uxMenuExit.Size = new System.Drawing.Size(190, 22);
            this.uxMenuExit.Text = "E&xit";
            this.uxMenuExit.Click += new System.EventHandler(this.uxMenuExit_Click);
            // 
            // uxlblSep
            // 
            this.uxlblSep.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.uxlblSep.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.uxlblSep.Location = new System.Drawing.Point(0, 261);
            this.uxlblSep.Name = "uxlblSep";
            this.uxlblSep.Size = new System.Drawing.Size(516, 3);
            this.uxlblSep.TabIndex = 2;
            // 
            // uxAppIcon
            // 
            this.uxAppIcon.Image = ((System.Drawing.Image)(resources.GetObject("uxAppIcon.Image")));
            this.uxAppIcon.Location = new System.Drawing.Point(12, 15);
            this.uxAppIcon.Name = "uxAppIcon";
            this.uxAppIcon.Size = new System.Drawing.Size(48, 48);
            this.uxAppIcon.TabIndex = 3;
            this.uxAppIcon.TabStop = false;
            // 
            // uxlblEnabled
            // 
            this.uxlblEnabled.AutoSize = true;
            this.uxlblEnabled.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.uxlblEnabled.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.uxlblEnabled.Location = new System.Drawing.Point(86, 14);
            this.uxlblEnabled.Name = "uxlblEnabled";
            this.uxlblEnabled.Size = new System.Drawing.Size(188, 25);
            this.uxlblEnabled.TabIndex = 4;
            this.uxlblEnabled.Text = "Hosts file is [status].";
            // 
            // uxlblHostNames
            // 
            this.uxlblHostNames.AutoSize = true;
            this.uxlblHostNames.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.uxlblHostNames.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.uxlblHostNames.Location = new System.Drawing.Point(89, 54);
            this.uxlblHostNames.Name = "uxlblHostNames";
            this.uxlblHostNames.Size = new System.Drawing.Size(52, 15);
            this.uxlblHostNames.TabIndex = 5;
            this.uxlblHostNames.Text = "Host(s) :";
            // 
            // uxlblHostsCount
            // 
            this.uxlblHostsCount.AutoSize = true;
            this.uxlblHostsCount.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.uxlblHostsCount.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.uxlblHostsCount.Location = new System.Drawing.Point(184, 54);
            this.uxlblHostsCount.Name = "uxlblHostsCount";
            this.uxlblHostsCount.Size = new System.Drawing.Size(105, 15);
            this.uxlblHostsCount.TabIndex = 6;
            this.uxlblHostsCount.Text = "[hostname count]";
            // 
            // uxlblHostsFileSize
            // 
            this.uxlblHostsFileSize.AutoSize = true;
            this.uxlblHostsFileSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.uxlblHostsFileSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.uxlblHostsFileSize.Location = new System.Drawing.Point(184, 78);
            this.uxlblHostsFileSize.Name = "uxlblHostsFileSize";
            this.uxlblHostsFileSize.Size = new System.Drawing.Size(63, 15);
            this.uxlblHostsFileSize.TabIndex = 8;
            this.uxlblHostsFileSize.Text = "[0 byte(s)]";
            // 
            // uxlblFileSize
            // 
            this.uxlblFileSize.AutoSize = true;
            this.uxlblFileSize.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.uxlblFileSize.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.uxlblFileSize.Location = new System.Drawing.Point(89, 78);
            this.uxlblFileSize.Name = "uxlblFileSize";
            this.uxlblFileSize.Size = new System.Drawing.Size(53, 15);
            this.uxlblFileSize.TabIndex = 7;
            this.uxlblFileSize.Text = "File size:";
            // 
            // uxbtnDisableHostsFile
            // 
            this.uxbtnDisableHostsFile.BackColor = System.Drawing.SystemColors.Control;
            this.uxbtnDisableHostsFile.Enabled = false;
            this.uxbtnDisableHostsFile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uxbtnDisableHostsFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uxbtnDisableHostsFile.Location = new System.Drawing.Point(124, 135);
            this.uxbtnDisableHostsFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uxbtnDisableHostsFile.Name = "uxbtnDisableHostsFile";
            this.uxbtnDisableHostsFile.Size = new System.Drawing.Size(75, 75);
            this.uxbtnDisableHostsFile.TabIndex = 1;
            this.uxbtnDisableHostsFile.Text = "|&Disable Hosts File";
            this.uxbtnDisableHostsFile.UseCompatibleTextRendering = true;
            this.uxbtnDisableHostsFile.UseVisualStyleBackColor = false;
            this.uxbtnDisableHostsFile.Click += new System.EventHandler(this.uxbtnDisableHostsFile_Click);
            // 
            // uxbtnEnableHostsFile
            // 
            this.uxbtnEnableHostsFile.BackColor = System.Drawing.SystemColors.Control;
            this.uxbtnEnableHostsFile.Enabled = false;
            this.uxbtnEnableHostsFile.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uxbtnEnableHostsFile.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uxbtnEnableHostsFile.Location = new System.Drawing.Point(205, 135);
            this.uxbtnEnableHostsFile.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uxbtnEnableHostsFile.Name = "uxbtnEnableHostsFile";
            this.uxbtnEnableHostsFile.Size = new System.Drawing.Size(75, 75);
            this.uxbtnEnableHostsFile.TabIndex = 2;
            this.uxbtnEnableHostsFile.Text = "|&Ensable Hosts File";
            this.uxbtnEnableHostsFile.UseCompatibleTextRendering = true;
            this.uxbtnEnableHostsFile.UseVisualStyleBackColor = false;
            this.uxbtnEnableHostsFile.Click += new System.EventHandler(this.uxbtnEnableHostsFile_Click);
            // 
            // uxbtnEdit
            // 
            this.uxbtnEdit.BackColor = System.Drawing.SystemColors.Control;
            this.uxbtnEdit.Enabled = false;
            this.uxbtnEdit.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uxbtnEdit.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uxbtnEdit.Location = new System.Drawing.Point(43, 135);
            this.uxbtnEdit.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uxbtnEdit.Name = "uxbtnEdit";
            this.uxbtnEdit.Size = new System.Drawing.Size(75, 75);
            this.uxbtnEdit.TabIndex = 0;
            this.uxbtnEdit.Text = "|Edit &Hosts File";
            this.uxbtnEdit.UseCompatibleTextRendering = true;
            this.uxbtnEdit.UseVisualStyleBackColor = false;
            this.uxbtnEdit.Click += new System.EventHandler(this.uxbtnEdit_Click);
            // 
            // uxbtnFlushDNS
            // 
            this.uxbtnFlushDNS.BackColor = System.Drawing.SystemColors.Control;
            this.uxbtnFlushDNS.Enabled = false;
            this.uxbtnFlushDNS.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.uxbtnFlushDNS.ForeColor = System.Drawing.SystemColors.ControlText;
            this.uxbtnFlushDNS.Location = new System.Drawing.Point(286, 135);
            this.uxbtnFlushDNS.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.uxbtnFlushDNS.Name = "uxbtnFlushDNS";
            this.uxbtnFlushDNS.Size = new System.Drawing.Size(75, 75);
            this.uxbtnFlushDNS.TabIndex = 3;
            this.uxbtnFlushDNS.Text = "|&Flush DNS Cache";
            this.uxbtnFlushDNS.UseCompatibleTextRendering = true;
            this.uxbtnFlushDNS.UseVisualStyleBackColor = false;
            this.uxbtnFlushDNS.Click += new System.EventHandler(this.uxbtnFlushDNS_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ClientSize = new System.Drawing.Size(404, 324);
            this.Controls.Add(this.uxbtnFlushDNS);
            this.Controls.Add(this.uxbtnEdit);
            this.Controls.Add(this.uxbtnEnableHostsFile);
            this.Controls.Add(this.uxbtnDisableHostsFile);
            this.Controls.Add(this.uxlblHostsFileSize);
            this.Controls.Add(this.uxlblFileSize);
            this.Controls.Add(this.uxlblHostsCount);
            this.Controls.Add(this.uxlblHostNames);
            this.Controls.Add(this.uxlblEnabled);
            this.Controls.Add(this.uxAppIcon);
            this.Controls.Add(this.uxlblSep);
            this.Controls.Add(this.uxbtnRunAsAdmin);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Hosts Manager";
            this.Activated += new System.EventHandler(this.MainForm_Activated);
            this.Resize += new System.EventHandler(this.MainForm_Resize);
            this.uxTrayMenu.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.uxAppIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
        private Button uxbtnDisableHostsFile;
        private Button uxbtnEnableHostsFile;
        private Button uxbtnEdit;
        private ContextMenuStrip uxTrayMenu;
        private ToolStripMenuItem uxMenuItemShow;
        private ToolStripMenuItem uxMenuAbout;
        private ToolStripMenuItem uxMenuExit;
        private ToolStripMenuItem uxMenuEnableHostsFile;
        private ToolStripSeparator uxMenuSeperator3;
        private ToolStripSeparator uxMenuSeperator1;
        private Button uxbtnFlushDNS;
    }
}