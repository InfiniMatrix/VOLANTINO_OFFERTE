using System;
using System.Windows.Forms;

public static class MPExceptionHelper
{
    public static void HandleException(Exception ex) { Console.WriteLine(ex.Message); }
    public static void HandleException(Exception ex, string message) { Console.WriteLine(message + ": " + ex.Message); }
    public static void HandleException(Exception ex, bool useUI) { HandleException(ex); }
    public static string GetExceptionDetails(Exception ex) { return ex.ToString(); }

    public static class UI
    {
        public static void ShowException(Exception ex) { }
        public static void ShowException(Exception ex, string title) { }
        public static void LogException(Exception ex) { }
    }

    public static class ExceptionHelper
    {
        public delegate void OnExceptionDelegate(Exception ex);
        public delegate Form GetMainFormDelegate();

        public static OnExceptionDelegate OnException { get; set; }
        public static GetMainFormDelegate GetMainForm { get; set; }

        public static void Initialize() { }
        public static void Initialize(bool useUI) { }
        public static void HandleException(Exception ex) { }
        public static string GetDetails(Exception ex) { return ex.ToString(); }
    }
}
