using System;

namespace VdO2013SRCore
{
    public interface ISiteReaderConfiguration
    {
        bool Add(string key, VdO2013Core.Config.ConfigSettingValue value, out Exception error);
        bool Add(VdO2013SRCore.SettingWrapper value, out Exception error);
        bool Add(VdO2013SRCore.SettingWrapper value);
        bool Add<TValue>(string key, TValue value) where TValue : IConvertible;
        bool Add<TValue>(string key, TValue value, out Exception error) where TValue : IConvertible;
        bool GetBoolean(string key);
        DateTime GetDateTime(string key);
        int GetInt32(string key);
        string GetString(string key);
        bool Assigned { get; }
        bool HasFile { get; }
        string FilePath { get; }
        int Count { get; }
        bool Exists(string settingName);
        TResult Get<TResult>(string key, out Exception error) where TResult : IConvertible;
        TResult Get<TResult>(string key) where TResult : IConvertible;
        System.Collections.Generic.IEnumerator<System.Configuration.KeyValueConfigurationElement> GetEnumerator();
        VdO2013Core.Config.FeaturesSection GetFeaturesSection();
        bool Remove(string key);
        bool Set(string key, VdO2013Core.Config.ConfigSettingValue value, out Exception error);
        bool Set(VdO2013SRCore.SettingWrapper value, out Exception error);
        bool Set<TValue>(string key, TValue value) where TValue : IConvertible;
        bool Set<TValue>(string key, TValue value, out Exception error) where TValue : IConvertible;
        System.Configuration.KeyValueConfigurationElement this[string key] { get; }
        bool Upsert(string key, VdO2013Core.Config.ConfigSettingValue value, out Exception error);
        bool Upsert(VdO2013SRCore.SettingWrapper value, out Exception error);
        bool Upsert<TValue>(string key, TValue value) where TValue : IConvertible;
        bool Upsert<TValue>(string key, TValue value, out Exception error) where TValue : IConvertible;
    }
}
