using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.Serialization;
using WPR.Common;

namespace WPR.WindowsCompability
{
    // projection: System......IsolatedStorageSettings
    public class IsolatedStorageSettings2 //RnD : static
    {
        private static IsolatedStorageSettings2 _ApplicationSettings;
        private const string LocalSettingsName = "__LocalSettings";

        private IsolatedStorageFile _Holder;
        private static Dictionary<string, object> _Settings;

        public IsolatedStorageSettings2()// RnD: static
        {
        }

        internal IsolatedStorageSettings2(IsolatedStorageFile file)
        {
            _Holder = file;
            if (!file.FileExists(LocalSettingsName))
            {
                _Settings = new Dictionary<string, object>();
            }
            else
            {
                using (IsolatedStorageFileStream fs = file.OpenFile(
                    LocalSettingsName, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    using (StreamReader sr = new StreamReader(fs))
                    {
                        DataContractSerializer reader = new DataContractSerializer(
                            typeof(Dictionary<string, object>));
                        try
                        {
                            _Settings = (reader.ReadObject(fs) as Dictionary<string, object>)!;
                        }
                        catch (Exception ex)
                        {
                            Log.Error(LogCategory.Common,
                                $"Failed to deserialize isolated settings. Error\n {ex}");
                        }

                        if (_Settings == null)
                        {
                            _Settings = new Dictionary<string, object>();
                        }
                    }
                }
            }
        }

        ~IsolatedStorageSettings2()
        {
            Save();
        }

        public void Save()
        {
            using (IsolatedStorageFileStream storage = _Holder.CreateFile(LocalSettingsName))
            {
                DataContractSerializer serializer = new DataContractSerializer(typeof(Dictionary<string, object>));
                serializer.WriteObject(storage, _Settings);
            }
        }


        // RnD: static 
        public static IsolatedStorageSettings2 ApplicationSettings
        {
            get
            {
                if (_ApplicationSettings == null)
                {
                    _ApplicationSettings = new
                        IsolatedStorageSettings2(
                            IsolatedStorageFile.GetUserStoreForApplication());
                }

                return _ApplicationSettings;
            }
        }

        public object this[string key]
        {
            get => _Settings[key];
            set => _Settings[key] = value;
        }

        public object? this[object key]
        {
            get
            {
                string? keyString = key as string;
                if (keyString == null)
                {
                    return null;
                }
                if (!_Settings.ContainsKey(keyString))
                {
                    return null;
                }

                return _Settings[keyString];
            }
            set
            {
                string? keyString = key as string;
                if (keyString == null)
                {
                    return;
                }
                if (!_Settings.ContainsKey(keyString))
                {
                    return;
                }

                _Settings[keyString] = value!;
            }
        }

        //RnD: static
        // [MaybeNullWhen(false)] 
        public static bool TryGetValue(string key, out object value)
        {
            //value = true;
            return _Settings.TryGetValue(key, out value);
        }

        //RnD : static
        //public IsolatedStorageSettings2 get_ApplicationSettings()
        //{
            //byte[] result = System.Security.Cryptography
            //   .ProtectedData.Unprotect(byteArrayOfOriginalData, 
            //   additionalEntropyOrSalt, 
            //   DataProtectionScope.CurrentUser);
            //return default;
        //}
    }
}
