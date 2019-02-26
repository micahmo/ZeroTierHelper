using System;
using System.Windows.Forms;

namespace ZeroTierHelperClient
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new MainForm());
            }
            catch (Exception ex)
            {
                MessageBox.Show(Properties.Resources.ErrorExclamation + Environment.NewLine + Environment.NewLine + ex.ToString());
            }
        }
    }
}
