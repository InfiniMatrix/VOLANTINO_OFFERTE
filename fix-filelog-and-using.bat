@echo off
echo Fixing FileLog class and remaining using statements...

echo Creating FileLog as non-static class...
(
echo using System;
echo.
echo public class FileLog
echo {
echo     public void Log^(string message^) { }
echo     public void LogError^(string message^) { }
echo     public void LogWarning^(string message^) { }
echo     public void LogDebug^(string message^) { }
echo     public void Write^(string message^) { }
echo     public void WriteLine^(string message^) { }
echo     public string FileName { get; set; }
echo     public FileLog^(^) { }
echo     public FileLog^(string fileName^) { FileName = fileName; }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo Removing remaining using statements for nested classes...
powershell -ExecutionPolicy Bypass -Command ^
"$files = Get-ChildItem -Path 'C:\VdO_Progetto\VdO2013Core' -Filter '*.cs' -Recurse; ^
foreach ($file in $files) { ^
    $content = Get-Content $file.FullName -Raw; ^
    $original = $content; ^
    $content = $content -replace 'using MPLogHelper\.UI;[\r\n]*', ''; ^
    $content = $content -replace 'using MPExceptionHelper\.UI;[\r\n]*', ''; ^
    $content = $content -replace 'using MPUtils\.PluginFactory;[\r\n]*', ''; ^
    $content = $content -replace 'using MPUtils\.TextRes;[\r\n]*', ''; ^
    $content = $content -replace 'using MPExceptionHelper\.ExceptionHelper;[\r\n]*', ''; ^
    $content = $content -replace 'using FileLog;[\r\n]*', ''; ^
    if ($content -ne $original) { ^
        [System.IO.File]::WriteAllText($file.FullName, $content, [System.Text.Encoding]::UTF8); ^
        Write-Host \"  Fixed: $($file.Name)\"; ^
    } ^
}"

echo.
echo Done! Now:
echo 1. In Visual Studio, right-click MPLibraryStubs project
echo 2. Select Add - Existing Item
echo 3. Add the FileLog.cs file
echo 4. Rebuild MPLibraryStubs
echo 5. Rebuild VdO2013Core
pause