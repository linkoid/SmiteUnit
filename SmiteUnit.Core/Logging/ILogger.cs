using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Logging;

public interface ILogger
{
	public void Log(LogLevel logLevel, object? data);
}

public static class ILoggerExtensions
{
	public static void LogTrace   (this ILogger logger, object? data) => logger.Log(LogLevel.Trace   , data);
	public static void LogDebug   (this ILogger logger, object? data) => logger.Log(LogLevel.Debug   , data);
	public static void LogInfo    (this ILogger logger, object? data) => logger.Log(LogLevel.Info    , data);
	public static void LogWarning (this ILogger logger, object? data) => logger.Log(LogLevel.Warning , data);
	public static void LogError   (this ILogger logger, object? data) => logger.Log(LogLevel.Error   , data);
	public static void LogCritical(this ILogger logger, object? data) => logger.Log(LogLevel.Critical, data);

	public static void LogException(this ILogger logger, Exception exception, string? message = null)
	{
		object fullMessage = message != null ? $"{message}\n{exception}" : exception;
		logger.Log(LogLevel.Error, fullMessage);
	}
}
