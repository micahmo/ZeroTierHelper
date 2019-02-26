using System.Diagnostics;
using System.Windows.Forms;

namespace ZeroTierHelper
{
    /// <summary>
    /// Form which displays settings
    /// </summary>
    public partial class SettingsForm : Form
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
        }

        private void lblAPIInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://my.zerotier.com");
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tbAPIToken.Text = tbAPIToken.Text.Trim();
        }
    }
}
