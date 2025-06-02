namespace SecureNetProto
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelMain = new System.Windows.Forms.Panel();
            this.lblAppTitle = new System.Windows.Forms.Label();
            this.lblSubtitle = new System.Windows.Forms.Label();
            this.lblUsername = new System.Windows.Forms.Label();
            this.txtUsername = new System.Windows.Forms.TextBox();
            this.btnConnect = new System.Windows.Forms.Button();
            this.lblUsersOnline = new System.Windows.Forms.Label();
            this.lstContent = new System.Windows.Forms.ListBox();
            this.lblContent = new System.Windows.Forms.Label();
            this.btnShare = new System.Windows.Forms.Button();
            this.panelMain.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelMain
            // 
            this.panelMain.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.panelMain.BackColor = System.Drawing.Color.FromArgb(40, 48, 64);
            this.panelMain.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.panelMain.Controls.Add(this.lblAppTitle);
            this.panelMain.Controls.Add(this.lblSubtitle);
            this.panelMain.Controls.Add(this.lblUsername);
            this.panelMain.Controls.Add(this.txtUsername);
            this.panelMain.Controls.Add(this.btnConnect);
            this.panelMain.Controls.Add(this.lblUsersOnline);
            this.panelMain.Controls.Add(this.lblContent);
            this.panelMain.Controls.Add(this.lstContent);
            this.panelMain.Controls.Add(this.btnShare);
            this.panelMain.Location = new System.Drawing.Point(30, 22);
            this.panelMain.Name = "panelMain";
            this.panelMain.Size = new System.Drawing.Size(540, 440);
            this.panelMain.TabIndex = 0;
            this.panelMain.Padding = new System.Windows.Forms.Padding(18);
            // 
            // lblAppTitle
            // 
            this.lblAppTitle.AutoSize = true;
            this.lblAppTitle.Font = new System.Drawing.Font("Segoe UI Semibold", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblAppTitle.ForeColor = System.Drawing.Color.FromArgb(220, 230, 240);
            this.lblAppTitle.Location = new System.Drawing.Point(14, 8);
            this.lblAppTitle.Name = "lblAppTitle";
            this.lblAppTitle.Size = new System.Drawing.Size(213, 45);
            this.lblAppTitle.TabIndex = 0;
            this.lblAppTitle.Text = "SecureNet 🕸️";
            // 
            // lblSubtitle
            // 
            this.lblSubtitle.AutoSize = true;
            this.lblSubtitle.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(150, 170, 200);
            this.lblSubtitle.Location = new System.Drawing.Point(20, 55);
            this.lblSubtitle.Name = "lblSubtitle";
            this.lblSubtitle.Size = new System.Drawing.Size(314, 20);
            this.lblSubtitle.TabIndex = 1;
            this.lblSubtitle.Text = "Connect. Share. Stay private. Prototype Edition.";
            // 
            // lblUsername
            // 
            this.lblUsername.AutoSize = true;
            this.lblUsername.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUsername.ForeColor = System.Drawing.Color.FromArgb(220, 230, 240);
            this.lblUsername.Location = new System.Drawing.Point(20, 90);
            this.lblUsername.Name = "lblUsername";
            this.lblUsername.Size = new System.Drawing.Size(77, 19);
            this.lblUsername.TabIndex = 2;
            this.lblUsername.Text = "Username:";
            // 
            // txtUsername
            // 
            this.txtUsername.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtUsername.Location = new System.Drawing.Point(104, 86);
            this.txtUsername.MaxLength = 32;
            this.txtUsername.Name = "txtUsername";
            this.txtUsername.Size = new System.Drawing.Size(170, 25);
            this.txtUsername.TabIndex = 3;
            this.txtUsername.BackColor = System.Drawing.Color.FromArgb(60, 68, 90);
            this.txtUsername.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.txtUsername.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            // 
            // btnConnect
            // 
            this.btnConnect.BackColor = System.Drawing.Color.FromArgb(90, 200, 130);
            this.btnConnect.FlatAppearance.BorderSize = 0;
            this.btnConnect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnConnect.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnConnect.ForeColor = System.Drawing.Color.White;
            this.btnConnect.Location = new System.Drawing.Point(286, 84);
            this.btnConnect.Name = "btnConnect";
            this.btnConnect.Size = new System.Drawing.Size(90, 29);
            this.btnConnect.TabIndex = 4;
            this.btnConnect.Text = "Connect";
            this.btnConnect.UseVisualStyleBackColor = false;
            this.btnConnect.Click += new System.EventHandler(this.btnConnect_Click);
            // 
            // lblUsersOnline
            // 
            this.lblUsersOnline.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblUsersOnline.AutoSize = true;
            this.lblUsersOnline.Font = new System.Drawing.Font("Segoe UI", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lblUsersOnline.ForeColor = System.Drawing.Color.FromArgb(170, 210, 255);
            this.lblUsersOnline.Location = new System.Drawing.Point(410, 24);
            this.lblUsersOnline.Name = "lblUsersOnline";
            this.lblUsersOnline.Size = new System.Drawing.Size(96, 19);
            this.lblUsersOnline.TabIndex = 5;
            this.lblUsersOnline.Text = "Users online: 0";
            // 
            // lblContent
            // 
            this.lblContent.AutoSize = true;
            this.lblContent.Font = new System.Drawing.Font("Segoe UI", 11F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.lblContent.ForeColor = System.Drawing.Color.FromArgb(220, 230, 240);
            this.lblContent.Location = new System.Drawing.Point(20, 135);
            this.lblContent.Name = "lblContent";
            this.lblContent.Size = new System.Drawing.Size(117, 20);
            this.lblContent.TabIndex = 6;
            this.lblContent.Text = "Shared Content";
            // 
            // lstContent
            // 
            this.lstContent.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.lstContent.FormattingEnabled = true;
            this.lstContent.ItemHeight = 17;
            this.lstContent.Location = new System.Drawing.Point(20, 157);
            this.lstContent.Name = "lstContent";
            this.lstContent.Size = new System.Drawing.Size(470, 180);
            this.lstContent.TabIndex = 7;
            this.lstContent.BackColor = System.Drawing.Color.FromArgb(50, 56, 74);
            this.lstContent.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.lstContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstContent.DoubleClick += lstContent_DoubleClick;
            // 
            // btnShare
            // 
            this.btnShare.BackColor = System.Drawing.Color.FromArgb(86, 156, 245);
            this.btnShare.FlatAppearance.BorderSize = 0;
            this.btnShare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnShare.Font = new System.Drawing.Font("Segoe UI Semibold", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnShare.ForeColor = System.Drawing.Color.White;
            this.btnShare.Location = new System.Drawing.Point(374, 355);
            this.btnShare.Name = "btnShare";
            this.btnShare.Size = new System.Drawing.Size(116, 37);
            this.btnShare.TabIndex = 8;
            this.btnShare.Text = "Share Content";
            this.btnShare.UseVisualStyleBackColor = false;
            this.btnShare.Click += new System.EventHandler(this.btnShare_Click);
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnConnect;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(28, 32, 38);
            this.ClientSize = new System.Drawing.Size(604, 487);
            this.Controls.Add(this.panelMain);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SecureNet";
            this.panelMain.ResumeLayout(false);
            this.panelMain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelMain;
        private System.Windows.Forms.Label lblAppTitle;
        private System.Windows.Forms.Label lblSubtitle;
        private System.Windows.Forms.Label lblUsername;
        private System.Windows.Forms.TextBox txtUsername;
        private System.Windows.Forms.Button btnConnect;
        private System.Windows.Forms.Label lblUsersOnline;
        private System.Windows.Forms.ListBox lstContent;
        private System.Windows.Forms.Label lblContent;
        private System.Windows.Forms.Button btnShare;
    }
}
