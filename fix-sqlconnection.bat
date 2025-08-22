@echo off
echo Commenting out VdO2013DataCore reference in SqlConnectionFactory.cs...

powershell -ExecutionPolicy Bypass -Command ^
"$file = 'C:\VdO_Progetto\VdO2013Core\Data\SqlConnectionFactory.cs'; ^
if (Test-Path $file) { ^
    $content = Get-Content $file -Raw; ^
    $content = $content -replace 'using VdO2013DataCore;', '// using VdO2013DataCore; // TODO: Fix circular dependency'; ^
    [System.IO.File]::WriteAllText($file, $content, [System.Text.Encoding]::UTF8); ^
    Write-Host 'Commented out VdO2013DataCore reference'; ^
}"

echo.
echo Done! Now rebuild VdO2013Core.
pause