#region Usings

using System;
using System.IO;
using System.Linq;
using System.Net;

#endregion

namespace WebHelper
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
                var errorCodesInMessage = Enum.GetValues(typeof(Codes)).Cast<Codes>().Where(code => ex.ToString().Contains(code.ToString("D")));
                if (errorCodesInMessage.Any())
                {
                    throw new WebRequestException(string.Format(Properties.Resources.WebRequsetError, url) + Environment.NewLine + Environment.NewLine + ex.ToString(), (int)errorCodesInMessage.ElementAt(0));
                }
                else
                {
                    throw new WebRequestException(string.Format(Properties.Resources.WebRequsetError, url) + Environment.NewLine + Environment.NewLine + ex.ToString());
                }                
            }

            return result;
        }

        #endregion
    }

    /// <summary>
    /// Defines exceptions thrown by WebHelperRequests
    /// </summary>
    public class WebRequestException : Exception
    {
        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        public WebRequestException(string message) : base(message) { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="message"></param>
        /// <param name="code"></param>
        public WebRequestException(string message, int code) : base(message)
        {
            ErrorCode = code;
        }

        #endregion

        #region Public properties

        /// <summary>
        /// The error code returned by the web request. -1 if unknown.
        /// </summary>
        public int ErrorCode { get; } = -1;

        #endregion
    }

    /// <summary>
    /// Defines the list of supported HTTP return codes
    /// </summary>
    public enum Codes
    {
        Forbidden = 403
    }
}
