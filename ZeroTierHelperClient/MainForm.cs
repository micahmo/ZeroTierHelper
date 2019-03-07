﻿#region Usings

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Network = ZeroTierAPI.Network;
using Member = ZeroTierAPI.Member;
using ZeroTierHelperClient.Properties;
using WebHelper;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using Octokit;
using Application = System.Windows.Forms.Application;

#endregion

namespace ZeroTierHelperClient
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

            VerifyInstallation();

            // We want to display the data on load, but we don't want to bombard the user with error messages if there are any issues
            DoRefresh(suppressErrorMessages: true);
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

        private void helpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(Resources.HelpWindowHeader + Environment.NewLine +
                string.Format(Resources.VersionNumber, Application.ProductVersion) + Environment.NewLine + Environment.NewLine +
                Resources.APITokenHelp + Environment.NewLine + Environment.NewLine +
                Resources.NetworkHelp + Environment.NewLine + Environment.NewLine +
                Resources.DataHelp + Environment.NewLine + Environment.NewLine +
                Resources.InstallationHelp + Environment.NewLine + Environment.NewLine +
                (IsInstalled() ? Resources.InstallMode : Resources.RuntimeMode));
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            DoRefresh();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {
            ShowSettings();
        }

        private void btnInstall_Click(object sender, EventArgs e)
        {
            DoInstall();
        }

        private void checkForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CheckForUpdates();
        }

        #endregion

        #region Private methods

        private void ShowSettings()
        {
            new SettingsForm().ShowDialog();
            Settings.Default.Save();
            DoRefresh(suppressErrorMessages: true);
        }

        private void DoRefresh(bool suppressErrorMessages = false)
        {
            if (string.IsNullOrEmpty(Settings.Default.APIToken))
            {
                if (suppressErrorMessages == false)
                {
                    MessageBox.Show(Resources.MissingAPITokenError, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                return;
            }

            Cursor.Current = Cursors.WaitCursor;

            try
            {
                IEnumerable<Network> networks = ZeroTierAPI.Requests.GetNetworks(Settings.Default.APIToken);
                CreateTabPages(networks);
                IDictionary<Network, IList<Member>> networkMembers = ZeroTierAPI.Requests.GetMembers(Settings.Default.APIToken, networks);
                CreateDataGrids(networkMembers);
            }
            catch (Exception ex) when (ex is WebRequestException)
            {
                if (suppressErrorMessages == false)
                {
                    if ((ex as WebRequestException)?.ErrorCode == 403)
                    {
                        // If there is any other problem retrieving the data, show the user
                        MessageBox.Show(Resources.IncorrectAPIToken, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        // If there is any other problem retrieving the data, show the user
                        MessageBox.Show(ex.ToString(), Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void CreateTabPages(IEnumerable<Network> networks)
        {
            tabControlNetworks.TabPages.Clear();

            foreach (Network network in networks)
            {
                tabControlNetworks.TabPages.Add(new NetworkTabPage(network));
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

        private void VerifyInstallation()
        {
            if (IsInstalled())
            {
                // We're running from the install directory, hide the Install button
                btnInstall.Visible = false;
            }

            // Check if there's an OLD exe. If so, let the user know that the update was successful and delete the old EXE.
            if (File.Exists(CurrentApplicationPath + OLD_EXECUTABLE_EXTENSION))
            {
                MessageBox.Show(Resources.SuccessfullyUpdated, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                File.Delete(CurrentApplicationPath + OLD_EXECUTABLE_EXTENSION);
            }
        }

        private bool IsInstalled()
        {
            return CurrentApplicationPath.Contains(InstallPath);
        }

        private void DoInstall()
        {
            string[] commands =
            {
                // Make our Program Files directory
                $"mkdir \"{InstallPath}\"",
                
                // Copy the current executable to that directory
                $"copy \"{CurrentApplicationPath}\" \"{InstallPath}\" /Y",

                // Kill the current process
                $"taskkill /f /pid {Process.GetCurrentProcess().Id}",

                // Create a shortcut in the Start Menu
                $"powershell \"$s=(New-Object -COM WScript.Shell).CreateShortcut('{Path.Combine(StartMenuPath, Resources.ZeroTierHelperClient)}.lnk');$s.TargetPath = '{Path.Combine(InstallPath, CurrentApplicationExecutableName)}';$s.Save()\"",

                // Start the process from its new location
                $"\"{Path.Combine(InstallPath, CurrentApplicationExecutableName)}\"",

                // Wait for the old process to die
                $"timeout 1",

                // Delete the executable of the old process
                $"del \"{CurrentApplicationPath}\" /s /f /q",
            };

            StartCMDProcess(commands);
        }

        private void CheckForUpdates()
        {
            GitHubClient github = new GitHubClient(new ProductHeaderValue("ZeroTierHelper"));
            Task<IReadOnlyList<Release>> releases = github.Repository.Release.GetAll("micahmo", "ZeroTierHelper");
            string latestReleaseTag = releases.Result[0].TagName;

            if (latestReleaseTag == LATEST_RELEASE_TAG)
            {
                MessageBox.Show(Resources.UpToDate, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show(string.Format(Resources.NewVersionAvailable, latestReleaseTag) + Environment.NewLine +
                        Environment.NewLine + Resources.DownloadAndInstall, Text, MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    // The user chose to install, so initiate that process now.
                    string downloadUrl = $"https://github.com/micahmo/ZeroTierHelper/releases/download/{latestReleaseTag}/ZeroTierHelperClient.exe";
                    string[] commands =
                    {
                        // Kill the current process
                        $"taskkill /f /pid {Process.GetCurrentProcess().Id}",

                        // Wait for the process to die before we can rename the exe
                        $"timeout 1",

                        // Rename the current exe
                        $"move /y \"{CurrentApplicationPath}\" \"{CurrentApplicationPath + OLD_EXECUTABLE_EXTENSION}\"",

                        // Download the new exe
                        $"powershell \"[Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12; Invoke-WebRequest -Uri \"{downloadUrl}\" -OutFile \"{CurrentApplicationPath}\"\"",

                        // Launch the new exe
                        $"\"{CurrentApplicationPath}\"",

                        // Note: The old exe will be deleted on startup of the new exe
                    };

                    StartCMDProcess(commands);
                }
            }
        }

        private void StartCMDProcess(params string[] commands)
        {
            new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    Verb = "runas",
                    WindowStyle = ProcessWindowStyle.Hidden,
                    FileName = "cmd.exe",
                    Arguments = "/C " + string.Join(" & ", commands)
                }
            }.Start();
        }

        #endregion

        #region Readonly fields

        private string InstallPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), APPLICATION_NAME);

        private string StartMenuPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartMenu), "Programs");

        private string CurrentApplicationPath => new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).LocalPath;

        private string CurrentApplicationExecutableName => Path.GetFileName(CurrentApplicationPath);

        #endregion

        #region Consts

        private const string APPLICATION_NAME = "ZeroTierHelperClient";

        private const string LATEST_RELEASE_TAG = "v1.7";

        private const string OLD_EXECUTABLE_EXTENSION = "_OLD";

        #endregion
    }
}
