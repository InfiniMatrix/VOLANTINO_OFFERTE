@echo off

echo Diagnosing MPLibraryStubs issues...

echo.



echo 1. Checking if MPLibraryStubs.dll exists:

if exist "C:\VdO_Progetto\MPLibraryStubs\bin\Debug\MPLibraryStubs.dll" (

    echo    MPLibraryStubs.dll EXISTS

    echo    Size: 

    dir "C:\VdO_Progetto\MPLibraryStubs\bin\Debug\MPLibraryStubs.dll" | findstr MPLibraryStubs.dll

) else (

    echo    MPLibraryStubs.dll NOT FOUND - This is the problem!

)



echo.

echo 2. Checking .cs files in MPLibraryStubs folder:

dir C:\VdO_Progetto\MPLibraryStubs\*.cs /b



echo.

echo 3. Checking VdO2013Core project references:

echo Looking for MPLibraryStubs reference in VdO2013Core.csproj...

findstr /i "MPLibraryStubs" "C:\VdO_Progetto\VdO2013Core\VdO2013Core.csproj"



echo.

echo 4. Let's look at a specific error location...

echo Checking line 45 in ActionTaskManager.cs:

powershell -Command "Get-Content 'C:\VdO_Progetto\VdO2013Core\ActionTaskManager.cs' | Select-Object -Skip 44 -First 1"



echo.

echo 5. Checking line 82 in Global.cs (OneWeek error):

powershell -Command "Get-Content 'C:\VdO_Progetto\VdO2013Core\Global.cs' | Select-Object -Skip 81 -First 1"



echo.

echo ========================================

echo DIAGNOSIS:

echo.

echo If MPLibraryStubs.dll is missing:

echo   - MPLibraryStubs didn't build successfully

echo   - Check for build errors in MPLibraryStubs

echo.

echo If the reference is missing in VdO2013Core:

echo   - Need to add reference to MPLibraryStubs

echo.

echo Next steps:

echo 1. Try to build ONLY MPLibraryStubs first

echo 2. Check the Error List for MPLibraryStubs specific errors

echo 3. Make sure all .cs files show in Solution Explorer under MPLibraryStubs

echo ========================================

pause



echo.

echo Attempting to fix common issues...

echo.



echo Creating a simple test to verify classes are accessible...

(

echo using System;

echo.

echo namespace TestStubs

echo {

echo     class Program

echo     {

echo         static void Main()

echo         {

echo             // Test if classes exist

echo             var log = FileLog.Default;

echo             var helper = new UACHelper();

echo             var ext = DirectoryInfoExtensions.GetExceptionMessage(null);

echo             Console.WriteLine("All classes found!");

echo         }

echo     }

echo }

) > C:\VdO_Progetto\TestStubs.cs



echo.

echo If you can't see the stub classes in VdO2013Core, try:

echo.

echo 1. Close Visual Studio completely

echo 2. Delete these folders:

echo    - C:\VdO_Progetto\.vs

echo    - C:\VdO_Progetto\MPLibraryStubs\bin

echo    - C:\VdO_Progetto\MPLibraryStubs\obj

echo    - C:\VdO_Progetto\VdO2013Core\bin

echo    - C:\VdO_Progetto\VdO2013Core\obj

echo 3. Reopen Visual Studio and the solution

echo 4. Build ONLY MPLibraryStubs first

echo 5. Then build VdO2013Core

pause