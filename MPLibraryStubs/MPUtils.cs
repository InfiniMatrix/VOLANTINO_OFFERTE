using System;
using System.Collections.Generic;

public static class MPUtils
{
    public static string GetAppPath() { return AppDomain.CurrentDomain.BaseDirectory; }
    public static string GetTempPath() { return System.IO.Path.GetTempPath(); }
    public static bool IsNullOrEmpty(string value) { return string.IsNullOrEmpty(value); }

    public static class PluginFactory
    {
        public static object CreatePlugin(Type type) { return Activator.CreateInstance(type); }
        public static T CreatePlugin<T>() where T : new() { return new T(); }
    }

    public static class OSVersionInfo
    {
        public static string Name { get { return "Windows"; } }
        public static string Edition { get { return ""; } }  
        public static string ServicePack { get { return ""; } }
        public static string VersionString { get { return Environment.OSVersion.VersionString; } }
        public static int ProcessorBits { get { return Environment.Is64BitProcess ? 64 : 32; } }
        public static int OSBits { get { return Environment.Is64BitOperatingSystem ? 64 : 32; } }
        public static int ProgramBits { get { return IntPtr.Size * 8; } }
    }
}

public class MPTextRes
{
    public virtual string GetString(string key) { return key; }
    public virtual string GetString(string key, params object[] args) { return string.Format(key, args); }
    public static string KStaticConstructorMessageFormat { get { return "Static constructor for {0}"; } }
}
