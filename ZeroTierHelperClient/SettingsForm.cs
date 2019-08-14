using System.Diagnostics;
using System.Windows.Forms;

namespace ZeroTierHelperClient
{
    /// <summary>
    /// Form which displays settings
    /// </summary>
    public partial class SettingsForm : Form
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public SettingsForm()
        {
            InitializeComponent();
            InitializeDataBinding();
        }

        #endregion

        #region Event handlers

        private void lblAPIInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://my.zerotier.com");
        }

        private void SettingsForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            tbAPIToken.Text = tbAPIToken.Text.Trim();
        }

        #endregion

        #region Private methods

        private void InitializeDataBinding()
        {
            BindingSource apiTokenBindingSource = new BindingSource {DataSource = Settings.Default};
            tbAPIToken.DataBindings.Add(nameof(tbAPIToken.Text), apiTokenBindingSource, nameof(Settings.APIToken));

            BindingSource autoRefreshBindingSource = new BindingSource {DataSource = Settings.Default};
            cbAutoRefresh.DataBindings.Add(nameof(cbAutoRefresh.Checked), autoRefreshBindingSource, nameof(Settings.AutoRefresh));
        }

        #endregion
    }
}
