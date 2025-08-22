@echo off
echo Fixing nested classes in MPLibraryStubs...

echo Creating updated MPLogHelper.cs with nested classes...
(
echo using System;
echo using System.Collections.Generic;
echo.
echo public static class MPLogHelper
echo {
echo     public static void Log^(string message^) { Console.WriteLine^("[LOG] " + message^); }
echo     public static void LogError^(string message^) { Console.WriteLine^("[ERROR] " + message^); }
echo     public static void LogWarning^(string message^) { Console.WriteLine^("[WARN] " + message^); }
echo     public static void LogDebug^(string message^) { Console.WriteLine^("[DEBUG] " + message^); }
echo.
echo     public static class FileLog
echo     {
echo         public static void Log^(string message^) { }
echo         public static void LogError^(string message^) { }
echo         public static void LogWarning^(string message^) { }
echo         public static void LogDebug^(string message^) { }
echo         public static void Write^(string message^) { }
echo         public static void WriteLine^(string message^) { }
echo     }
echo.
echo     public static class UI
echo     {
echo         public static void ShowMessage^(string message^) { }
echo         public static void ShowError^(string message^) { }
echo         public static void ShowWarning^(string message^) { }
echo         public static bool ShowQuestion^(string message^) { return true; }
echo     }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\MPLogHelper.cs

echo Creating updated MPExceptionHelper.cs with nested classes...
(
echo using System;
echo.
echo public static class MPExceptionHelper
echo {
echo     public static void HandleException^(Exception ex^) { Console.WriteLine^(ex.Message^); }
echo     public static string GetExceptionDetails^(Exception ex^) { return ex.ToString^(^); }
echo.
echo     public static class UI
echo     {
echo         public static void ShowException^(Exception ex^) { }
echo         public static void ShowException^(Exception ex, string title^) { }
echo         public static void LogException^(Exception ex^) { }
echo     }
echo.
echo     public static class ExceptionHelper
echo     {
echo         public static void HandleException^(Exception ex^) { }
echo         public static string GetDetails^(Exception ex^) { return ex.ToString^(^); }
echo     }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\MPExceptionHelper.cs

echo Creating updated MPUtils.cs with nested classes...
(
echo using System;
echo using System.Collections.Generic;
echo.
echo public static class MPUtils
echo {
echo     public static string GetAppPath^(^) { return AppDomain.CurrentDomain.BaseDirectory; }
echo     public static string GetTempPath^(^) { return System.IO.Path.GetTempPath^(^); }
echo     public static bool IsNullOrEmpty^(string value^) { return string.IsNullOrEmpty^(value^); }
echo.
echo     public static class PluginFactory
echo     {
echo         public static object CreatePlugin^(Type type^) { return Activator.CreateInstance^(type^); }
echo         public static T CreatePlugin^<T^>^(^) where T : new^(^) { return new T^(^); }
echo     }
echo.
echo     public static class TextRes
echo     {
echo         public static string GetString^(string key^) { return key; }
echo         public static string GetString^(string key, params object[] args^) { return string.Format^(key, args^); }
echo     }
echo }
) > C:\VdO_Progetto\MPLibraryStubs\MPUtils.cs

echo Removing standalone FileLog.cs since it's now nested...
del C:\VdO_Progetto\MPLibraryStubs\FileLog.cs 2>nul

echo.
echo Done! Now rebuild the MPLibraryStubs project in Visual Studio.
pause