@echo off

echo Adding all .cs files to MPLibraryStubs.csproj...



echo.

echo Backing up original project file...

copy "C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj" "C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj.backup"



echo.

echo Adding all .cs files to the project...

powershell -ExecutionPolicy Bypass -Command ^

"$projFile = 'C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj'; ^

$content = Get-Content $projFile -Raw; ^

$csFiles = Get-ChildItem 'C:\VdO_Progetto\MPLibraryStubs\*.cs' -Name; ^

$itemGroupMatch = [regex]::Match($content, '<ItemGroup>[\s\S]*?<Compile Include=[\s\S]*?</ItemGroup>'); ^

if ($itemGroupMatch.Success) { ^

    $itemGroup = $itemGroupMatch.Value; ^

    $newItemGroup = $itemGroup; ^

    foreach ($file in $csFiles) { ^

        if ($itemGroup -notmatch [regex]::Escape($file)) { ^

            Write-Host \"Adding $file to project...\"; ^

            $newCompile = \"    <Compile Include=\`\"$file\`\" />\`r\`n  </ItemGroup>\"; ^

            $newItemGroup = $newItemGroup -replace '</ItemGroup>', $newCompile; ^

        } else { ^

            Write-Host \"$file already in project\"; ^

        } ^

    } ^

    if ($itemGroup -ne $newItemGroup) { ^

        $content = $content.Replace($itemGroup, $newItemGroup); ^

        Set-Content -Path $projFile -Value $content -Encoding UTF8; ^

        Write-Host 'Project file updated successfully!'; ^

    } else { ^

        Write-Host 'All files already included in project'; ^

    } ^

} else { ^

    Write-Host 'Creating new ItemGroup for Compile items...'; ^

    $compileItems = ''; ^

    foreach ($file in $csFiles) { ^

        $compileItems += \"    <Compile Include=\`\"$file\`\" />\`r\`n\"; ^

    } ^

    $newItemGroup = \"  <ItemGroup>\`r\`n$compileItems  </ItemGroup>\"; ^

    $content = $content -replace '(</Project>)', \"$newItemGroup\`r\`n`$1\"; ^

    Set-Content -Path $projFile -Value $content -Encoding UTF8; ^

    Write-Host 'Project file updated with new ItemGroup!'; ^

}"



echo.

echo Displaying updated project file content:

echo ========================================

type "C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj"

echo ========================================



echo.

echo Done! The project file has been updated.

echo.

echo Now in Visual Studio:

echo 1. You may see a dialog saying the project file was modified

echo 2. Click "Ricarica" (Reload) or "Ricarica tutto" (Reload All)

echo 3. Then rebuild MPLibraryStubs

echo.

echo If something went wrong, restore from backup:

echo copy "C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj.backup" "C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj"

pause