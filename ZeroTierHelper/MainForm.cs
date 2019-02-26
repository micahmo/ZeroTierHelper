#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using ZeroTierHelper.Properties;

#endregion

namespace ZeroTierHelper
{
    /// <summary>
    /// MainForm class
    /// </summary>
    public partial class MainForm : Form
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides

        /// <summary>
        /// Override the OnLoad
        /// </summary>
        /// <param name="e"></param>
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrEmpty(Settings.Default.APIToken) == false)
            {
                DoRefresh();
            }
        }

        /// <summary>
        /// Override the CmdKey event so we can handle F5 and perform a refresh
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="keyData"></param>
        /// <returns></returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                DoRefresh();
                return true;
            }
            else
            {
                return base.ProcessCmdKey(ref msg, keyData);
            }
        }

        #endregion

        #region Event handlers

        private void btnHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.HelpWindowHeader + Environment.NewLine +
                string.Format(Resources.VersionNumber, Application.ProductVersion) + Environment.NewLine + Environment.NewLine +
                Resources.APITokenHelp + Environment.NewLine + Environment.NewLine +
                Resources.NetworkHelp + Environment.NewLine + Environment.NewLine +
                Resources.DataHelp);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DoRefresh();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        #endregion

        #region Private methods

        private void ShowSettings()
        {
            new SettingsForm().ShowDialog();
            Settings.Default.Save();
        }

        private void DoRefresh()
        {
            if (string.IsNullOrEmpty(Settings.Default.APIToken))
            {
                MessageBox.Show(Resources.MissingAPITokenError, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                IEnumerable<Network> networks = ZeroTierAPI.GetNetworks(Settings.Default.APIToken);
                CreateTabPages(networks);
                IDictionary<Network, IList<Member>> networkMembers = ZeroTierAPI.GetMembers(Settings.Default.APIToken, networks);
                CreateDataGrids(networkMembers);
            }
            catch (Exception ex)
            {
                // If there is any problem retrieving the data, show the user
                MessageBox.Show(ex.ToString(), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CreateTabPages(IEnumerable<Network> networks)
        {
            tabControlNetworks.TabPages.Clear();

            foreach (Network network in networks)
            {
                tabControlNetworks.TabPages.Add(new TabPage
                {
                    Text = network.Name + $" ({network.ID})",
                    Tag = network.ID
                });
            }

            if (tabControlNetworks.TabPages.Count > 0)
            {
                tabControlNetworks.SelectedIndex = 0;
            }
        }

        private void CreateDataGrids(IDictionary<Network, IList<Member>> networkMembers)
        {
            foreach (KeyValuePair<Network, IList<Member>> networkMembersPair in networkMembers)
            {
                // Find the tab page that holds this network
                foreach (TabPage tabPage in tabControlNetworks.TabPages)
                {
                    if (tabPage.Tag.ToString() == networkMembersPair.Key.ID)
                    {
                        MemberDataGridView dataGridView = new MemberDataGridView
                        {
                            DataSource = networkMembersPair.Value,
                        };

                        tabPage.Controls.Add(dataGridView);

                        break;
                    }
                }
            }
        }

        #endregion
    }
}
