using System;
using System.IO;
using Newtonsoft.Json;

namespace WPR.Common
{
    public class Configuration
    {
        private class ConfigurationPrivate
        {
            public string DataStorePath;
            public String GamerTag;
            public string? RegistrationToken;
            public string? UserEmail;
            public bool IsRegistered;
        };

        private const string ConfigurationFilePath = "config.json";

        private string PrivateDataFolderPath;
        private ConfigurationPrivate? _ConfPrivate;

        public string? DataStorePath
        {
            get => _ConfPrivate!.DataStorePath;
            set => _ConfPrivate!.DataStorePath = value;
        }

        public string? GamerTag
        {
            get => _ConfPrivate!.GamerTag;
            set => _ConfPrivate!.GamerTag = value;
        }

        public string? RegistrationToken
        {
            get => _ConfPrivate!.RegistrationToken;
            set => _ConfPrivate!.RegistrationToken = value;
        }

        public string? UserEmail
        {
            get => _ConfPrivate!.UserEmail;
            set => _ConfPrivate!.UserEmail = value;
        }

        public bool IsRegistered
        {
            get => _ConfPrivate!.IsRegistered;
            set => _ConfPrivate!.IsRegistered = value;
        }

        public static Configuration? Current { get; set; }

        private string ConfigurationFilePathFull => Path.Combine(PrivateDataFolderPath, ConfigurationFilePath);

        public void RestoreDefaultDataStoragePath()
        {
            DataStorePath = PrivateDataFolderPath;
        }

        public Configuration(string PrivateDataFolder)
        {
            PrivateDataFolderPath = PrivateDataFolder;

            try
            {
                var seralizer = new JsonSerializer();
                _ConfPrivate = JsonConvert.DeserializeObject<ConfigurationPrivate>(File.ReadAllText(ConfigurationFilePathFull));
            } catch (Exception ex)
            {
                Log.Error(LogCategory.Common, $"Failed to load configuration file with error: {ex}");
                _ConfPrivate = new ConfigurationPrivate();
            }

            if (DataStorePath == null)
            {
                DataStorePath = PrivateDataFolder;
            }
        }

        ~Configuration()
        {
            Save();
        }

        public string DataPath(string path)
        {
            return Path.Combine(DataStorePath!, path);
        }
        public void Save()
        {
            File.WriteAllText(ConfigurationFilePathFull, JsonConvert.SerializeObject(_ConfPrivate));
        }
    }
}
