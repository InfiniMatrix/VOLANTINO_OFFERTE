using System;
using System.Drawing;
using System.Linq;
using System.Reflection;

public static class MPExtensionMethods
{
    // String extensions
    public static bool IsNullOrEmpty(this string value) { return string.IsNullOrEmpty(value); }
    public static string SafeTrim(this string value) { return value?.Trim() ?? string.Empty; }
    public static string FormatWith(this string format, params object[] args) { return string.Format(format, args ?? new object[0]); }
    public static string Left(this string value, int length) { return value?.Length > length ? value.Substring(0, length) : value ?? ""; }
    public static string Right(this string value, int length) { return value?.Length > length ? value.Substring(value.Length - length) : value ?? ""; }
    public static int ReverseIndexOf(this string value, char c) { return value?.LastIndexOf(c) ?? -1; }
    public static int ReverseIndexOf(this string value, string s) { return value?.LastIndexOf(s) ?? -1; }
    public static int ReverseIndexOf(this string value, string s, int startIndex) { return value?.LastIndexOf(s, startIndex) ?? -1; }
    public static string ReverseSubstring(this string value, int startIndex) { return value?.Substring(startIndex) ?? ""; }
    public static string ReverseSubstring(this string value, int startIndex, int length) { return value?.Substring(startIndex, length) ?? ""; }
    public static string Quote(this string value) { return "\"" + value + "\""; }
    public static bool IsInt32(this string value) { int result; return int.TryParse(value, out result); }
    public static int ToInt32(this string value) { return int.Parse(value ?? "0"); }
    public static string EscapeSpecialChar(this string value) { return value?.Replace("\\", "\\\\").Replace("\"", "\\\"") ?? ""; }
    public static string UnescapeSpecialChar(this string value) { return value?.Replace("\\\"", "\"").Replace("\\\\", "\\") ?? ""; }
    public static string ToTitleCase(this string value) { return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value?.ToLower() ?? ""); }
    public static Color FromHexValue(this string value) { return System.Drawing.ColorTranslator.FromHtml(value); }
    public static string ToHexValue(this Color color) { return System.Drawing.ColorTranslator.ToHtml(color); }
    public static string AppendCRLF(this string value) { return value + "\r\n"; }
    public static string Replace(this string value, string oldValue, string newValue, StringComparison comparison) { return value?.Replace(oldValue, newValue) ?? ""; }

    // String Split extensions - FIX for StringSplitOptions
    public static string[] Split(this string str, char separator, StringSplitOptions options) 
    { 
        return str?.Split(new char[] { separator }, options) ?? new string[0]; 
    }

    // Numeric extensions
    public static string Indent(this int level) { return new string(' ', level * 4); }
    public static int OneWeek(this int value) { return value * 7; }

    // Enum extensions
    public static bool In<T>(this T value, params T[] values) where T : struct { return values != null && values.Contains(value); }

    // Assembly extensions
    public static string[] GetRecursiveReferencedAssemblyNames(this Assembly assembly) { return new string[0]; }
    public static AssemblyName GetRecursiveReferencedAssemblyNames(this Assembly assembly, string name) { return new AssemblyName(name); }

    // String array extensions
    public static int IndexOf(this string[] array, string value) { return Array.IndexOf(array, value); }
}

// Exception extension - FIX for Exception.Exception
public static class ExceptionExtensions
{
    public static Exception Exception(this Exception ex) { return ex; }
}
