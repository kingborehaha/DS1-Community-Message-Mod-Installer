namespace rando_script
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.b_install = new System.Windows.Forms.Button();
            this.b_browse = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.b_restoreBackups = new System.Windows.Forms.Button();
            this.cb_SimpleInstall = new System.Windows.Forms.CheckBox();
            this.b_SimpleInstallInfo = new System.Windows.Forms.Button();
            this.cb_ModifyAllFMG = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // b_install
            // 
            this.b_install.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b_install.Location = new System.Drawing.Point(276, 89);
            this.b_install.Name = "b_install";
            this.b_install.Size = new System.Drawing.Size(75, 23);
            this.b_install.TabIndex = 0;
            this.b_install.Text = "Install";
            this.b_install.UseVisualStyleBackColor = true;
            this.b_install.Click += new System.EventHandler(this.b_install_Click);
            // 
            // b_browse
            // 
            this.b_browse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.b_browse.Location = new System.Drawing.Point(195, 89);
            this.b_browse.Name = "b_browse";
            this.b_browse.Size = new System.Drawing.Size(75, 23);
            this.b_browse.TabIndex = 1;
            this.b_browse.Text = "Browse";
            this.b_browse.UseVisualStyleBackColor = true;
            this.b_browse.Click += new System.EventHandler(this.b_browse_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.Filter = "DARKSOULS.EXE or DarkSoulsRemastered.exe|*.exe";
            // 
            // b_restoreBackups
            // 
            this.b_restoreBackups.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.b_restoreBackups.Location = new System.Drawing.Point(12, 89);
            this.b_restoreBackups.Name = "b_restoreBackups";
            this.b_restoreBackups.Size = new System.Drawing.Size(79, 23);
            this.b_restoreBackups.TabIndex = 2;
            this.b_restoreBackups.Text = "Uninstall";
            this.b_restoreBackups.UseVisualStyleBackColor = true;
            this.b_restoreBackups.Click += new System.EventHandler(this.b_restoreBackups_Click);
            // 
            // cb_SimpleInstall
            // 
            this.cb_SimpleInstall.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cb_SimpleInstall.AutoSize = true;
            this.cb_SimpleInstall.Location = new System.Drawing.Point(12, 36);
            this.cb_SimpleInstall.Name = "cb_SimpleInstall";
            this.cb_SimpleInstall.Size = new System.Drawing.Size(265, 19);
            this.cb_SimpleInstall.TabIndex = 4;
            this.cb_SimpleInstall.Text = "Simple Installation (Mod Compatible version)";
            this.cb_SimpleInstall.UseVisualStyleBackColor = true;
            // 
            // b_SimpleInstallInfo
            // 
            this.b_SimpleInstallInfo.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.b_SimpleInstallInfo.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.b_SimpleInstallInfo.Location = new System.Drawing.Point(276, 34);
            this.b_SimpleInstallInfo.Name = "b_SimpleInstallInfo";
            this.b_SimpleInstallInfo.Size = new System.Drawing.Size(39, 21);
            this.b_SimpleInstallInfo.TabIndex = 5;
            this.b_SimpleInstallInfo.Text = "Info";
            this.b_SimpleInstallInfo.UseVisualStyleBackColor = true;
            this.b_SimpleInstallInfo.Click += new System.EventHandler(this.b_SimpleInstallInfo_Click);
            // 
            // cb_ModifyAllFMG
            // 
            this.cb_ModifyAllFMG.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.cb_ModifyAllFMG.AutoSize = true;
            this.cb_ModifyAllFMG.Enabled = false;
            this.cb_ModifyAllFMG.Location = new System.Drawing.Point(12, 61);
            this.cb_ModifyAllFMG.Name = "cb_ModifyAllFMG";
            this.cb_ModifyAllFMG.Size = new System.Drawing.Size(183, 19);
            this.cb_ModifyAllFMG.TabIndex = 3;
            this.cb_ModifyAllFMG.Text = "Modify Non-English Text Files";
            this.cb_ModifyAllFMG.UseVisualStyleBackColor = true;
            this.cb_ModifyAllFMG.Visible = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(363, 124);
            this.Controls.Add(this.b_SimpleInstallInfo);
            this.Controls.Add(this.cb_SimpleInstall);
            this.Controls.Add(this.cb_ModifyAllFMG);
            this.Controls.Add(this.b_restoreBackups);
            this.Controls.Add(this.b_browse);
            this.Controls.Add(this.b_install);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "DS1 Community Message Mod Installer";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button b_install;
        private Button b_browse;
        private OpenFileDialog openFileDialog1;
        private Button b_restoreBackups;
        private CheckBox cb_SimpleInstall;
        private Button b_SimpleInstallInfo;
        private CheckBox cb_ModifyAllFMG;
    }
}