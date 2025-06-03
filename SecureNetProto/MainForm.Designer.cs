namespace SecureNetProto
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            // Instantiate panels and controls
            this.panelStartup = new System.Windows.Forms.Panel();
            this.lblLogo = new System.Windows.Forms.Label();
            this.lblEnterUsername = new System.Windows.Forms.Label();
            this.txtStartupUsername = new System.Windows.Forms.TextBox();
            this.btnStartupConnect = new System.Windows.Forms.Button();

            this.panelConnecting = new System.Windows.Forms.Panel();
            this.lblConnecting = new System.Windows.Forms.Label();
            this.txtConnectingLog = new System.Windows.Forms.TextBox();

            this.panelMain = new System.Windows.Forms.Panel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();

            // ** UPDATED controls for Main panel layout **
            this.lblUsername = new System.Windows.Forms.Label();
            this.lblUsersOnline = new System.Windows.Forms.Label();
            this.lblContent = new System.Windows.Forms.Label();
            this.lstContent = new System.Windows.Forms.ListBox();
            this.btnShare = new System.Windows.Forms.Button();

            // ───── Startup Panel ─────
            this.panelStartup.BackColor = System.Drawing.Color.FromArgb(28, 32, 38);
            this.panelStartup.Controls.Add(this.lblLogo);
            this.panelStartup.Controls.Add(this.lblEnterUsername);
            this.panelStartup.Controls.Add(this.txtStartupUsername);
            this.panelStartup.Controls.Add(this.btnStartupConnect);
            this.panelStartup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelStartup.Name = "panelStartup";
            this.panelStartup.Size = new System.Drawing.Size(604, 487);
            this.panelStartup.TabIndex = 0;

            // lblLogo
            this.lblLogo.AutoSize = true;
            this.lblLogo.Font = new System.Drawing.Font("Segoe UI Semibold", 34F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblLogo.ForeColor = System.Drawing.Color.FromArgb(220, 230, 240);
            this.lblLogo.Location = new System.Drawing.Point(85, 60);
            this.lblLogo.Name = "lblLogo";
            this.lblLogo.Size = new System.Drawing.Size(425, 62);
            this.lblLogo.TabIndex = 0;
            this.lblLogo.Text = "SecureNet 🕸️";

            // lblEnterUsername
            this.lblEnterUsername.AutoSize = true;
            this.lblEnterUsername.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblEnterUsername.ForeColor = System.Drawing.Color.FromArgb(180, 200, 220);
            this.lblEnterUsername.Location = new System.Drawing.Point(200, 160);
            this.lblEnterUsername.Name = "lblEnterUsername";
            this.lblEnterUsername.Size = new System.Drawing.Size(207, 25);
            this.lblEnterUsername.TabIndex = 1;
            this.lblEnterUsername.Text = "Enter your username:";

            // txtStartupUsername
            this.txtStartupUsername.Font = new System.Drawing.Font("Segoe UI", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtStartupUsername.Location = new System.Drawing.Point(180, 200);
            this.txtStartupUsername.MaxLength = 32;
            this.txtStartupUsername.Name = "txtStartupUsername";
            this.txtStartupUsername.Size = new System.Drawing.Size(245, 31);
            this.txtStartupUsername.TabIndex = 2;
            this.txtStartupUsername.BackColor = System.Drawing.Color.FromArgb(60, 68, 90);
            this.txtStartupUsername.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtStartupUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtStartupUsername.TextChanged += new System.EventHandler(this.txtStartupUsername_TextChanged);

            // btnStartupConnect
            this.btnStartupConnect.BackColor = System.Drawing.Color.FromArgb(90, 200, 130);
            this.btnStartupConnect.FlatAppearance.BorderSize = 0;
            this.btnStartupConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnStartupConnect.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnStartupConnect.ForeColor = System.Drawing.Color.White;
            this.btnStartupConnect.Location = new System.Drawing.Point(230, 260);
            this.btnStartupConnect.Name = "btnStartupConnect";
            this.btnStartupConnect.Size = new System.Drawing.Size(140, 40);
            this.btnStartupConnect.TabIndex = 3;
            this.btnStartupConnect.Text = "Connect";
            this.btnStartupConnect.UseVisualStyleBackColor = false;
            this.btnStartupConnect.Enabled = false;
            this.btnStartupConnect.Click += new System.EventHandler(this.btnStartupConnect_Click);

            // ───── Connecting Panel ─────
            this.panelConnecting.BackColor = System.Drawing.Color.FromArgb(28, 32, 38);
            this.panelConnecting.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelConnecting.Name = "panelConnecting";
            this.panelConnecting.Size = new System.Drawing.Size(604, 487);
            this.panelConnecting.TabIndex = 1;
            this.panelConnecting.Visible = false;

            // lblConnecting
            this.lblConnecting.AutoSize = true;
            this.lblConnecting.Font = new System.Drawing.Font("Segoe UI Semibold", 22F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblConnecting.ForeColor = System.Drawing.Color.FromArgb(120, 200, 255);
            this.lblConnecting.Location = new System.Drawing.Point(190, 70);
            this.lblConnecting.Name = "lblConnecting";
            this.lblConnecting.Size = new System.Drawing.Size(210, 41);
            this.lblConnecting.TabIndex = 0;
            this.lblConnecting.Text = "Connecting...";

            // txtConnectingLog
            this.txtConnectingLog.Multiline = true;
            this.txtConnectingLog.Font = new System.Drawing.Font("Consolas", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtConnectingLog.Location = new System.Drawing.Point(90, 140);
            this.txtConnectingLog.Name = "txtConnectingLog";
            this.txtConnectingLog.ReadOnly = true;
            this.txtConnectingLog.Size = new System.Drawing.Size(420, 220);
            this.txtConnectingLog.BackColor = System.Drawing.Color.FromArgb(30, 36, 48);
            this.txtConnectingLog.ForeColor = System.Drawing.Color.FromArgb(170, 210, 255);
            this.txtConnectingLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtConnectingLog.TabIndex = 1;

            this.panelConnecting.Controls.Add(this.lblConnecting);
            this.panelConnecting.Controls.Add(this.txtConnectingLog);

            // ───── Main Panel ─────
            this.panelMain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(40, 48, 64);
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelMain.Controls.Add(this.lblAppTitle);
            this.panelMain.Controls.Add(this.lblSubtitle);

            // ** UPDATED: place username & online count on the same line **
            this.panelMain.Controls.Add(this.lblUsername);
            this.panelMain.Controls.Add(this.lblUsersOnline);

            this.panelMain.Controls.Add(this.lblContent);
            this.panelMain.Controls.Add(this.lstContent);

            // ** UPDATED: share button pinned to bottom‐left **
            this.panelMain.Controls.Add(this.btnShare);

            this.panelMain.Location = new System.Drawing.Point(30, 22);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(540, 440);
            this.panelMain.TabIndex = 2;
            this.panelMain.Padding = new System.Windows.Forms.Padding(18);
            this.panelMain.Visible = false;

            // lblAppTitle
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 26F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(220, 230, 240);
            this.lblAppTitle.Location = new System.Drawing.Point(20, 20);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(250, 47);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "SecureNet 🕸️";

            // lblSubtitle
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(180, 200, 220);
            this.lblSubtitle.Location = new System.Drawing.Point(22, 75);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(312, 21);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Connect. Share. Stay private. Prototype Edition.";

            // ─── UPDATED: lblUsername ──────────────────────────────────────────────
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(200, 220, 240);
            this.lblUsername.Location = new System.Drawing.Point(24, 110);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(110, 20);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username: N/A";
            // (actual text set in InitializeMainPanelAfterConnect)

            // ─── UPDATED: lblUsersOnline (aligned on same Y as lblUsername, but anchored right) ───
            this.lblUsersOnline.AutoSize = true;
            this.lblUsersOnline.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUsersOnline.ForeColor = System.Drawing.Color.FromArgb(200, 220, 240);
            this.lblUsersOnline.Location = new System.Drawing.Point(420, 110);
            this.lblUsersOnline.Name = "lblUsersOnline";
            this.lblUsersOnline.Size = new System.Drawing.Size(100, 20);
            this.lblUsersOnline.TabIndex = 3;
            this.lblUsersOnline.Text = "Users online: 1";

            // lblContent (directly under the username/online row)
            this.lblContent.AutoSize = true;
            this.lblContent.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblContent.ForeColor = System.Drawing.Color.FromArgb(200, 220, 240);
            this.lblContent.Location = new System.Drawing.Point(24, 150);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(65, 20);
            this.lblContent.TabIndex = 4;
            this.lblContent.Text = "Content:";

            // lstContent (fills the middle area under “Content:”)
            this.lstContent.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.lstContent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lstContent.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lstContent.BackColor = System.Drawing.Color.FromArgb(50, 56, 74);
            this.lstContent.FormattingEnabled = true;
            this.lstContent.ItemHeight = 18;
            this.lstContent.Location = new System.Drawing.Point(24, 180);
            this.lstContent.Name = "lstContent";
            this.lstContent.Size = new System.Drawing.Size(496, 192);
            this.lstContent.TabIndex = 5;
            this.lstContent.DoubleClick += new System.EventHandler(this.lstContent_DoubleClick);

            // ─── UPDATED: btnShare (pinned bottom‐left inside panelMain) ────────────
            this.btnShare.BackColor = System.Drawing.Color.FromArgb(90, 200, 130);
            this.btnShare.FlatAppearance.BorderSize = 0;
            this.btnShare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShare.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShare.ForeColor = System.Drawing.Color.White;
            // Place it 10 pixels above the bottom of panelMain, and 24 from the left margin.
            this.btnShare.Location = new System.Drawing.Point(24, 400);
            this.btnShare.Name = "btnShare";
            this.btnShare.Size = new System.Drawing.Size(100, 30);
            this.btnShare.TabIndex = 6;
            this.btnShare.Text = "Share File";
            this.btnShare.UseVisualStyleBackColor = false;
            this.btnShare.Click += new System.EventHandler(this.btnShare_Click);

            // ───── Form (MainForm) ─────
            this.AcceptButton = this.btnStartupConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(28, 32, 38);
            this.ClientSize = new System.Drawing.Size(604, 487);
            this.Controls.Add(this.panelStartup);
            this.Controls.Add(this.panelConnecting);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SecureNet";

            // Finalize layouts
            this.panelStartup.ResumeLayout(false);
            this.panelStartup.PerformLayout();
            this.panelConnecting.ResumeLayout(false);
            this.panelConnecting.PerformLayout();
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel panelStartup;
        private System.Windows.Forms.Label lblLogo;
        private System.Windows.Forms.Label lblEnterUsername;
        private System.Windows.Forms.TextBox txtStartupUsername;
        private System.Windows.Forms.Button btnStartupConnect;

        private System.Windows.Forms.Panel panelConnecting;
        private System.Windows.Forms.Label lblConnecting;
        private System.Windows.Forms.TextBox txtConnectingLog;

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblSubtitle;

        // ** UPDATED: only label‐based username and online count **
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.Label lblUsersOnline;

        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.ListBox lstContent;
        private System.Windows.Forms.Button btnShare;
    }
}
