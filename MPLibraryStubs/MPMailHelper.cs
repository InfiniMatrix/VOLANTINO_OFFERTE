using System;
public static class MPMailHelper
{
    public static void SendMail(string to, string subject, string body) { }
    public static bool ValidateEmail(string email) => email.Contains("@");
}
