namespace ZeroTierHelperClient
{
    partial class SettingsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SettingsForm));
            this.btnClose = new System.Windows.Forms.Button();
            this.lblAPIToken = new System.Windows.Forms.Label();
            this.tbAPIToken = new System.Windows.Forms.TextBox();
            this.lblAPIInfo = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            resources.ApplyResources(this.btnClose, "btnClose");
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Name = "btnClose";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lblAPIToken
            // 
            resources.ApplyResources(this.lblAPIToken, "lblAPIToken");
            this.lblAPIToken.Name = "lblAPIToken";
            // 
            // tbAPIToken
            // 
            resources.ApplyResources(this.tbAPIToken, "tbAPIToken");
            this.tbAPIToken.Name = "tbAPIToken";
            // 
            // lblAPIInfo
            // 
            resources.ApplyResources(this.lblAPIInfo, "lblAPIInfo");
            this.lblAPIInfo.Name = "lblAPIInfo";
            this.lblAPIInfo.TabStop = true;
            this.lblAPIInfo.UseCompatibleTextRendering = true;
            this.lblAPIInfo.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAPIInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAPIInfo_LinkClicked);
            // 
            // SettingsForm
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAPIInfo);
            this.Controls.Add(this.tbAPIToken);
            this.Controls.Add(this.lblAPIToken);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SettingsForm_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnClose;
        private System.Windows.Forms.Label lblAPIToken;
        private System.Windows.Forms.TextBox tbAPIToken;
        private System.Windows.Forms.LinkLabel lblAPIInfo;
    }
}