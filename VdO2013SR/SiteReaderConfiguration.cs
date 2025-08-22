using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VdO2013Core;
using VdO2013Core.Config;
using VdO2013QRCode;
using VdO2013SRCore;

namespace VdO2013SR
{
    /// <inheritdoc cref="ISiteReaderConfiguration" />
    /// <summary>
    /// Classe per la lettura ed il salvataggio delle impostazioni in un file di configurazione XML.
    /// </summary>
    [Serializable]
    internal class SiteReaderConfiguration : IEnumerable<KeyValueConfigurationElement>, ISiteReaderConfiguration
    {
        protected MPLogHelper.FileLog Logger;

        private Configuration _configuration;
        public readonly ISiteReader2 Reader;

        protected KeyValueConfigurationCollection Settings => Assigned ? ConfigSettingsHelper.AppSettings(Configuration) : null;
        public FeaturesSection GetFeaturesSection() => ConfigSettingsHelper.GetFeaturesSection(Configuration);

        internal Configuration Configuration
        {
            get
            {
                Exception error = null;
                if (_configuration == null)
                    ConfigSettingsHelper.Load(Reader.ReaderName, out _configuration, out error);

                if (error != null)
                    throw error;

                return _configuration;
            }
        }

        public bool Assigned => Configuration != null;
        public bool HasFile => Assigned && Configuration.HasFile;
        public string FilePath => Assigned ? Configuration.FilePath : null;

        public SiteReaderConfiguration(ISiteReader2 reader)
        {
            Logger = new MPLogHelper.FileLog(this);
            Reader = reader ?? throw new ArgumentNullException(nameof(reader));
        }

        internal void Reset()
        {
            _configuration = null;
        }

        internal bool Load(out Exception error, bool reload = false)
        {
            error = null;
            try
            {
                if (reload)
                    Reset();

                //!!
				Reader.QRCode.SettingsRead(out error);
                if (Global.DebugLevel > 1) Logger.WriteError(error);

                Logger.WriteSeparator();

                string Pad(string arg) => arg.PadRight(Global.padLenForName, Global.padCharForName);

                var t = this.GetType();
                Logger.WriteWarn(Global.logFormatForNameAndValue, Pad("Reader"), t.FullName);
                Logger.WriteWarn(Global.logFormatForNameAndValue, Pad("+->Assembly"), t.Assembly.FullName);
                Logger.WriteWarn(Global.logFormatForNameAndValue, Pad("+->CodeBase"), t.Assembly.CodeBase);
                Logger.WriteWarn(Global.logFormatForNameAndValue, Pad("+->Class"), t.AssemblyQualifiedName);
                Logger.WriteWarn(Global.logFormatForNameAndValue, Pad("+->Config"), _configuration.FilePath);

                if (Global.DebugLevel > 0)
                {
                    Logger.WriteInfo(Global.logFormatForNameAndValue, Pad("+->ConfigurationGet details:"), string.Empty);
                    foreach (var key in Configuration.AppSettings.Settings.AllKeys)
                    {
                        var setting=Configuration.AppSettings.Settings[key];
						Logger.WriteInfo(Global.logFormatForNameAndValue, Pad("+---->" + setting.Key), setting.Value);
                    }
                }

                Logger.WriteSeparator();
                return true;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return false;
        }
        internal bool Load(bool reload = false)
        {
            var result = Load(out Exception error, reload);
            if (Global.DebugLevel > 1) Logger.WriteError(error);
            return result;
        }

        internal bool Save(out Exception error)
        {
            try
            {
                if (Reader.QRCode.Changed)
                    Reader.QRCode.SettingsWrite(out error);
                return ConfigSettingsHelper.Save(_configuration, out error) == ConfigSettingsHelper.SettingChangeResult.Saved;
            }
            catch (Exception ex)
            {
                error = ex;
            }
            return false;
        }
        internal bool Save()
        {
            var result = Save(out Exception error);
            if (Global.DebugLevel > 1) Logger.WriteError(error);
            return result;
        }

        public bool Exists(string settingName) { return Assigned && ConfigSettingsHelper.AppSettingExists(Configuration, settingName); }

        public bool Upsert(SettingWrapper value, out Exception error)
        {
            return ConfigSettingsHelper.AppSettingWrite(Configuration, value.Key, value.Value, out error, ConfigSettingsHelper.SettingChangeMode.Upsert) != ConfigSettingsHelper.SettingChangeResult.Unchanged;

        }
        public bool Upsert(SettingWrapper value)
        {
            var result = Upsert(value, out Exception error);
            if (error != null) throw error;
            return result;
        }
        public bool Upsert(string key, ConfigSettingValue value, out Exception error)
        {
            return Upsert(new SettingWrapper(key, value), out error);
        }

        public bool Set(SettingWrapper value, out Exception error)
        {
            error = null;
            if (Exists(value.Key))
                return ConfigSettingsHelper.AppSettingWrite(Configuration, value.Key, value.Value, out error, ConfigSettingsHelper.SettingChangeMode.Modify) == ConfigSettingsHelper.SettingChangeResult.Modified;
            return false;
        }
        public bool Set(SettingWrapper value)
        {
            var result = Set(value, out Exception error);
            if (error != null) throw error;
            return result;
        }
        public bool Set(string key, ConfigSettingValue value, out Exception error)
        {
            return Set(new SettingWrapper(key, value), out error);
        }

        public bool Add(SettingWrapper value, out Exception error)
        {
            error = null;
            if (!Exists(value.Key))
                return ConfigSettingsHelper.AppSettingWrite(Configuration, value.Key, value.Value, out error, ConfigSettingsHelper.SettingChangeMode.Append) == ConfigSettingsHelper.SettingChangeResult.Appended;
            return false;
        }
        public bool Add(SettingWrapper value)
        {
            var result = Add(value, out Exception error);
            if (error != null) throw error;
            return result;
        }
        public bool Add(string key, ConfigSettingValue value, out Exception error)
        {
            return Add(new SettingWrapper(key, value), out error);
        }

        public bool Remove(string key)
        {
            if (Exists(key))
            {
                Settings.Remove(key);
                return true;
            }
            return false;
        }

        public KeyValueConfigurationElement this[string key] { get { return Settings[key]; } }

        public int Count { get { return Settings.Count; } }

        public string GetString(string key) => Get<string>(key);
        public int GetInt32(string key) => Get<int>(key);
        public bool GetBoolean(string key) => Get<bool>(key);
        public DateTime GetDateTime(string key) => Get<DateTime>(key);

		#region Add<TValue>
        public bool Add<TValue>(string key, TValue value, out Exception error) where TValue : IConvertible
        {
            error = null;
            if (Exists(key)) return false;
            return ConfigSettingsHelper.AppSettingWrite(Configuration, key, new ConfigSettingValue(value), out error, ConfigSettingsHelper.SettingChangeMode.Append) == ConfigSettingsHelper.SettingChangeResult.Appended;
        }
        public bool Add<TValue>(string key, TValue value) where TValue : IConvertible
        {
            var result = Add<TValue>(key, value, out Exception error);
            if (error != null) throw error;
            return result;
        }
		#endregion
		
		#region Get<TResult>
        public TResult Get<TResult>(string key, out Exception error) where TResult : IConvertible
        {
            error = null;
            if (!Exists(key)) return default(TResult);
            var value = ConfigSettingsHelper.AppSettingRead(Configuration, key, out error);
            return (TResult)Convert.ChangeType(value.Value, typeof(TResult));
        }
        public TResult Get<TResult>(string key) where TResult : IConvertible
        {
            var result = Get<TResult>(key, out Exception error);
            if (error != null) throw error;
            return result;
        }
		#endregion
		
		#region Set<TValue>
        public bool Set<TValue>(string key, TValue value, out Exception error) where TValue : IConvertible
        {
            error = null;
            if (!Exists(key)) return false;
            return ConfigSettingsHelper.AppSettingWrite(Configuration, key, new ConfigSettingValue(value), out error, ConfigSettingsHelper.SettingChangeMode.Modify) == ConfigSettingsHelper.SettingChangeResult.Modified;
        }
        public bool Set<TValue>(string key, TValue value) where TValue : IConvertible
        {
            var result = Set<TValue>(key, value, out Exception error);
            if (error != null) throw error;
            return result;
        }
		#endregion
		
		#region Upsert<TValue>
        public bool Upsert<TValue>(string key, TValue value, out Exception error) where TValue : IConvertible
        {
            error = null;
            //if (!Exists(key)) return false;
            return ConfigSettingsHelper.AppSettingWrite(Configuration, key, new ConfigSettingValue(value), out error, ConfigSettingsHelper.SettingChangeMode.Upsert) != ConfigSettingsHelper.SettingChangeResult.Unchanged;
        }
        public bool Upsert<TValue>(string key, TValue value) where TValue : IConvertible
        {
            var result = Upsert<TValue>(key, value, out Exception error);
            if (error != null) throw error;
            return result;
        }
		#endregion

        #region IEnumerable<KeyValueConfigurationElement> members
        public IEnumerator<KeyValueConfigurationElement> GetEnumerator()
        {
            return (from KeyValueConfigurationElement s in Settings select s).GetEnumerator();
        }
        #endregion

        #region IEnumerable members
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return Settings.GetEnumerator();
        }
        #endregion
    }
}
