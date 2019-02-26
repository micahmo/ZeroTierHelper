#region Usings

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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

            GetNetworks();
            CreateTabPages();
            GetMembers();
            CreateDataGrids();
        }

        private void GetNetworks()
        {
            _networks.Clear();

            List<object> networks = JsonConvert.DeserializeObject<List<object>>(DoRequest(BASE_URL + GET_NETWORKS_COMMAND));
            foreach (var jsonNetwork in networks)
            {
                dynamic network = JObject.Parse(jsonNetwork.ToString());
                string id = string.Empty;
                string name = string.Empty;
                foreach (JProperty jproperty in network)
                {
                    if (jproperty.Name == "id")
                    {
                        id = jproperty.Value.ToString();
                    }
                    else if (jproperty.Name == "config")
                    {
                        foreach (JProperty childjproperty in (dynamic)JObject.Parse(jproperty.Value.ToString()))
                        {
                            if (childjproperty.Name == "name")
                            {
                                name = childjproperty.Value.ToString();
                            }
                        }
                    }
                }

                _networks.Add(new Network { ID = id, Name = name });
            }
        }

        private void CreateTabPages()
        {
            tabControlNetworks.TabPages.Clear();

            foreach (Network network in _networks)
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

        private void GetMembers()
        {
            _networkMembers.Clear();

            foreach (Network network in _networks)
            {
                _networkMembers[network] = new List<Member>();

                List<object> networkMembers = JsonConvert.DeserializeObject<List<object>>(DoRequest(BASE_URL + string.Format(GET_NETWORK_MEMBERS_COMMAND, network.ID)));
                foreach (var jsonMember in networkMembers)
                {
                    dynamic member = JObject.Parse(jsonMember.ToString());
                    string name = string.Empty;
                    string description = string.Empty;
                    string id = string.Empty;
                    bool online = default(bool);
                    List<string> ips = new List<string>();

                    foreach (JProperty jproperty in member)
                    {
                        if (jproperty.Name == "name")
                        {
                            name = jproperty.Value.ToString();
                        }
                        else if (jproperty.Name == "description")
                        {
                            description = jproperty.Value.ToString();
                        }
                        else if (jproperty.Name == "nodeId")
                        {
                            id = jproperty.Value.ToString();
                        }
                        else if (jproperty.Name == "online")
                        {
                            online = bool.Parse(jproperty.Value.ToString());
                        }
                        else if (jproperty.Name == "config")
                        {
                            foreach (JProperty childjproperty in (dynamic)JObject.Parse(jproperty.Value.ToString()))
                            {
                                if (childjproperty.Name == "ipAssignments")
                                {
                                    List<object> ipAssignments = JsonConvert.DeserializeObject<List<object>>(childjproperty.Value.ToString());
                                    ips = ipAssignments.Cast<string>().ToList();
                                }
                            }
                        }
                    }

                    _networkMembers[network].Add(new Member
                    {
                        Name = name,
                        Description = description,
                        Online = online,
                        IPAssignmentsList = ips,
                        ID = id
                    });
                }
            }
        }

        private void CreateDataGrids()
        {
            foreach (KeyValuePair<Network, List<Member>> networkMembersPair in _networkMembers)
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

        private string DoRequest(string url)
        {
            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Headers["Authorization"] = $"bearer {Settings.Default.APIToken}";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Resources.WebRequsetError, url) + Environment.NewLine + Environment.NewLine + 
                    ex.ToString(), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty;
            }
        }

        #endregion

        #region Private consts

        private const string BASE_URL = "https://my.zerotier.com/api/";

        private const string GET_NETWORKS_COMMAND = "network";

        private const string GET_NETWORK_INFO_COMMAND = "network/{0}";

        private const string GET_NETWORK_MEMBERS_COMMAND = "network/{0}/member";

        #endregion

        #region Private fields

        private List<Network> _networks = new List<Network>();

        private Dictionary<Network, List<Member>> _networkMembers = new Dictionary<Network, List<Member>>();

        #endregion        
    }
}
