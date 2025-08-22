using System;

public static class MPFingerPrint
{
    public static string GetFingerprint() => Guid.NewGuid().ToString();
}

public enum FingerPrintParts
{
    None = 0,
    ProcessorId = 1,
    HardDiskId = 2,
    NetworkAdapterId = 4,
    All = 7
}