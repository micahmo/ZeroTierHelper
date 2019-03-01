#region Usings

using System;
using System.IO;
using System.Xml.Serialization;

#endregion

namespace ZeroTierHelperClient
{
    /// <summary>
    /// Defines the settings for the ZeroTier Helper Client
    /// </summary>
    [Serializable]
    public class Settings
    {
        #region Public static properties (singleton)

        /// <summary>
        /// Singleton instance of the Settings class
        /// </summary>
        public static Settings Default
        {
            get
            {
                if (_settings == null)
                {
                    try
                    {
                        XmlSerializer xmlSerializer = new XmlSerializer(typeof(Settings));
                        using (FileStream settingsFile = File.Open(SettingsPath, FileMode.Open))
                        {
                            using (StreamReader streamReader = new StreamReader(settingsFile))
                            {
                                _settings = (Settings)xmlSerializer.Deserialize(streamReader);
                            }
                        }
                    }
                    catch
                    {
                        _settings = new Settings();
                    }
                }

                return _settings;
            }
        }
        private static Settings _settings; // Backing field

        #endregion

        #region Public properties (settings)

        /// <summary>
        /// The API Token used to retrieve user data
        /// </summary>
        public string APIToken { get; set; }

        #endregion

        #region Public static methods

        /// <summary>
        /// Persist the settings
        /// </summary>
        public void Save()
        {
            if (Directory.Exists(SettingsFolderPath) == false)
            {
                Directory.CreateDirectory(SettingsFolderPath);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(Settings));
            using (Stream reader = new FileStream(SettingsPath, FileMode.Create))
            {
                serializer.Serialize(reader, this);
            }
        }

        #endregion

        #region Private static properties

        private static string SettingsPath => Path.Combine(SettingsFolderPath, ConfigFileName);

        private static string SettingsFolderPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), ConfigFolderName);

        private static string ConfigFolderName => "ZeroTierHelperClient";

        private static string ConfigFileName => "ZeroTierHelperClient.config";

        #endregion
    }
}
