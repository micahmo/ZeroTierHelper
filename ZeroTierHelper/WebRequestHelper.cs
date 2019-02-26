#region Usings

using System;
using System.IO;
using System.Net;

#endregion

namespace ZeroTierHelper
{
    /// <summary>
    /// Class containing helper methods for web requests
    /// </summary>
    public class WebRequestHelper
    {
        #region Public static methods

        /// <summary>
        /// Performs an HTTPWebRequest with the given URL and returns the text response
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static string DoRequest(string url, string apiToken = "")
        {
            string result = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                if (string.IsNullOrEmpty(apiToken) == false)
                {
                    request.Headers["Authorization"] = $"bearer {apiToken}";
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(string.Format(Properties.Resources.WebRequsetError, url) + Environment.NewLine + Environment.NewLine + ex.ToString(), ex);
            }

            return result;
        }

        #endregion
    }
}
