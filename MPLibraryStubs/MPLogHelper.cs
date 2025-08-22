using System;
using System.Collections.Generic;

public static class MPLogHelper
{
    public static void Log(string message) { Console.WriteLine("[LOG] " + message); }
    public static void LogError(string message) { Console.WriteLine("[ERROR] " + message); }
    public static void LogWarning(string message) { Console.WriteLine("[WARN] " + message); }
    public static void LogDebug(string message) { Console.WriteLine("[DEBUG] " + message); }

    public static class UI
    {
        public static void ShowMessage(string message) { }
        public static void ShowError(string message) { }
        public static void ShowWarning(string message) { }
        public static bool ShowQuestion(string message) { return true; }
    }
}
