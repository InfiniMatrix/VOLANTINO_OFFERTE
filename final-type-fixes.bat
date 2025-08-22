@echo off

echo Creating final type conversion fixes...



echo Updating MPExtensionMethods.cs with type conversion methods...

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

echo     // String[] extensions

echo     public static int IndexOf^(this string[] array, string value^) { return Array.IndexOf^(array, value^); }

echo.

echo     // Object to string conversions

echo     public static string ToDebugString^(this object obj^) { return obj?.ToString^(^) ?? "null"; }

echo     public static string ToDebugString^(this MethodBase method^) { return method?.Name ?? "unknown"; }

echo     public static string ToDebugString^(this Type type^) { return type?.Name ?? "unknown"; }

echo     public static string ToDebugString^(this Exception ex^) { return ex?.Message ?? "unknown exception"; }

echo }

echo.

echo // String splitter extension

echo public static class StringExtensions

echo {

echo     public static string[] Split^(this string str, char separator, StringSplitOptions options^) 

echo     { 

echo         return str.Split^(new char[] { separator }, options^); 

echo     }

echo }

) > C:\VdO_Progetto\MPLibraryStubs\MPExtensionMethods.cs



echo Updating FileLog.cs with object parameter overloads...

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

echo     // Write methods with various overloads including object parameters

echo     public void WriteInfo^(string message^) { }

echo     public void WriteInfo^(string format, object arg0^) { }

echo     public void WriteInfo^(string format, object arg0, object arg1^) { }

echo     public void WriteInfo^(string format, object arg0, object arg1, object arg2^) { }

echo     public void WriteInfo^(string format, object arg0, object arg1, object arg2, object arg3^) { }

echo     public void WriteInfo^(string format, params object[] args^) { }

echo.

echo     public void WriteError^(string message^) { }

echo     public void WriteError^(Exception ex^) { WriteError^(ex.Message^); }

echo     public void WriteError^(string message, Exception ex^) { }

echo     public void WriteError^(Exception ex, string message^) { WriteError^(message, ex^); }

echo     public void WriteError^(string format, object arg0^) { }

echo     public void WriteError^(string format, object arg0, object arg1^) { }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2^) { }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2, object arg3^) { }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { }

echo     public void WriteError^(string format, params object[] args^) { }

echo.

echo     public void WriteWarn^(string message^) { }

echo     public void WriteWarn^(string format, object arg0^) { }

echo     public void WriteWarn^(string format, object arg0, object arg1^) { }

echo     public void WriteWarn^(string format, object arg0, object arg1, object arg2^) { }

echo     public void WriteWarn^(string format, object arg0, object arg1, object arg2, object arg3^) { }

echo     public void WriteWarn^(string format, params object[] args^) { }

echo.

echo     public void WriteDebug^(string message^) { }

echo     public void WriteDebug^(MethodBase method^) { WriteDebug^(method.Name^); }

echo     public void WriteDebug^(MethodBase method, string message^) { WriteDebug^(method.Name + ": " + message^); }

echo     public void WriteDebug^(Exception ex^) { WriteDebug^(ex.Message^); }

echo     public void WriteDebug^(Exception ex, string message^) { WriteDebug^(message + ": " + ex.Message^); }

echo     public void WriteDebug^(string format, object arg0^) { }

echo     public void WriteDebug^(string format, object arg0, object arg1^) { }

echo     public void WriteDebug^(string format, object arg0, object arg1, object arg2^) { }

echo     public void WriteDebug^(string format, params object[] args^) { }

echo.

echo     public void WriteMethod^(string message^) { }

echo     public void WriteMethod^(MethodBase method^) { WriteMethod^(method.Name^); }

echo     public void WriteMethod^(string format, object arg0^) { }

echo     public void WriteMethod^(string format, object arg0, object arg1^) { }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2^) { }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2, object arg3^) { }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { }

echo     public void WriteMethod^(string format, params object[] args^) { }

echo.

echo     public void WriteMethodResult^(string message^) { }

echo     public void WriteMethodResult^(MethodBase method^) { WriteMethodResult^(method.Name^); }

echo     public void WriteMethodResult^(string format, object arg0^) { }

echo     public void WriteMethodResult^(string format, object arg0, object arg1^) { }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2^) { }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3^) { }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5^) { }

echo     public void WriteMethodResult^(string format, params object[] args^) { }

echo.

echo     public void WriteCtor^(string message^) { }

echo     public void WriteCtor^(object obj^) { WriteCtor^(obj?.GetType^(^).Name ?? "null"^); }

echo     public void WriteCtor^(Type type^) { WriteCtor^(type?.Name ?? "null"^); }

echo }

echo.

echo public class FileLogUI 

echo { 

echo     public static void Initialize^(^) { }

echo }

echo.

echo public enum LogKind { Info, Warning, Error, Debug }

) > C:\VdO_Progetto\MPLibraryStubs\FileLog.cs



echo Updating ProductPrivilege enum with Standard value...

(

echo public enum ProductPrivilege 

echo {

echo     None,

echo     Nessuna,

echo     Demo,

echo     Base,

echo     Standard,

echo     Amministrativa,

echo     Avanzata,

echo     Professional,

echo     Enterprise

echo }

) > C:\VdO_Progetto\MPLibraryStubs\ProductPrivilege.cs



echo.

echo Done! Now rebuild MPLibraryStubs and then VdO2013Core.

pause