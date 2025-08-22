@echo off

echo Creating final comprehensive fixes for all errors...



echo Creating updated FileLog.cs with ALL method overloads...

(

echo using System;

echo using System.Windows.Forms;

echo using System.Reflection;

echo.

echo public class FileLog

echo {

echo     private static FileLog _default = new FileLog^(^);

echo     public static FileLog Default { get { return _default; } }

echo.

echo     public string FileName { get; set; }

echo     public static bool Initialized { get; set; }

echo     public static bool CreateSubfolderPerDay { get; set; }

echo     public static bool ReplicateOnConsole { get; set; }

echo     public static int RemoveSubfolderPerDay { get; set; }

echo     public static int OlderThan { get; set; }

echo.

echo     public delegate Form GetMainFormDelegate^(^);

echo     public static GetMainFormDelegate GetMainForm { get; set; }

echo.

echo     public FileLog^(^) { }

echo     public FileLog^(string fileName^) { FileName = fileName; }

echo.

echo     public static void Initialize^(string path^) { Initialized = true; }

echo     public static void Initialize^(string path, bool createSubfolderPerDay^) { Initialized = true; CreateSubfolderPerDay = createSubfolderPerDay; }

echo     public static void Initialize^(string path, string appName^) { Initialized = true; }

echo.

echo     // Basic methods

echo     public void Log^(string message^) { }

echo     public void LogError^(string message^) { }

echo     public void LogWarning^(string message^) { }

echo     public void LogDebug^(string message^) { }

echo     public void Write^(string message^) { }

echo     public void WriteLine^(string message^) { }

echo     public void WriteLine^(LogKind kind, string message^) { }

echo     public void WriteLine^(string message, LogKind kind^) { }

echo.

echo     // WriteCtor overloads - INCLUDING OBJECT TYPES

echo     public void WriteCtor^(string message^) { }

echo     public void WriteCtor^(object obj^) { WriteCtor^(obj?.GetType^(^).Name ?? "null"^); }

echo     public void WriteCtor^(Type type^) { WriteCtor^(type?.Name ?? "null"^); }

echo.

echo     // WriteMethod overloads - INCLUDING METHODBASE

echo     public void WriteMethod^(string message^) { }

echo     public void WriteMethod^(MethodBase method^) { WriteMethod^(method?.Name ?? "unknown"^); }

echo     public void WriteMethod^(string format, object arg0^) { WriteMethod^(string.Format^(format, arg0^)^); }

echo     public void WriteMethod^(string format, object arg0, object arg1^) { WriteMethod^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2^) { WriteMethod^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteMethod^(string.Format^(format, arg0, arg1, arg2, arg3^)^); }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { WriteMethod^(string.Format^(format, arg0, arg1, arg2, arg3, arg4^)^); }

echo     public void WriteMethod^(string format, params object[] args^) { WriteMethod^(string.Format^(format, args^)^); }

echo.

echo     // WriteMethodResult overloads - INCLUDING METHODBASE

echo     public void WriteMethodResult^(string message^) { }

echo     public void WriteMethodResult^(MethodBase method^) { WriteMethodResult^(method?.Name ?? "unknown"^); }

echo     public void WriteMethodResult^(string format, object arg0^) { WriteMethodResult^(string.Format^(format, arg0^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1^) { WriteMethodResult^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2^) { WriteMethodResult^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteMethodResult^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { WriteMethodResult^(string.Format^(format, arg0, arg1, arg2, arg3, arg4^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5^) { WriteMethodResult^(string.Format^(format, arg0, arg1, arg2, arg3, arg4, arg5^)^); }

echo     public void WriteMethodResult^(string format, params object[] args^) { WriteMethodResult^(string.Format^(format, args^)^); }

echo.

echo     // WriteInfo overloads

echo     public void WriteInfo^(string message^) { }

echo     public void WriteInfo^(string format, object arg0^) { WriteInfo^(string.Format^(format, arg0^)^); }

echo     public void WriteInfo^(string format, object arg0, object arg1^) { WriteInfo^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteInfo^(string format, object arg0, object arg1, object arg2^) { WriteInfo^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteInfo^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteInfo^(string.Format^(format, arg0, arg1, arg2, arg3^)^); }

echo     public void WriteInfo^(string format, params object[] args^) { WriteInfo^(string.Format^(format, args^)^); }

echo.

echo     // WriteError overloads

echo     public void WriteError^(string message^) { }

echo     public void WriteError^(Exception ex^) { WriteError^(ex?.Message ?? "unknown error"^); }

echo     public void WriteError^(string message, Exception ex^) { WriteError^(message + ": " + ex?.Message^); }

echo     public void WriteError^(Exception ex, string message^) { WriteError^(message + ": " + ex?.Message^); }

echo     public void WriteError^(string format, object arg0^) { WriteError^(string.Format^(format, arg0^)^); }

echo     public void WriteError^(string format, object arg0, object arg1^) { WriteError^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2^) { WriteError^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteError^(string.Format^(format, arg0, arg1, arg2, arg3^)^); }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { WriteError^(string.Format^(format, arg0, arg1, arg2, arg3, arg4^)^); }

echo     public void WriteError^(string format, params object[] args^) { WriteError^(string.Format^(format, args^)^); }

echo.

echo     // WriteWarn overloads

echo     public void WriteWarn^(string message^) { }

echo     public void WriteWarn^(string format, object arg0^) { WriteWarn^(string.Format^(format, arg0^)^); }

echo     public void WriteWarn^(string format, object arg0, object arg1^) { WriteWarn^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteWarn^(string format, object arg0, object arg1, object arg2^) { WriteWarn^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteWarn^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteWarn^(string.Format^(format, arg0, arg1, arg2, arg3^)^); }

echo     public void WriteWarn^(string format, params object[] args^) { WriteWarn^(string.Format^(format, args^)^); }

echo.

echo     // WriteDebug overloads - INCLUDING METHODBASE

echo     public void WriteDebug^(string message^) { }

echo     public void WriteDebug^(MethodBase method^) { WriteDebug^(method?.Name ?? "unknown"^); }

echo     public void WriteDebug^(MethodBase method, string message^) { WriteDebug^(method?.Name + ": " + message^); }

echo     public void WriteDebug^(Exception ex^) { WriteDebug^(ex?.Message ?? "unknown error"^); }

echo     public void WriteDebug^(Exception ex, string message^) { WriteDebug^(message + ": " + ex?.Message^); }

echo     public void WriteDebug^(string format, object arg0^) { WriteDebug^(string.Format^(format, arg0^)^); }

echo     public void WriteDebug^(string format, object arg0, object arg1^) { WriteDebug^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteDebug^(string format, object arg0, object arg1, object arg2^) { WriteDebug^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteDebug^(string format, params object[] args^) { WriteDebug^(string.Format^(format, args^)^); }

echo }

echo.

echo public class FileLogUI 

echo { 

echo     public static void Initialize^(^) { }

echo }

echo.

echo public enum LogKind { Info, Warning, Error, Debug }

) > C:\VdO_Progetto\MPLibraryStubs\FileLog.cs



echo Creating MPExtensionMethods.cs with Split extension fix...

(

echo using System;

echo using System.Drawing;

echo using System.Linq;

echo using System.Reflection;

echo.

echo public static class MPExtensionMethods

echo {

echo     // String extensions

echo     public static bool IsNullOrEmpty^(this string value^) { return string.IsNullOrEmpty^(value^); }

echo     public static string SafeTrim^(this string value^) { return value?.Trim^(^) ?? string.Empty; }

echo     public static string FormatWith^(this string format, params object[] args^) { return string.Format^(format, args ?? new object[0]^); }

echo     public static string Left^(this string value, int length^) { return value?.Length ^> length ? value.Substring^(0, length^) : value ?? ""; }

echo     public static string Right^(this string value, int length^) { return value?.Length ^> length ? value.Substring^(value.Length - length^) : value ?? ""; }

echo     public static int ReverseIndexOf^(this string value, char c^) { return value?.LastIndexOf^(c^) ?? -1; }

echo     public static int ReverseIndexOf^(this string value, string s^) { return value?.LastIndexOf^(s^) ?? -1; }

echo     public static int ReverseIndexOf^(this string value, string s, int startIndex^) { return value?.LastIndexOf^(s, startIndex^) ?? -1; }

echo     public static string ReverseSubstring^(this string value, int startIndex^) { return value?.Substring^(startIndex^) ?? ""; }

echo     public static string ReverseSubstring^(this string value, int startIndex, int length^) { return value?.Substring^(startIndex, length^) ?? ""; }

echo     public static string Quote^(this string value^) { return "\"" + value + "\""; }

echo     public static bool IsInt32^(this string value^) { int result; return int.TryParse^(value, out result^); }

echo     public static int ToInt32^(this string value^) { return int.Parse^(value ?? "0"^); }

echo     public static string EscapeSpecialChar^(this string value^) { return value?.Replace^("\\", "\\\\"^).Replace^("\"", "\\\""^) ?? ""; }

echo     public static string UnescapeSpecialChar^(this string value^) { return value?.Replace^("\\\"", "\""^).Replace^("\\\\", "\\"^) ?? ""; }

echo     public static string ToTitleCase^(this string value^) { return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase^(value?.ToLower^(^) ?? ""^); }

echo     public static Color FromHexValue^(this string value^) { return System.Drawing.ColorTranslator.FromHtml^(value^); }

echo     public static string ToHexValue^(this Color color^) { return System.Drawing.ColorTranslator.ToHtml^(color^); }

echo     public static string AppendCRLF^(this string value^) { return value + "\r\n"; }

echo     public static string Replace^(this string value, string oldValue, string newValue, StringComparison comparison^) { return value?.Replace^(oldValue, newValue^) ?? ""; }

echo.

echo     // String Split extensions - FIX for StringSplitOptions

echo     public static string[] Split^(this string str, char separator, StringSplitOptions options^) 

echo     { 

echo         return str?.Split^(new char[] { separator }, options^) ?? new string[0]; 

echo     }

echo.

echo     // Numeric extensions

echo     public static string Indent^(this int level^) { return new string^(' ', level * 4^); }

echo     public static int OneWeek^(this int value^) { return value * 7; }

echo.

echo     // Enum extensions

echo     public static bool In^<T^>^(this T value, params T[] values^) where T : struct { return values != null ^&^& values.Contains^(value^); }

echo.

echo     // Assembly extensions

echo     public static string[] GetRecursiveReferencedAssemblyNames^(this Assembly assembly^) { return new string[0]; }

echo     public static AssemblyName GetRecursiveReferencedAssemblyNames^(this Assembly assembly, string name^) { return new AssemblyName^(name^); }

echo.

echo     // String array extensions

echo     public static int IndexOf^(this string[] array, string value^) { return Array.IndexOf^(array, value^); }

echo }

echo.

echo // Exception extension - FIX for Exception.Exception

echo public static class ExceptionExtensions

echo {

echo     public static Exception Exception^(this Exception ex^) { return ex; }

echo }

) > C:\VdO_Progetto\MPLibraryStubs\MPExtensionMethods.cs



echo Creating comprehensive AdditionalClasses.cs...

(

echo using System;

echo using System.IO;

echo using System.Collections.Generic;

echo using System.Collections;

echo.

echo // Directory extensions

echo public static class DirectoryInfoExtensions

echo {

echo     public static void Copy^(this DirectoryInfo source, string destDirName, bool recursive^) { }

echo     public static void Copy^(this DirectoryInfo source, string destDirName, bool recursive, OverwriteSetting overwrite, OverwriteTimeStampField timeField, CopySelection selection^) { }

echo     public static string GetExceptionMessage^(Exception ex^) { return ex?.Message ?? ""; }

echo }

echo.

echo // HTTP helpers

echo public static class HttpHelper

echo {

echo     public static string Get^(string url^) { return ""; }

echo }

echo.

echo public class HttpRedirectNotAllowedException : Exception 

echo {

echo     public HttpRedirectNotAllowedException^(^) : base^(^) { }

echo     public HttpRedirectNotAllowedException^(string message^) : base^(message^) { }

echo }

echo.

echo // UAC helpers

echo public static class UACHelper

echo {

echo     public static bool IsRunAsAdmin^(^) { return false; }

echo     public static bool IsUACEnabled^(^) { return true; }

echo     public static void RestartAsAdmin^(^) { }

echo     public static bool CanWriteToDirectory^(string path^) { return true; }

echo }

echo.

echo // Plugin factory classes

echo public class PluginActivatorFactory 

echo {

echo     public static IPluginActivatorFactory Create^(^) { return new PluginActivatorFactoryImpl^(^); }

echo }

echo.

echo public interface IPluginActivatorFactory : IEnumerable^<IPluginActivator^> 

echo {

echo }

echo.

echo public class PluginActivatorFactoryImpl : IPluginActivatorFactory

echo {

echo     public IEnumerator^<IPluginActivator^> GetEnumerator^(^) { yield break; }

echo     IEnumerator IEnumerable.GetEnumerator^(^) { return GetEnumerator^(^); }

echo }

echo.

echo public interface IPluginActivator 

echo { 

echo     Type[] GetPluginTypes^(^);

echo     object CreatePlugin^(Type type^);

echo }

echo.

echo public class DummyPluginActivator : IPluginActivator

echo {

echo     public Type[] GetPluginTypes^(^) { return new Type[0]; }

echo     public object CreatePlugin^(Type type^) { return null; }

echo }

echo.

echo // Enums

echo public enum OverwriteSetting { Never, Always, NewerOnly, Ask }

echo public enum OverwriteTimeStampField { None, Modified, Created }

echo public enum CopySelection { All, FilesOnly, DirectoriesOnly }

echo.

echo // VdO specific classes

echo public class VdO2013Main { }

echo public class Services { }

echo public class Suite { }

echo.

echo public class VDataStorage 

echo {

echo     public VDataStorage^(VDataStorageKind kind^) { }

echo }

echo.

echo public enum VDataStorageKind { Local, Network }

echo.

echo // Exception helpers

echo public class ExceptionHelperUI 

echo { 

echo     public static void Initialize^(^) { }

echo }

) > C:\VdO_Progetto\MPLibraryStubs\AdditionalClasses.cs



echo Updating MPExceptionHelper.cs...

(

echo using System;

echo using System.Windows.Forms;

echo.

echo public static class MPExceptionHelper

echo {

echo     public static void HandleException^(Exception ex^) { Console.WriteLine^(ex.Message^); }

echo     public static void HandleException^(Exception ex, string message^) { Console.WriteLine^(message + ": " + ex.Message^); }

echo     public static void HandleException^(Exception ex, bool useUI^) { HandleException^(ex^); }

echo     public static string GetExceptionDetails^(Exception ex^) { return ex.ToString^(^); }

echo.

echo     public static class UI

echo     {

echo         public static void ShowException^(Exception ex^) { }

echo         public static void ShowException^(Exception ex, string title^) { }

echo         public static void LogException^(Exception ex^) { }

echo     }

echo.

echo     public static class ExceptionHelper

echo     {

echo         public delegate void OnExceptionDelegate^(Exception ex^);

echo         public delegate Form GetMainFormDelegate^(^);

echo.

echo         public static OnExceptionDelegate OnException { get; set; }

echo         public static GetMainFormDelegate GetMainForm { get; set; }

echo.

echo         public static void Initialize^(^) { }

echo         public static void Initialize^(bool useUI^) { }

echo         public static void HandleException^(Exception ex^) { }

echo         public static string GetDetails^(Exception ex^) { return ex.ToString^(^); }

echo     }

echo }

) > C:\VdO_Progetto\MPLibraryStubs\MPExceptionHelper.cs



echo.

echo Done! Now rebuild MPLibraryStubs and then VdO2013Core.

pause