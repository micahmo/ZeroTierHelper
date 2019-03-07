#region Usings

using System.ComponentModel;
using System.Windows.Forms;
using ZeroTierAPI;

#endregion

namespace ZeroTierHelperClient
{
    /// <summary>
    /// Defines a <see cref="TabPage"/> control to show <see cref="Network"/>s
    /// </summary>
    public sealed class NetworkTabPage : TabPage
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkTabPage() { }

        /// <summary>
        /// Constructor
        /// </summary>
        public NetworkTabPage(Network network)
        {
            Network = network;

            Text = Network.Name + $" ({Network.ID})";
            Tag = Network.ID;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The <see cref="Network"/> associated with this TabPage
        /// </summary>
        [Browsable(false)]
        public Network Network { get; set; }

        #endregion
    }
}
