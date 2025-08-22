using System;
using System.IO;
using System.Collections.Generic;
using System.Collections;

// Directory extensions
public static class DirectoryInfoExtensions
{
    public static void Copy(this DirectoryInfo source, string destDirName, bool recursive) { }
    public static void Copy(this DirectoryInfo source, string destDirName, bool recursive, OverwriteSetting overwrite, OverwriteTimeStampField timeField, CopySelection selection) { }
    public static string GetExceptionMessage(Exception ex) { return ex?.Message ?? ""; }
}

// HTTP helpers
public static class HttpHelper
{
    public static string Get(string url) { return ""; }
}

public class HttpRedirectNotAllowedException : Exception 
{
    public HttpRedirectNotAllowedException() : base() { }
    public HttpRedirectNotAllowedException(string message) : base(message) { }
}

// UAC helpers
public static class UACHelper
{
    public static bool IsRunAsAdmin() { return false; }
    public static bool IsUACEnabled() { return true; }
    public static void RestartAsAdmin() { }
    public static bool CanWriteToDirectory(string path) { return true; }
}

// Plugin factory classes
public class PluginActivatorFactory 
{
    public static IPluginActivatorFactory Create() { return new PluginActivatorFactoryImpl(); }
}

public interface IPluginActivatorFactory : IEnumerable<IPluginActivator> 
{
}

public class PluginActivatorFactoryImpl : IPluginActivatorFactory
{
    public IEnumerator<IPluginActivator> GetEnumerator() { yield break; }
    IEnumerator IEnumerable.GetEnumerator() { return GetEnumerator(); }
}

public interface IPluginActivator 
{ 
    Type[] GetPluginTypes();
    object CreatePlugin(Type type);
}

public class DummyPluginActivator : IPluginActivator
{
    public Type[] GetPluginTypes() { return new Type[0]; }
    public object CreatePlugin(Type type) { return null; }
}

// Enums
public enum OverwriteSetting { Never, Always, NewerOnly, Ask }
public enum OverwriteTimeStampField { None, Modified, Created }
public enum CopySelection { All, FilesOnly, DirectoriesOnly }

// VdO specific classes
public class VdO2013Main { }
public class Services { }
public class Suite { }

public class VDataStorage 
{
    public VDataStorage(VDataStorageKind kind) { }
}

public enum VDataStorageKind { Local, Network }

// Exception helpers
public class ExceptionHelperUI 
{ 
    public static void Initialize() { }
}
