#region Usings

using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using ZeroTierHelperClient.Properties;
using Member = ZeroTierAPI.Member;

#endregion

namespace ZeroTierHelperClient
{
    sealed class MemberDataGridView : DataGridView
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public MemberDataGridView()
        {
            Columns.AddRange(
            new DataGridViewTextBoxColumn
            {
                Name = NAME_COLUMN,
                HeaderText = Resources.Name,
                DataPropertyName = NAME_COLUMN
            },
            new DataGridViewTextBoxColumn
            {
                Name = DESCRIPTION_COLUMN,
                HeaderText = Resources.Description,
                DataPropertyName = DESCRIPTION_COLUMN,
                FillWeight = 150,
            },
            new DataGridViewTextBoxColumn
            {
                Name = ID_COLUMN,
                HeaderText = Resources.ID,
                DataPropertyName = ID_COLUMN,
            },
            new DataGridViewCheckBoxColumn
            {
                Name = ONLINE_COLUMN,
                HeaderText = Resources.Online,
                DataPropertyName = ONLINE_COLUMN,
                FillWeight = 35
            },
            new DataGridViewTextBoxColumn
            {
                Name = IP_ASSIGNMENTS_COLUMN,
                HeaderText = Resources.IPAssignments,
                DataPropertyName = IP_ASSIGNMENTS_COLUMN,
            },
            new DataGridViewButtonColumn
            {
                Name = RDP_COLUMN,
                Text = Resources.RDP,
                Tag = RDP_COLUMN,
                UseColumnTextForButtonValue = true,
                FillWeight = 50
            },
            new DataGridViewButtonColumn
            {
                Name = FILE_SHARE_COLUMN,
                Text = Resources.FileShare,
                Tag = FILE_SHARE_COLUMN,
                UseColumnTextForButtonValue = true,
                FillWeight = 75
            },
            new DataGridViewButtonColumn
            {
                Name = PING_COLUMN,
                Text = Resources.Ping,
                Tag = PING_COLUMN,
                UseColumnTextForButtonValue = true,
                FillWeight = 50,
            }
            );

            Dock = DockStyle.Fill;
            ReadOnly = true;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        #endregion

        #region Overrides

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            base.OnCellClick(e);

            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            var column = Columns[e.ColumnIndex];
            Member member = Rows[e.RowIndex].DataBoundItem as Member;

            if (column.Tag?.ToString() == RDP_COLUMN)
            {
                string ipAddress = GetIPAddress(member);
                if (string.IsNullOrEmpty(ipAddress) == false)
                {
                    Process.Start(Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe"), $"/v:{ipAddress}");
                }
            }
            else if (column.Tag?.ToString() == FILE_SHARE_COLUMN)
            {
                string ipAddress = GetIPAddress(member);
                if (string.IsNullOrEmpty(ipAddress) == false)
                {
                    Process.Start("explorer.exe", $"\\\\{ipAddress}");
                }
            }
            else if (column.Tag?.ToString() == PING_COLUMN)
            {
                string ipAddress = GetIPAddress(member);
                if (string.IsNullOrEmpty(ipAddress) == false)
                {
                    Process.Start("cmd", $"/C ping {ipAddress} -t");
                }
            }
        }

        protected override void OnCellMouseClick(DataGridViewCellMouseEventArgs e)
        {
            base.OnCellMouseClick(e);

            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            if (e.Button == MouseButtons.Right)
            {
                ShowContextMenuForCell(e.ColumnIndex, e.RowIndex, Cursor.Position);
            }
        }

        private string GetIPAddress(Member member)
        {
            string result = string.Empty;

            if (member.IPAssignmentsList.Count == 0)
            {
                MessageBox.Show(Resources.NoIPsAssignedToMember, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (member.IPAssignmentsList.Count > 1)
            {
                MessageBox.Show(Resources.MultipleIPsAssignedToMember, Resources.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (member.IPAssignmentsList.Count == 1)
            {
                result = member.IPAssignmentsList[0];
            }

            return result;
        }

        /// <summary>
        /// Given a <paramref name="columnIndex"/> and a <paramref name="rowIndex"/>, shows a context menu at the given <paramref name="location"/>.
        /// </summary>
        /// <param name="columnIndex"></param>
        /// <param name="rowIndex"></param>
        /// <param name="location"></param>
        private void ShowContextMenuForCell(int columnIndex, int rowIndex, Point location)
        {
            var column = Columns[columnIndex];
            var row = Rows[rowIndex];
            var cell = row.Cells[columnIndex];

            // Create the menu
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            // Create the "Copy <value>" menu item
            if (column is DataGridViewTextBoxColumn && string.IsNullOrEmpty(cell.Value?.ToString()) == false)
            {
                ToolStripMenuItem copyValueMenuItem = new ToolStripMenuItem
                {
                    Text = string.Format(Resources.CopyValue, cell.Value),
                };

                copyValueMenuItem.Click += (_, __) =>
                {
                    Clipboard.SetText(cell.Value.ToString());
                };

                contextMenu.Items.Add(copyValueMenuItem);
            }

            if (contextMenu.Items.Count > 0)
            {
                if (cell.Selected == false)
                {
                    ClearSelection();
                    cell.Selected = true;
                }

                contextMenu.Show(location);
            }
        }

        #endregion

        #region Private consts

        private const string NAME_COLUMN = "Name";

        private const string DESCRIPTION_COLUMN = "Description";

        private const string ID_COLUMN = "ID";

        private const string ONLINE_COLUMN = "Online";

        private const string IP_ASSIGNMENTS_COLUMN = "IPAssignments";

        private const string RDP_COLUMN = "RDP";

        private const string FILE_SHARE_COLUMN = "File Share";

        private const string PING_COLUMN = "Ping";

        #endregion
    }
}
