using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using WPR.Common;

namespace WPR.WindowsCompability
{
    // projection: System......IsolatedStorageSettings
    public class IsolatedStorageSettings2 //RnD : static
    {
        private static IsolatedStorageSettings2? _ApplicationSettings;
        private readonly string? _SettingsPath;
        private static Dictionary<string, object> _Settings = new();

        public IsolatedStorageSettings2()// RnD: static
        {
        }

        internal IsolatedStorageSettings2(string? settingsPath)
        {
            _SettingsPath = settingsPath;
            if (settingsPath == null || !File.Exists(settingsPath))
            {
                _Settings = new Dictionary<string, object>();
            }
            else
            {
                using (FileStream fs = File.Open(settingsPath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    try
                    {
                        _Settings = IsolatedStorageSettingsSerializer.Deserialize(fs);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(LogCategory.Common,
                            $"Failed to deserialize isolated settings. Error\n {ex}");
                        _Settings = new Dictionary<string, object>();
                    }
                }
            }
        }

        public void Save()
        {
            if (_SettingsPath == null)
            {
                return;
            }

            IsolatedStorageSettingsSerializer.Save(_SettingsPath, _Settings);
        }


        // RnD: static 
        public static IsolatedStorageSettings2 ApplicationSettings
        {
            get
            {
                if (_ApplicationSettings == null)
                {
                    _ApplicationSettings = new IsolatedStorageSettings2(
                        IsolatedStorageSettingsSerializer.GetApplicationSettingsPath());
                }

                return _ApplicationSettings;
            }
        }

        public object this[string key]
        {
            get
            {
                return _Settings[key];
            }

            set
            {
                _Settings[key] = value;
            }
        }

        public int Count => _Settings.Count;

        public ICollection<string> Keys => _Settings.Keys;

        public ICollection<object> Values => _Settings.Values;

        public void Add(string key, object value)
        {
            _Settings.Add(key, value);
        }

        public bool Contains(string key)
        {
            return _Settings.ContainsKey(key);
        }

        public bool Remove(string key)
        {
            return _Settings.Remove(key);
        }

        public void Clear()
        {
            _Settings.Clear();
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
        public bool TryGetValue<T>(string key, out T value)
        {
            if (_Settings.TryGetValue(key, out object? stored) && stored is T typed)
            {
                value = typed;
                return true;
            }

            value = default!;
            return false;
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
