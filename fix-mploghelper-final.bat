@echo off
echo Fixing MPLogHelper to remove nested FileLog class...

echo Creating updated MPLogHelper.cs without FileLog nested class...
(
echo using System;
echo using System.Collections.Generic;
echo.
echo public static class MPLogHelper
echo {
echo     public static void Log^(string message^) { Console.WriteLine^("[LOG] " + message^); }
echo     public static void LogError^(string message^) { Console.WriteLine^("[ERROR] " + message^); }
echo     public static void LogWarning^(string message^) { Console.WriteLine^("[WARN] " + message^); }
echo     public static void LogDebug^(string message^) { Console.WriteLine^("[DEBUG] " + message^); }
echo.
echo     public static class UI
echo     {
echo         public static void ShowMessage^(string message^) { }
echo         public static void ShowError^(string message^) { }
echo         public static void ShowWarning^(string message^) { }
echo         public static bool ShowQuestion^(string message^) { return true; }
echo     }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\MPLogHelper.cs

echo Creating updated MPUtils.cs without static TextRes...
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
echo }
echo.
echo // TextRes as separate non-static class
echo public class TextRes
echo {
echo     public virtual string GetString^(string key^) { return key; }
echo     public virtual string GetString^(string key, params object[] args^) { return string.Format^(key, args^); }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\MPUtils.cs

echo.
echo Now we need to replace MPLogHelper.FileLog with FileLog in the code...
powershell -ExecutionPolicy Bypass -Command ^
"$files = Get-ChildItem -Path 'C:\VdO_Progetto\VdO2013Core' -Filter '*.cs' -Recurse; ^
foreach ($file in $files) { ^
    $content = Get-Content $file.FullName -Raw; ^
    $original = $content; ^
    $content = $content -replace 'MPLogHelper\.FileLog', 'FileLog'; ^
    $content = $content -replace 'MPUtils\.TextRes', 'TextRes'; ^
    if ($content -ne $original) { ^
        [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8); ^
        Write-Host \"  Fixed: $($file.Name)\"; ^
    } ^
}"

echo.
echo Done! Now rebuild MPLibraryStubs and then VdO2013Core.
pause