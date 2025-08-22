@echo off
echo Checking SqlConnectionFactory.cs usage...

powershell -ExecutionPolicy Bypass -Command ^
"$file = 'C:\VdO_Progetto\VdO2013Core\Data\SqlConnectionFactory.cs'; ^
if (Test-Path $file) { ^
    Write-Host 'Content around line 5 of SqlConnectionFactory.cs:'; ^
    $lines = Get-Content $file; ^
    for ($i = 0; $i -lt [Math]::Min(20, $lines.Count); $i++) { ^
        Write-Host ('{0,3}: {1}' -f ($i+1), $lines[$i]); ^
    } ^
}"

pause