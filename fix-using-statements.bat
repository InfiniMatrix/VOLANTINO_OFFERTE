@echo off
echo Fixing MP using statements in VdO2013Core...
echo.

powershell -ExecutionPolicy Bypass -Command ^
"$files = Get-ChildItem -Path 'C:\VdO_Progetto\VdO2013Core' -Filter '*.cs' -Recurse; ^
$count = 0; ^
foreach ($file in $files) { ^
    $content = Get-Content $file.FullName -Raw; ^
    $original = $content; ^
    $content = $content -replace 'using MPLogHelper;[\r\n]*', ''; ^
    $content = $content -replace 'using MPUtils;[\r\n]*', ''; ^
    $content = $content -replace 'using MPExtensionMethods;[\r\n]*', ''; ^
    $content = $content -replace 'using MPExceptionHelper;[\r\n]*', ''; ^
    $content = $content -replace 'using MPCommonRes;[\r\n]*', ''; ^
    $content = $content -replace 'using MPMailHelper;[\r\n]*', ''; ^
    $content = $content -replace 'using FileLog;[\r\n]*', ''; ^
    $content = $content -replace 'using ProductPrivilege;[\r\n]*', ''; ^
    $content = $content -replace 'using ImageFormatClass;[\r\n]*', ''; ^
    $content = $content -replace 'using OverwriteSetting;[\r\n]*', ''; ^
    $content = $content -replace 'using MPFingerPrint;[\r\n]*', ''; ^
    $content = $content -replace 'using GCalendarSettings;[\r\n]*', ''; ^
    if ($content -ne $original) { ^
        Set-Content -Path $file.FullName -Value $content -Encoding UTF8; ^
        Write-Host \"  Fixed: $($file.Name)\"; ^
        $count++; ^
    } ^
}; ^
Write-Host \"\"; ^
Write-Host \"Fixed $count files\" -ForegroundColor Green"

echo.
echo Done! Now try building VdO2013Core in Visual Studio.
pause