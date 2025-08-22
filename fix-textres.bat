@echo off
echo Fixing TextRes naming conflict...

echo Renaming stub TextRes to MPTextRes...
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
echo // MPTextRes as base class for TextRes
echo public class MPTextRes
echo {
echo     public virtual string GetString^(string key^) { return key; }
echo     public virtual string GetString^(string key, params object[] args^) { return string.Format^(key, args^); }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\MPUtils.cs

echo Updating VdO2013Core TextRes.cs to inherit from MPTextRes...
powershell -ExecutionPolicy Bypass -Command ^
"$file = 'C:\VdO_Progetto\VdO2013Core\TextRes.cs'; ^
if (Test-Path $file) { ^
    $content = Get-Content $file -Raw; ^
    $content = $content -replace ': TextRes', ': MPTextRes'; ^
    [System.IO.File]::WriteAllText($file, $content, [System.Text.Encoding]::UTF8); ^
    Write-Host 'Updated TextRes.cs'; ^
}"

echo.
echo Done! Now:
echo 1. Rebuild MPLibraryStubs
echo 2. Add reference to VdO2013DataCore project
echo 3. Rebuild VdO2013Core
pause