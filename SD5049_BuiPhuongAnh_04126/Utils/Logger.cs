using System;

namespace Unsplash.Automation.Tests.Utils;

public enum LogLevel
{
    Debug = 1,
    Info = 2,
    Warn = 3,
    Error = 4,
    None = 5
}

/// <summary>
/// Simple static logger for test runs. Control verbosity via <see cref="Level"/>.
/// </summary>
public static class Logger
{
    public static LogLevel Level { get; set; } = LogLevel.Info;

    private static void Write(LogLevel level, string message)
    {
        if (level < Level || Level == LogLevel.None) return;
        var ts = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
        Console.WriteLine($"[{ts}] [{level}] {message}");
    }

    public static void Debug(string message) => Write(LogLevel.Debug, message);
    public static void Info(string message) => Write(LogLevel.Info, message);
    public static void Warn(string message) => Write(LogLevel.Warn, message);
    public static void Error(string message) => Write(LogLevel.Error, message);
}
