#region Usings

using System.Drawing;
using System.Windows.Forms;

#endregion

namespace ZeroTierHelperClient
{
    /// <summary>
    /// Defines a set of static utility methods
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Returns the <see cref="Icon"/> association with the main form of the application.
        /// Returns <see langword="null"/> if no such icon is found.
        /// </summary>
        public static Icon GetApplicationIcon()
        {
            Icon result = null;

            if (Application.OpenForms.Count > 0)
            {
                result = Application.OpenForms[0]?.Icon;
            }

            return result;
        }
    }
}
