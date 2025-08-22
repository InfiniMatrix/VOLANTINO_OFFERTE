@echo off

echo Fixing specific compilation errors...



echo.

echo 1. Updating FileLog.cs to accept object in constructor...

powershell -Command ^

"$file = 'C:\VdO_Progetto\MPLibraryStubs\FileLog.cs'; ^

$content = Get-Content $file -Raw; ^

if ($content -notmatch 'public FileLog\(object obj\)') { ^

    $content = $content -replace '(public FileLog\(string fileName\) { FileName = fileName; })', ^

    '$1`r`n    public FileLog(object obj) { FileName = obj?.ToString() ?? \"object\"; }'; ^

    Set-Content -Path $file -Value $content -Encoding UTF8; ^

    Write-Host 'Added FileLog(object) constructor'; ^

} else { ^

    Write-Host 'FileLog(object) constructor already exists'; ^

}"



echo.

echo 2. Let's check how OneWeek is being used and what type OlderThan is...

echo Searching for OlderThan declaration in Global.cs:

findstr /n "OlderThan" "C:\VdO_Progetto\VdO2013Core\Global.cs"



echo.

echo 3. Creating a comprehensive fix for ALL the issues...

(

echo using System;

echo using System.Windows.Forms;

echo using System.Reflection;

echo.

echo public class FileLog

echo {

echo     private static FileLog _default = new FileLog^(^);

echo     public static FileLog Default { get { return _default; } }

echo.

echo     public string FileName { get; set; }

echo     public static bool Initialized { get; set; }

echo     public static bool CreateSubfolderPerDay { get; set; }

echo     public static bool ReplicateOnConsole { get; set; }

echo     public static int RemoveSubfolderPerDay { get; set; }

echo     public static int OlderThan { get; set; }

echo.

echo     public delegate Form GetMainFormDelegate^(^);

echo     public static GetMainFormDelegate GetMainForm { get; set; }

echo.

echo     public FileLog^(^) { }

echo     public FileLog^(string fileName^) { FileName = fileName; }

echo     public FileLog^(object obj^) { FileName = obj?.ToString^(^) ?? "object"; }

echo.

echo     public static void Initialize^(string path^) { Initialized = true; }

echo     public static void Initialize^(string path, bool createSubfolderPerDay^) { Initialized = true; CreateSubfolderPerDay = createSubfolderPerDay; }

echo     public static void Initialize^(string path, string appName^) { Initialized = true; }

echo.

echo     // Basic methods

echo     public void Log^(string message^) { }

echo     public void LogError^(string message^) { }

echo     public void LogWarning^(string message^) { }

echo     public void LogDebug^(string message^) { }

echo     public void Write^(string message^) { }

echo     public void WriteLine^(string message^) { }

echo     public void WriteLine^(LogKind kind, string message^) { }

echo     public void WriteLine^(string message, LogKind kind^) { }

echo.

echo     // WriteCtor - GENERIC OBJECT OVERLOAD

echo     public void WriteCtor^(string message^) { }

echo     public void WriteCtor^(object obj^) { WriteCtor^(obj?.GetType^(^).Name ?? "null"^); }

echo     public void WriteCtor^(Type type^) { WriteCtor^(type?.Name ?? "null"^); }

echo.

echo     // WriteMethod - ALL OVERLOADS INCLUDING METHODBASE

echo     public void WriteMethod^(string message^) { }

echo     public void WriteMethod^(MethodBase method^) { WriteMethod^(method?.Name ?? "unknown"^); }

echo     public void WriteMethod^(MethodBase method, string message^) { WriteMethod^(method?.Name + ": " + message^); }

echo     public void WriteMethod^(string format, object arg0^) { WriteMethod^(string.Format^(format, arg0^)^); }

echo     public void WriteMethod^(string format, object arg0, object arg1^) { WriteMethod^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2^) { WriteMethod^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteMethod^(string.Format^(format, arg0, arg1, arg2, arg3^)^); }

echo     public void WriteMethod^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { WriteMethod^(string.Format^(format, arg0, arg1, arg2, arg3, arg4^)^); }

echo     public void WriteMethod^(string format, params object[] args^) { WriteMethod^(string.Format^(format, args^)^); }

echo.

echo     // WriteMethodResult - ALL OVERLOADS INCLUDING METHODBASE

echo     public void WriteMethodResult^(string message^) { }

echo     public void WriteMethodResult^(MethodBase method^) { WriteMethodResult^(method?.Name ?? "unknown"^); }

echo     public void WriteMethodResult^(MethodBase method, string message^) { WriteMethodResult^(method?.Name + ": " + message^); }

echo     public void WriteMethodResult^(string format, object arg0^) { WriteMethodResult^(string.Format^(format, arg0^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1^) { WriteMethodResult^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2^) { WriteMethodResult^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteMethodResult^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { WriteMethodResult^(string.Format^(format, arg0, arg1, arg2, arg3, arg4^)^); }

echo     public void WriteMethodResult^(string format, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5^) { WriteMethodResult^(string.Format^(format, arg0, arg1, arg2, arg3, arg4, arg5^)^); }

echo     public void WriteMethodResult^(string format, params object[] args^) { WriteMethodResult^(string.Format^(format, args^)^); }

echo.

echo     // WriteInfo overloads

echo     public void WriteInfo^(string message^) { }

echo     public void WriteInfo^(string format, object arg0^) { WriteInfo^(string.Format^(format, arg0^)^); }

echo     public void WriteInfo^(string format, object arg0, object arg1^) { WriteInfo^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteInfo^(string format, object arg0, object arg1, object arg2^) { WriteInfo^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteInfo^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteInfo^(string.Format^(format, arg0, arg1, arg2, arg3^)^); }

echo     public void WriteInfo^(string format, params object[] args^) { WriteInfo^(string.Format^(format, args^)^); }

echo.

echo     // WriteError overloads

echo     public void WriteError^(string message^) { }

echo     public void WriteError^(Exception ex^) { WriteError^(ex?.Message ?? "unknown error"^); }

echo     public void WriteError^(string message, Exception ex^) { WriteError^(message + ": " + ex?.Message^); }

echo     public void WriteError^(Exception ex, string message^) { WriteError^(message + ": " + ex?.Message^); }

echo     public void WriteError^(string format, object arg0^) { WriteError^(string.Format^(format, arg0^)^); }

echo     public void WriteError^(string format, object arg0, object arg1^) { WriteError^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2^) { WriteError^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteError^(string.Format^(format, arg0, arg1, arg2, arg3^)^); }

echo     public void WriteError^(string format, object arg0, object arg1, object arg2, object arg3, object arg4^) { WriteError^(string.Format^(format, arg0, arg1, arg2, arg3, arg4^)^); }

echo     public void WriteError^(string format, params object[] args^) { WriteError^(string.Format^(format, args^)^); }

echo.

echo     // WriteWarn overloads

echo     public void WriteWarn^(string message^) { }

echo     public void WriteWarn^(string format, object arg0^) { WriteWarn^(string.Format^(format, arg0^)^); }

echo     public void WriteWarn^(string format, object arg0, object arg1^) { WriteWarn^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteWarn^(string format, object arg0, object arg1, object arg2^) { WriteWarn^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteWarn^(string format, object arg0, object arg1, object arg2, object arg3^) { WriteWarn^(string.Format^(format, arg0, arg1, arg2, arg3^)^); }

echo     public void WriteWarn^(string format, params object[] args^) { WriteWarn^(string.Format^(format, args^)^); }

echo.

echo     // WriteDebug overloads - INCLUDING ALL METHODBASE OVERLOADS

echo     public void WriteDebug^(string message^) { }

echo     public void WriteDebug^(MethodBase method^) { WriteDebug^(method?.Name ?? "unknown"^); }

echo     public void WriteDebug^(MethodBase method, string message^) { WriteDebug^(method?.Name + ": " + message^); }

echo     public void WriteDebug^(Exception ex^) { WriteDebug^(ex?.Message ?? "unknown error"^); }

echo     public void WriteDebug^(Exception ex, string message^) { WriteDebug^(message + ": " + ex?.Message^); }

echo     public void WriteDebug^(string format, object arg0^) { WriteDebug^(string.Format^(format, arg0^)^); }

echo     public void WriteDebug^(string format, object arg0, object arg1^) { WriteDebug^(string.Format^(format, arg0, arg1^)^); }

echo     public void WriteDebug^(string format, object arg0, object arg1, object arg2^) { WriteDebug^(string.Format^(format, arg0, arg1, arg2^)^); }

echo     public void WriteDebug^(string format, params object[] args^) { WriteDebug^(string.Format^(format, args^)^); }

echo }

echo.

echo public class FileLogUI 

echo { 

echo     public static void Initialize^(^) { }

echo }

echo.

echo public enum LogKind { Info, Warning, Error, Debug }

) > C:\VdO_Progetto\MPLibraryStubs\FileLog.cs



echo.

echo 4. Now let's fix the VdO2013Core code to call OneWeek correctly...

echo.

echo The error "FileLog.OlderThan.OneWeek" should be "FileLog.OlderThan.OneWeek()"

echo You need to manually fix this in Visual Studio:

echo.

echo In Global.cs line 82, change:

echo    FileLog.RemoveSubfolderPerDay = FileLog.OlderThan.OneWeek;

echo To:

echo    FileLog.RemoveSubfolderPerDay = FileLog.OlderThan.OneWeek();

echo.

echo 5. For the System.Data.Entity error, you need to install Entity Framework:

echo    - Right-click on VdO2013Core project

echo    - Select "Gestisci pacchetti NuGet" (Manage NuGet Packages)

echo    - Search for "EntityFramework"

echo    - Install version 6.x (for .NET Framework 4.5.2)

echo.

echo After these changes:

echo 1. Rebuild MPLibraryStubs

echo 2. Fix the OneWeek() call in Global.cs

echo 3. Rebuild VdO2013Core

pause