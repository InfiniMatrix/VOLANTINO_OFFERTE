@echo off

echo Adding references to MPLibraryStubs.csproj...



powershell -ExecutionPolicy Bypass -Command "$proj = 'C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj'; $content = Get-Content $proj -Raw; if ($content -notmatch 'System.Windows.Forms') { $ref = '    <Reference Include=\"System\" />' + \"`r`n\" + '    <Reference Include=\"System.Core\" />' + \"`r`n\" + '    <Reference Include=\"System.Windows.Forms\" />' + \"`r`n\" + '    <Reference Include=\"System.Drawing\" />'; $content = $content -replace '(    <Reference Include=\"System\" />)', $ref; Set-Content -Path $proj -Value $content -Encoding UTF8; Write-Host 'Added references successfully'; } else { Write-Host 'References already exist'; }"



echo.

echo Done! Now rebuild MPLibraryStubs and VdO2013Core in Visual Studio.

pause