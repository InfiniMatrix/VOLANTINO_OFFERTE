@echo off

echo Fixing MPLibraryStubs references and extension methods...



echo Adding System.Windows.Forms and System.Drawing references to MPLibraryStubs project...

powershell -Command ^

"$proj = 'C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj'; ^

$content = Get-Content $proj -Raw; ^

if ($content -notmatch 'System.Windows.Forms') { ^

    $content = $content -replace '(</Reference>[\r\n\s]*)(</ItemGroup>)', '$1    <Reference Include=\"System.Windows.Forms\" />`r`n    <Reference Include=\"System.Drawing\" />`r`n  $2'; ^

    Set-Content -Path $proj -Value $content -Encoding UTF8; ^

    Write-Host 'Added Windows.Forms and Drawing references'; ^

}"



echo.

echo Fixing MPExtensionMethods to use correct Color namespace...

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

echo     public static string FormatWith^(this string format, params object[] args^) { return string.Format^(format, args ?? new object[0]^); }

echo     public static string Left^(this string value, int length^) { return value?.Length ^> length ? value.Substring^(0, length^) : value ?? ""; }

echo     public static string Right^(this string value, int length^) { return value?.Length ^> length ? value.Substring^(value.Length - length^) : value ?? ""; }

echo     public static int ReverseIndexOf^(this string value, char c^) { return value?.LastIndexOf^(c^) ?? -1; }

echo     public static int ReverseIndexOf^(this string value, string s^) { return value?.LastIndexOf^(s^) ?? -1; }

echo     public static string ReverseSubstring^(this string value, int startIndex^) { return value?.Substring^(startIndex^) ?? ""; }

echo     public static string Quote^(this string value^) { return "\"" + value + "\""; }

echo     public static bool IsInt32^(this string value^) { int result; return int.TryParse^(value, out result^); }

echo     public static int ToInt32^(this string value^) { return int.Parse^(value ?? "0"^); }

echo     public static string EscapeSpecialChar^(this string value^) { return value?.Replace^("\\", "\\\\"^).Replace^("\"", "\\\""^) ?? ""; }

echo     public static string UnescapeSpecialChar^(this string value^) { return value?.Replace^("\\\"", "\""^).Replace^("\\\\", "\\"^) ?? ""; }

echo     public static string ToTitleCase^(this string value^) { return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase^(value?.ToLower^(^) ?? ""^); }

echo     public static Color FromHexValue^(this string value^) { return System.Drawing.ColorTranslator.FromHtml^(value^); }

echo     public static string ToHexValue^(this Color color^) { return System.Drawing.ColorTranslator.ToHtml^(color^); }

echo.

echo     // Numeric extensions

echo     public static string Indent^(this int level^) { return new string^(' ', level * 4^); }

echo.

echo     // Enum extensions

echo     public static bool In^<T^>^(this T value, params T[] values^) where T : struct { return values != null ^&^& values.Contains^(value^); }

echo.

echo     // Assembly extensions

echo     public static string[] GetRecursiveReferencedAssemblyNames^(this System.Reflection.Assembly assembly^) { return new string[0]; }

echo }

) > C:\VdO_Progetto\MPLibraryStubs\MPExtensionMethods.cs



echo Fixing FileLog with correct Form type...

(

echo using System;

echo using System.Windows.Forms;

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



echo.

echo Done! Now:

echo 1. Close the MPLibraryStubs project in Visual Studio (right-click - Scarica Progetto)

echo 2. Reload the MPLibraryStubs project (right-click - Ricarica Progetto)

echo 3. Rebuild MPLibraryStubs

echo 4. Rebuild VdO2013Core

pause