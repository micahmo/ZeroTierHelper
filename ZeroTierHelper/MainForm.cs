using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Windows.Forms;
using ZeroTierHelper.Properties;

namespace ZeroTierHelper
{
    public partial class MainForm : Form
    {

        #region Constructor

        public MainForm()
        {
            InitializeComponent();
        }

        #endregion

        #region Overrides

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (string.IsNullOrEmpty(Settings.Default.APIToken) == false)
            {
                btnRefresh.PerformClick();
            }
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.F5)
            {
                btnRefresh.PerformClick();
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
            MessageBox.Show($"—————————— ZeroTier Helper Client ——————————\nVersion {Application.ProductVersion}\n\nAccess to your API token is needed to retrieve network and member data. Click on Settings to enter your API token.\n\nOnce your data is retrieved, each network will be shown on a new tab. Within that tab, a list of members will be displayed in a grid.\n\nThis data is static, so at any time you may press Refresh to retrieve the latest data.");
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(Settings.Default.APIToken))
            {
                MessageBox.Show("API key is needed to access ZeroTier data. Please click on Settings and enter your API key.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            GetNetworks();
            CreateTabPages();
            GetMembers();
            CreateDataGrids();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            new SettingsForm().ShowDialog();
            Settings.Default.Save();
        }

        #endregion

        #region Private methods

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
                MessageBox.Show($"Error retrieving web request: \"{url}\"\n\n{ex}");
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
