using System;
using System.Windows.Forms;
using System.Reflection;

public class FileLog
{
    private static FileLog _default = new FileLog();
    public static FileLog Default { get { return _default; } }

    public string FileName { get; set; }
    public static bool Initialized { get; set; }
    public static bool CreateSubfolderPerDay { get; set; }
    public static bool ReplicateOnConsole { get; set; }
    public static int RemoveSubfolderPerDay { get; set; }
    public static int OlderThan { get; set; }

    public delegate Form GetMainFormDelegate();
    public static GetMainFormDelegate GetMainForm { get; set; }

    public FileLog() { }
    public FileLog(string fileName) { FileName = fileName; }
    public FileLog(object obj) { FileName = obj?.ToString() ?? "object"; }

    public static void Initialize(string path) { Initialized = true; }
    public static void Initialize(string path, bool createSubfolderPerDay) { Initialized = true; CreateSubfolderPerDay = createSubfolderPerDay; }
    public static void Initialize(string path, string appName) { Initialized = true; }

    // Basic methods
    public void Log(string message) { }
    public void LogError(string message) { }
    public void LogWarning(string message) { }
    public void LogDebug(string message) { }
    public void Write(string message) { }
    public void WriteLine(string message) { }
    public void WriteLine(LogKind kind, string message) { }
    public void WriteLine(string message, LogKind kind) { }

    // WriteCtor - GENERIC OBJECT OVERLOAD
    public void WriteCtor(string message) { }
    public void WriteCtor(object obj) { WriteCtor(obj?.GetType().Name ?? "null"); }
    public void WriteCtor(Type type) { WriteCtor(type?.Name ?? "null"); }

    // WriteMethod - ALL OVERLOADS INCLUDING METHODBASE
    public void WriteMethod(string message) { }
    public void WriteMethod(MethodBase method) { WriteMethod(method?.Name ?? "unknown"); }
    public void WriteMethod(MethodBase method, string message) { WriteMethod(method?.Name + ": " + message); }
    public void WriteMethod(string format, object arg0) { WriteMethod(string.Format(format, arg0)); }
    public void WriteMethod(string format, object arg0, object arg1) { WriteMethod(string.Format(format, arg0, arg1)); }
    public void WriteMethod(string format, object arg0, object arg1, object arg2) { WriteMethod(string.Format(format, arg0, arg1, arg2)); }
    public void WriteMethod(string format, object arg0, object arg1, object arg2, object arg3) { WriteMethod(string.Format(format, arg0, arg1, arg2, arg3)); }
    public void WriteMethod(string format, object arg0, object arg1, object arg2, object arg3, object arg4) { WriteMethod(string.Format(format, arg0, arg1, arg2, arg3, arg4)); }
    public void WriteMethod(string format, params object[] args) { WriteMethod(string.Format(format, args)); }

    // WriteMethodResult - ALL OVERLOADS INCLUDING METHODBASE
    public void WriteMethodResult(string message) { }
    public void WriteMethodResult(MethodBase method) { WriteMethodResult(method?.Name ?? "unknown"); }
    public void WriteMethodResult(MethodBase method, string message) { WriteMethodResult(method?.Name + ": " + message); }
    public void WriteMethodResult(string format, object arg0) { WriteMethodResult(string.Format(format, arg0)); }
    public void WriteMethodResult(string format, object arg0, object arg1) { WriteMethodResult(string.Format(format, arg0, arg1)); }
    public void WriteMethodResult(string format, object arg0, object arg1, object arg2) { WriteMethodResult(string.Format(format, arg0, arg1, arg2)); }
    public void WriteMethodResult(string format, object arg0, object arg1, object arg2, object arg3) { WriteMethodResult(string.Format(format, arg0, arg1, arg2)); }
    public void WriteMethodResult(string format, object arg0, object arg1, object arg2, object arg3, object arg4) { WriteMethodResult(string.Format(format, arg0, arg1, arg2, arg3, arg4)); }
    public void WriteMethodResult(string format, object arg0, object arg1, object arg2, object arg3, object arg4, object arg5) { WriteMethodResult(string.Format(format, arg0, arg1, arg2, arg3, arg4, arg5)); }
    public void WriteMethodResult(string format, params object[] args) { WriteMethodResult(string.Format(format, args)); }

    // WriteInfo overloads
    public void WriteInfo(string message) { }
    public void WriteInfo(string format, object arg0) { WriteInfo(string.Format(format, arg0)); }
    public void WriteInfo(string format, object arg0, object arg1) { WriteInfo(string.Format(format, arg0, arg1)); }
    public void WriteInfo(string format, object arg0, object arg1, object arg2) { WriteInfo(string.Format(format, arg0, arg1, arg2)); }
    public void WriteInfo(string format, object arg0, object arg1, object arg2, object arg3) { WriteInfo(string.Format(format, arg0, arg1, arg2, arg3)); }
    public void WriteInfo(string format, params object[] args) { WriteInfo(string.Format(format, args)); }

    // WriteError overloads
    public void WriteError(string message) { }
    public void WriteError(Exception ex) { WriteError(ex?.Message ?? "unknown error"); }
    public void WriteError(string message, Exception ex) { WriteError(message + ": " + ex?.Message); }
    public void WriteError(Exception ex, string message) { WriteError(message + ": " + ex?.Message); }
    public void WriteError(string format, object arg0) { WriteError(string.Format(format, arg0)); }
    public void WriteError(string format, object arg0, object arg1) { WriteError(string.Format(format, arg0, arg1)); }
    public void WriteError(string format, object arg0, object arg1, object arg2) { WriteError(string.Format(format, arg0, arg1, arg2)); }
    public void WriteError(string format, object arg0, object arg1, object arg2, object arg3) { WriteError(string.Format(format, arg0, arg1, arg2, arg3)); }
    public void WriteError(string format, object arg0, object arg1, object arg2, object arg3, object arg4) { WriteError(string.Format(format, arg0, arg1, arg2, arg3, arg4)); }
    public void WriteError(string format, params object[] args) { WriteError(string.Format(format, args)); }

    // WriteWarn overloads
    public void WriteWarn(string message) { }
    public void WriteWarn(string format, object arg0) { WriteWarn(string.Format(format, arg0)); }
    public void WriteWarn(string format, object arg0, object arg1) { WriteWarn(string.Format(format, arg0, arg1)); }
    public void WriteWarn(string format, object arg0, object arg1, object arg2) { WriteWarn(string.Format(format, arg0, arg1, arg2)); }
    public void WriteWarn(string format, object arg0, object arg1, object arg2, object arg3) { WriteWarn(string.Format(format, arg0, arg1, arg2, arg3)); }
    public void WriteWarn(string format, params object[] args) { WriteWarn(string.Format(format, args)); }

    // WriteDebug overloads - INCLUDING ALL METHODBASE OVERLOADS
    public void WriteDebug(string message) { }
    public void WriteDebug(MethodBase method) { WriteDebug(method?.Name ?? "unknown"); }
    public void WriteDebug(MethodBase method, string message) { WriteDebug(method?.Name + ": " + message); }
    public void WriteDebug(Exception ex) { WriteDebug(ex?.Message ?? "unknown error"); }
    public void WriteDebug(Exception ex, string message) { WriteDebug(message + ": " + ex?.Message); }
    public void WriteDebug(string format, object arg0) { WriteDebug(string.Format(format, arg0)); }
    public void WriteDebug(string format, object arg0, object arg1) { WriteDebug(string.Format(format, arg0, arg1)); }
    public void WriteDebug(string format, object arg0, object arg1, object arg2) { WriteDebug(string.Format(format, arg0, arg1, arg2)); }
    public void WriteDebug(string format, params object[] args) { WriteDebug(string.Format(format, args)); }
}

public class FileLogUI 
{ 
    public static void Initialize() { }
}

public enum LogKind { Info, Warning, Error, Debug }
