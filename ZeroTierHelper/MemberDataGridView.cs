using System;
using System.Diagnostics;
using System.Windows.Forms;
using ZeroTierHelper.Properties;

namespace ZeroTierHelper
{
    sealed class MemberDataGridView : DataGridView
    {
        public MemberDataGridView()
        {
            Columns.AddRange(
            new DataGridViewTextBoxColumn
            {
                Name = "Name",
                HeaderText = "Name",
                DataPropertyName = "Name"
            },
            new DataGridViewTextBoxColumn
            {
                Name = "Description",
                HeaderText = "Description",
                DataPropertyName = "Description",
                FillWeight = 150,
            },
            new DataGridViewTextBoxColumn
            {
                Name = "ID",
                HeaderText = "ID",
                DataPropertyName = "ID",
            },
            new DataGridViewCheckBoxColumn
            {
                Name = "Online",
                HeaderText = "Online",
                DataPropertyName = "Online",
                FillWeight = 35
            },
            new DataGridViewTextBoxColumn
            {
                Name = "IP Assignments",
                HeaderText = "IP Assignments",
                DataPropertyName = "IPAssignments"
            },
            new DataGridViewButtonColumn
            {
                Name = "RDP",
                Text = "RDP",
                Tag = "RDP",
                UseColumnTextForButtonValue = true,
                FillWeight = 50
            },
            new DataGridViewButtonColumn
            {
                Name = "File Share",
                Text = "File Share",
                Tag = "File Share",
                UseColumnTextForButtonValue = true,
                FillWeight = 75
            },
            new DataGridViewButtonColumn
            {
                Name = "Ping",
                Text = "Ping",
                Tag = "Ping",
                UseColumnTextForButtonValue = true,
                FillWeight = 50,
            }
            );

            Dock = DockStyle.Fill;
            ReadOnly = true;
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }

        protected override void OnCellClick(DataGridViewCellEventArgs e)
        {
            base.OnCellClick(e);

            if (e.ColumnIndex < 0 || e.RowIndex < 0) return;

            var column = Columns[e.ColumnIndex];
            Member member = Rows[e.RowIndex].DataBoundItem as Member;

            if (column.Tag?.ToString() == "RDP")
            {
                string ipAddress = GetIPAddress(member);
                if (string.IsNullOrEmpty(ipAddress) == false)
                {
                    Process.Start(Environment.ExpandEnvironmentVariables(@"%SystemRoot%\system32\mstsc.exe"), $"/v:{ipAddress}");
                }
            }
            else if (column.Tag?.ToString() == "File Share")
            {
                string ipAddress = GetIPAddress(member);
                if (string.IsNullOrEmpty(ipAddress) == false)
                {
                    Process.Start("explorer.exe", $"\\\\{ipAddress}");
                }
            }
            else if (column.Tag?.ToString() == "Ping")
            {
                string ipAddress = GetIPAddress(member);
                if (string.IsNullOrEmpty(ipAddress) == false)
                {
                    Process.Start("cmd", $"/C ping {ipAddress} -t");
                }
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

            if (string.IsNullOrEmpty(result) == false)
            {
                Clipboard.SetText(result);
            }
            return result;
        }
    }
}
