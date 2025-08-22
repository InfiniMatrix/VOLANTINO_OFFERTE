@echo off

echo Force rebuilding MPLibraryStubs...



echo.

echo Cleaning old build files...

if exist "C:\VdO_Progetto\MPLibraryStubs\bin" rd /s /q "C:\VdO_Progetto\MPLibraryStubs\bin"

if exist "C:\VdO_Progetto\MPLibraryStubs\obj" rd /s /q "C:\VdO_Progetto\MPLibraryStubs\obj"



echo.

echo Checking MPLibraryStubs.csproj...

type "C:\VdO_Progetto\MPLibraryStubs\MPLibraryStubs.csproj"



echo.

echo ===============================================

echo Try these steps in Visual Studio:

echo.

echo 1. Close the solution completely

echo 2. Delete these folders manually:

echo    - C:\VdO_Progetto\MPLibraryStubs\bin

echo    - C:\VdO_Progetto\MPLibraryStubs\obj

echo 3. Re-open the solution

echo 4. Right-click on the Solution (top level)

echo 5. Select "Pulisci soluzione" (Clean Solution)

echo 6. Select "Ricompila soluzione" (Rebuild Solution)

echo.

echo If that doesn't work, try:

echo - Close Visual Studio

echo - Delete the .vs folder in C:\VdO_Progetto\

echo - Restart Visual Studio

echo - Open the solution again

echo ===============================================

pause