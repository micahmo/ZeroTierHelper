#region Usings

using System.Drawing;
using System.Windows.Forms;
using ZeroTierHelperClient.Properties;

#endregion

namespace ZeroTierHelperClient
{
    /// <summary>
    /// Defines a <see cref="TabControl"/> to show <see cref="NetworkTabPage"/>s
    /// </summary>
    public class NetworkTabControl : TabControl
    {
        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkTabControl()
        {

        }

        #endregion

        #region Overrides

        /// <inheritdoc />
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            if (e.Button == MouseButtons.Right)
            {
                // Find the tab that was right-clicked
                for (int i = 0; i < TabCount; ++i)
                {
                    if (GetTabRect(i).Contains(e.Location))
                    {
                        ShowContextMenuForTab(Controls[i] as NetworkTabPage, Cursor.Position);
                        break;
                    }
                }
            }
        }

        #endregion

        #region Private methods

        private void ShowContextMenuForTab(NetworkTabPage tabPage, Point location)
        {
            if (tabPage == null) return;

            // Create the menu
            ContextMenuStrip contextMenu = new ContextMenuStrip();

            // Create the "Copy <value>" menu item
            if (string.IsNullOrEmpty(tabPage.Network.ID) == false)
            {
                ToolStripMenuItem copyValueMenuItem = new ToolStripMenuItem
                {
                    Text = string.Format(Resources.CopyValue, tabPage.Network.ID),
                };

                copyValueMenuItem.Click += (_, __) =>
                {
                    Clipboard.SetText(tabPage.Network.ID);
                };

                contextMenu.Items.Add(copyValueMenuItem);
            }

            if (contextMenu.Items.Count > 0)
            {
                contextMenu.Show(location);
            }
        }

        #endregion
    }
}
