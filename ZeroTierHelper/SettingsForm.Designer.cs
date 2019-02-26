namespace ZeroTierHelper
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
            this.btnClose = new System.Windows.Forms.Button();
            this.lblAPIToken = new System.Windows.Forms.Label();
            this.tbAPIToken = new System.Windows.Forms.TextBox();
            this.lblAPIInfo = new System.Windows.Forms.LinkLabel();
            this.SuspendLayout();
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Location = new System.Drawing.Point(384, 230);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 23);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "Close";
            this.btnClose.UseVisualStyleBackColor = true;
            // 
            // lblAPIToken
            // 
            this.lblAPIToken.AutoSize = true;
            this.lblAPIToken.Location = new System.Drawing.Point(12, 23);
            this.lblAPIToken.Name = "lblAPIToken";
            this.lblAPIToken.Size = new System.Drawing.Size(61, 13);
            this.lblAPIToken.TabIndex = 2;
            this.lblAPIToken.Text = "API Token:";
            // 
            // tbAPIToken
            // 
            this.tbAPIToken.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::ZeroTierHelper.Properties.Settings.Default, "APIToken", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.tbAPIToken.Location = new System.Drawing.Point(79, 20);
            this.tbAPIToken.Name = "tbAPIToken";
            this.tbAPIToken.Size = new System.Drawing.Size(380, 20);
            this.tbAPIToken.TabIndex = 3;
            this.tbAPIToken.Text = global::ZeroTierHelper.Properties.Settings.Default.APIToken;
            // 
            // lblAPIInfo
            // 
            this.lblAPIInfo.LinkArea = new System.Windows.Forms.LinkArea(106, 24);
            this.lblAPIInfo.Location = new System.Drawing.Point(12, 56);
            this.lblAPIInfo.Name = "lblAPIInfo";
            this.lblAPIInfo.Size = new System.Drawing.Size(441, 47);
            this.lblAPIInfo.TabIndex = 4;
            this.lblAPIInfo.TabStop = true;
            this.lblAPIInfo.Text = "Your API token provides access to your ZeroTier network data. Retrieve it from th" +
    "e ZeroTier account page (https://my.zerotier.com/) and paste above.";
            this.lblAPIInfo.UseCompatibleTextRendering = true;
            this.lblAPIInfo.VisitedLinkColor = System.Drawing.Color.Blue;
            this.lblAPIInfo.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblAPIInfo_LinkClicked);
            // 
            // SettingsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(471, 265);
            this.Controls.Add(this.lblAPIInfo);
            this.Controls.Add(this.tbAPIToken);
            this.Controls.Add(this.lblAPIToken);
            this.Controls.Add(this.btnClose);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "SettingsForm";
            this.Text = "Settings";
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