@echo off
echo Creating comprehensive MP library stubs with all extension methods...

echo Creating MPExtensionMethods.cs with all string extensions...
(
echo using System;
echo using System.Drawing;
echo using System.Linq;
echo.
echo public static class MPExtensionMethods
echo {
echo     // String extensions
echo     public static bool IsNullOrEmpty^(this string value^) { return string.IsNullOrEmpty^(value^); }
echo     public static string SafeTrim^(this string value^) { return value?.Trim^(^) ?? string.Empty; }
echo     public static string FormatWith^(this string format, params object[] args^) { return string.Format^(format, args^); }
echo     public static string Left^(this string value, int length^) { return value?.Length ^> length ? value.Substring^(0, length^) : value; }
echo     public static string Right^(this string value, int length^) { return value?.Length ^> length ? value.Substring^(value.Length - length^) : value; }
echo     public static int ReverseIndexOf^(this string value, char c^) { return value?.LastIndexOf^(c^) ?? -1; }
echo     public static int ReverseIndexOf^(this string value, string s^) { return value?.LastIndexOf^(s^) ?? -1; }
echo     public static string ReverseSubstring^(this string value, int startIndex^) { return value?.Substring^(startIndex^); }
echo     public static string Quote^(this string value^) { return "\"" + value + "\""; }
echo     public static bool IsInt32^(this string value^) { int result; return int.TryParse^(value, out result^); }
echo     public static int ToInt32^(this string value^) { return int.Parse^(value^); }
echo     public static string EscapeSpecialChar^(this string value^) { return value?.Replace^("\\", "\\\\"^).Replace^("\"", "\\\""^); }
echo     public static string UnescapeSpecialChar^(this string value^) { return value?.Replace^("\\\"", "\""^).Replace^("\\\\", "\\"^); }
echo     public static string ToTitleCase^(this string value^) { return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase^(value?.ToLower^(^) ?? ""^); }
echo     public static Color FromHexValue^(this string value^) { return ColorTranslator.FromHtml^(value^); }
echo     public static string ToHexValue^(this Color color^) { return ColorTranslator.ToHtml^(color^); }
echo.
echo     // Numeric extensions
echo     public static string Indent^(this int level^) { return new string^(' ', level * 4^); }
echo.
echo     // Enum extensions
echo     public static bool In^<T^>^(this T value, params T[] values^) where T : struct { return values.Contains^(value^); }
echo.
echo     // Assembly extensions
echo     public static string[] GetRecursiveReferencedAssemblyNames^(this System.Reflection.Assembly assembly^) { return new string[0]; }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\MPExtensionMethods.cs

echo Creating updated FileLog.cs with all properties and methods...
(
echo using System;
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
echo.
echo     public void Log^(string message^) { }
echo     public void LogError^(string message^) { }
echo     public void LogWarning^(string message^) { }
echo     public void LogDebug^(string message^) { }
echo     public void Write^(string message^) { }
echo     public void WriteLine^(string message^) { }
echo     public void WriteInfo^(string message^) { }
echo     public void WriteError^(string message^) { }
echo     public void WriteWarn^(string message^) { }
echo     public void WriteDebug^(string message^) { }
echo     public void WriteMethod^(string message^) { }
echo     public void WriteMethodResult^(string message^) { }
echo }
echo.
echo public class FileLogUI { }
echo public enum LogKind { Info, Warning, Error, Debug }
) > C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo Creating updated MPExceptionHelper.cs...
(
echo using System;
echo using System.Windows.Forms;
echo.
echo public static class MPExceptionHelper
echo {
echo     public static void HandleException^(Exception ex^) { Console.WriteLine^(ex.Message^); }
echo     public static void HandleException^(Exception ex, string message^) { Console.WriteLine^(message + ": " + ex.Message^); }
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
echo         public static void HandleException^(Exception ex^) { }
echo         public static string GetDetails^(Exception ex^) { return ex.ToString^(^); }
echo     }
echo }
echo.
echo public class ExceptionHelperUI { }
) > C:\VdO_Progetto\MPLibraryStubs\MPExceptionHelper.cs

echo Creating updated MPUtils.cs with all nested classes...
(
echo using System;
echo using System.Collections.Generic;
echo.
echo public static class MPUtils
echo {
echo     public static string GetAppPath^(^) { return AppDomain.CurrentDomain.BaseDirectory; }
echo     public static string GetTempPath^(^) { return System.IO.Path.GetTempPath^(^); }
echo     public static bool IsNullOrEmpty^(string value^) { return string.IsNullOrEmpty^(value^); }
echo.
echo     public static class PluginFactory
echo     {
echo         public static object CreatePlugin^(Type type^) { return Activator.CreateInstance^(type^); }
echo         public static T CreatePlugin^<T^>^(^) where T : new^(^) { return new T^(^); }
echo     }
echo.
echo     public static class OSVersionInfo
echo     {
echo         public static string Name { get { return "Windows"; } }
echo         public static string Edition { get { return ""; } }  
echo         public static string ServicePack { get { return ""; } }
echo         public static string VersionString { get { return Environment.OSVersion.VersionString; } }
echo     }
echo }
echo.
echo public class MPTextRes
echo {
echo     public virtual string GetString^(string key^) { return key; }
echo     public virtual string GetString^(string key, params object[] args^) { return string.Format^(key, args^); }
echo     public static string KStaticConstructorMessageFormat { get { return "Static constructor for {0}"; } }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\MPUtils.cs

echo Creating missing helper classes...
(
echo using System;
echo using System.IO;
echo.
echo public static class DirectoryInfoExtensions
echo {
echo     public static void Copy^(this DirectoryInfo source, string destDirName, bool recursive^) { }
echo }
echo.
echo public static class HttpHelper
echo {
echo     public static string Get^(string url^) { return ""; }
echo }
echo.
echo public class HttpRedirectNotAllowedException : Exception { }
echo.
echo public static class UACHelper
echo {
echo     public static bool IsRunAsAdmin^(^) { return false; }
echo     public static bool IsUACEnabled^(^) { return true; }
echo     public static void RestartAsAdmin^(^) { }
echo     public static bool CanWriteToDirectory^(string path^) { return true; }
echo }
echo.
echo public class PluginActivatorFactory { }
echo public interface IPluginActivatorFactory { }
echo public interface IPluginActivator { }
echo.
echo public enum OverwriteTimeStampField { None, Modified, Created }
echo public enum CopySelection { All, FilesOnly, DirectoriesOnly }
echo.
echo public class VdO2013Main { }
echo public class Services { }
echo public class Suite { }
echo.
echo public class VDataStorage { }
echo public enum VDataStorageKind { Local, Network }
) > C:\VdO_Progetto\MPLibraryStubs\AdditionalClasses.cs

echo Creating updated ProductPrivilege enum...
(
echo public enum ProductPrivilege 
echo {
echo     None,
echo     Nessuna,
echo     Demo,
echo     Base,
echo     Amministrativa,
echo     Avanzata,
echo     Professional,
echo     Enterprise
echo }
) > C:\VdO_Progetto\MPLibraryStubs\ProductPrivilege.cs

echo Creating updated OverwriteSetting enum...
(
echo public enum OverwriteSetting
echo {
echo     Never,
echo     Always,
echo     NewerOnly,
echo     Ask
echo }
) > C:\VdO_Progetto\MPLibraryStubs\OverwriteSetting.cs

echo Creating updated ImageFormatClass...
(
echo public enum ImageFormatClass
echo {
echo     Bmp,
echo     Jpeg,
echo     Gif,
echo     Tiff,
echo     Png,
echo     Unknown
echo }
) > C:\VdO_Progetto\MPLibraryStubs\ImageFormatClass.cs

echo.
echo Done! Now rebuild MPLibraryStubs and then VdO2013Core.
pause