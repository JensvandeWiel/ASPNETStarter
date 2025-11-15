using System.Text.Json;
using Serilog;
using Serilog.Context;
using Serilog.Sinks.XUnit3;

namespace Tests.Helpers;

/// <summary>
///     Centralized logging utility for integration tests with Serilog
/// </summary>
public static class TestLogger
{
    private static ILogger? _logger;
    private static readonly object Lock = new();

    /// <summary>
    ///     Gets the logger instance
    /// </summary>
    public static ILogger Logger => _logger ??
                                    throw new InvalidOperationException(
                                        "TestLogger not initialized. Call Initialize() first.");

    /// <summary>
    ///     Initializes the test logger with console and file output
    /// </summary>
    public static void Initialize()
    {
        lock (Lock)
        {
            if (_logger != null)
                return;

            // Create logs directory if it doesn't exist
            var logsDir = Path.Combine(AppContext.BaseDirectory, "logs");
            Directory.CreateDirectory(logsDir);

            _logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Destructure.ToMaximumDepth(3)
                .Enrich.FromLogContext()
                .Enrich.WithProperty("TestEnvironment", "Integration")
                .WriteTo.File(
                    Path.Combine(logsDir, "tests-.txt"),
                    rollingInterval: RollingInterval.Day,
                    retainedFileCountLimit: 10,
                    outputTemplate:
                    "[TEST] [{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.XUnit3TestOutput(
                    outputTemplate:
                    "[TEST] [{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz}] [{Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();

            Log.Logger = _logger;
        }
    }
    /// <summary>
    ///     Closes and flushes the logger
    /// </summary>
    public static void Close()
    {
        Log.CloseAndFlush();
        lock (Lock)
        {
            _logger = null;
        }
    }
}