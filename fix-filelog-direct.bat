@echo off

echo Directly fixing FileLog.cs...



del "C:\VdO_Progetto\MPLibraryStubs\FileLog.cs"



echo Creating clean FileLog.cs without any VdO2013Core references...

echo using System; > C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo using System.Windows.Forms; >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo using System.Reflection; >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo public class FileLog >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo { >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     private static FileLog _default = new FileLog(); >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static FileLog Default { get { return _default; } } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public string FileName { get; set; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static bool Initialized { get; set; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static bool CreateSubfolderPerDay { get; set; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static bool ReplicateOnConsole { get; set; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static int RemoveSubfolderPerDay { get; set; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static int OlderThan { get; set; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public delegate Form GetMainFormDelegate(); >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static GetMainFormDelegate GetMainForm { get; set; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public FileLog() { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public FileLog(string fileName) { FileName = fileName; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static void Initialize(string path) { Initialized = true; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static void Initialize(string path, bool createSubfolderPerDay) { Initialized = true; CreateSubfolderPerDay = createSubfolderPerDay; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static void Initialize(string path, string appName) { Initialized = true; } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void Log(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void LogError(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void LogWarning(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void LogDebug(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void Write(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteLine(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteLine(LogKind kind, string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteLine(string message, LogKind kind) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteCtor(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteCtor(object obj) { WriteCtor(obj?.GetType().Name ?? "null"); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteCtor(Type type) { WriteCtor(type?.Name ?? "null"); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(MethodBase method) { WriteMethod(method?.Name ?? "unknown"); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(MethodBase method, string message) { WriteMethod(method?.Name + ": " + message); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(string format, object arg0) { WriteMethod(string.Format(format, arg0)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(string format, object arg0, object arg1) { WriteMethod(string.Format(format, arg0, arg1)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(string format, object arg0, object arg1, object arg2) { WriteMethod(string.Format(format, arg0, arg1, arg2)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(string format, object arg0, object arg1, object arg2, object arg3) { WriteMethod(string.Format(format, arg0, arg1, arg2, arg3)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(string format, object arg0, object arg1, object arg2, object arg3, object arg4) { WriteMethod(string.Format(format, arg0, arg1, arg2, arg3, arg4)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethod(string format, params object[] args) { WriteMethod(string.Format(format, args)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(MethodBase method) { WriteMethodResult(method?.Name ?? "unknown"); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(MethodBase method, string message) { WriteMethodResult(method?.Name + ": " + message); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(string format, object arg0) { WriteMethodResult(string.Format(format, arg0)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(string format, object arg0, object arg1) { WriteMethodResult(string.Format(format, arg0, arg1)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(string format, object arg0, object arg1, object arg2) { WriteMethodResult(string.Format(format, arg0, arg1, arg2)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(string format, object arg0, object arg1, object arg2, object arg3) { WriteMethodResult(string.Format(format, arg0, arg1, arg2, arg3)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(string format, object arg0, object arg1, object arg2, object arg3, object arg4) { WriteMethodResult(string.Format(format, arg0, arg1, arg2, arg3, arg4)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(string format, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5) { WriteMethodResult(string.Format(format, arg0, arg1, arg2, arg3, arg4, arg5)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteMethodResult(string format, params object[] args) { WriteMethodResult(string.Format(format, args)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteInfo(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteInfo(string format, object arg0) { WriteInfo(string.Format(format, arg0)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteInfo(string format, object arg0, object arg1) { WriteInfo(string.Format(format, arg0, arg1)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteInfo(string format, object arg0, object arg1, object arg2) { WriteInfo(string.Format(format, arg0, arg1, arg2)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteInfo(string format, object arg0, object arg1, object arg2, object arg3) { WriteInfo(string.Format(format, arg0, arg1, arg2, arg3)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteInfo(string format, params object[] args) { WriteInfo(string.Format(format, args)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(Exception ex) { WriteError(ex?.Message ?? "unknown error"); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(string message, Exception ex) { WriteError(message + ": " + ex?.Message); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(Exception ex, string message) { WriteError(message + ": " + ex?.Message); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(string format, object arg0) { WriteError(string.Format(format, arg0)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(string format, object arg0, object arg1) { WriteError(string.Format(format, arg0, arg1)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(string format, object arg0, object arg1, object arg2) { WriteError(string.Format(format, arg0, arg1, arg2)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(string format, object arg0, object arg1, object arg2, object arg3) { WriteError(string.Format(format, arg0, arg1, arg2, arg3)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(string format, object arg0, object arg1, object arg2, object arg3, object arg4) { WriteError(string.Format(format, arg0, arg1, arg2, arg3, arg4)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteError(string format, params object[] args) { WriteError(string.Format(format, args)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteWarn(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteWarn(string format, object arg0) { WriteWarn(string.Format(format, arg0)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteWarn(string format, object arg0, object arg1) { WriteWarn(string.Format(format, arg0, arg1)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteWarn(string format, object arg0, object arg1, object arg2) { WriteWarn(string.Format(format, arg0, arg1, arg2)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteWarn(string format, object arg0, object arg1, object arg2, object arg3) { WriteWarn(string.Format(format, arg0, arg1, arg2, arg3)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteWarn(string format, params object[] args) { WriteWarn(string.Format(format, args)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(string message) { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(MethodBase method) { WriteDebug(method?.Name ?? "unknown"); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(MethodBase method, string message) { WriteDebug(method?.Name + ": " + message); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(Exception ex) { WriteDebug(ex?.Message ?? "unknown error"); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(Exception ex, string message) { WriteDebug(message + ": " + ex?.Message); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(string format, object arg0) { WriteDebug(string.Format(format, arg0)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(string format, object arg0, object arg1) { WriteDebug(string.Format(format, arg0, arg1)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(string format, object arg0, object arg1, object arg2) { WriteDebug(string.Format(format, arg0, arg1, arg2)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public void WriteDebug(string format, params object[] args) { WriteDebug(string.Format(format, args)); } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo public class FileLogUI >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo { >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo     public static void Initialize() { } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo. >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs

echo public enum LogKind { Info, Warning, Error, Debug } >> C:\VdO_Progetto\MPLibraryStubs\FileLog.cs



echo.

echo FileLog.cs has been recreated without any VdO2013Core references.

echo.

echo Now rebuild:

echo 1. Right-click MPLibraryStubs - Ricompila (Rebuild)

echo 2. If successful, right-click VdO2013Core - Ricompila (Rebuild)

pause