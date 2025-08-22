@echo off
echo Creating MP Library Stubs...

mkdir MPLibraryStubs 2>nul

echo Creating MPLogHelper.cs...
(
echo using System;
echo namespace MP {
echo     public static class MPLogHelper {
echo         public static void Log^(string message^) { }
echo         public static void LogError^(string message^) { }
echo         public static void LogWarning^(string message^) { }
echo         public static void LogDebug^(string message^) { }
echo     }
echo }
) > MPLibraryStubs\MPLogHelper.cs

echo Creating MPUtils.cs...
(
echo using System;
echo namespace MP {
echo     public static class MPUtils {
echo         public static string GetAppPath^(^) =^> AppDomain.CurrentDomain.BaseDirectory;
echo         public static string GetTempPath^(^) =^> System.IO.Path.GetTempPath^(^);
echo         public static bool IsNullOrEmpty^(string value^) =^> string.IsNullOrEmpty^(value^);
echo     }
echo }
) > MPLibraryStubs\MPUtils.cs

echo Creating MPExtensionMethods.cs...
(
echo using System;
echo namespace MP {
echo     public static class MPExtensionMethods {
echo         public static bool IsNullOrEmpty^(this string value^) =^> string.IsNullOrEmpty^(value^);
echo         public static string SafeTrim^(this string value^) =^> value?.Trim^(^) ?? string.Empty;
echo     }
echo }
) > MPLibraryStubs\MPExtensionMethods.cs

echo Creating MPExceptionHelper.cs...
(
echo using System;
echo namespace MP {
echo     public static class MPExceptionHelper {
echo         public static void HandleException^(Exception ex^) { Console.WriteLine^(ex.Message^); }
echo         public static string GetExceptionDetails^(Exception ex^) =^> ex.ToString^(^);
echo     }
echo }
) > MPLibraryStubs\MPExceptionHelper.cs

echo Creating MPCommonRes.cs...
(
echo namespace MP {
echo     public static class MPCommonRes {
echo         public static string GetString^(string key^) =^> key;
echo         public static string ErrorMessage =^> "Error";
echo         public static string WarningMessage =^> "Warning";
echo     }
echo }
) > MPLibraryStubs\MPCommonRes.cs

echo Creating MPMailHelper.cs...
(
echo using System;
echo namespace MP {
echo     public static class MPMailHelper {
echo         public static void SendMail^(string to, string subject, string body^) { }
echo         public static bool ValidateEmail^(string email^) =^> email.Contains^("@"^);
echo     }
echo }
) > MPLibraryStubs\MPMailHelper.cs

echo.
echo MP Library stubs created successfully!
echo.
echo Now removing code signing from project files...

powershell -Command "Get-ChildItem -Filter '*.csproj' -Recurse | ForEach-Object { $content = Get-Content $_.FullName -Raw; $content = $content -replace '<SignAssembly>true</SignAssembly>', '<SignAssembly>false</SignAssembly>'; $content = $content -replace '<AssemblyOriginatorKeyFile>.*?</AssemblyOriginatorKeyFile>', ''; Set-Content -Path $_.FullName -Value $content }"

echo.
echo Done! Now open the solution in Visual Studio.
echo.
echo Next steps:
echo 1. In Visual Studio, create a new Class Library project called "MPLibraryStubs"
echo 2. Add all the .cs files from the MPLibraryStubs folder
echo 3. Remove broken references to ..\vdo_commons\MP* projects
echo 4. Add reference to your new MPLibraryStubs project
echo 5. Build the solution
echo.
pause