using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Logging;

public interface ILogger
{
	public void Log(LogLevel logLevel, string message);
}

public static class ILoggerExtensions
{
	public static void LogTrace   (this ILogger logger, string message) => logger.Log(LogLevel.Trace   , message);
	public static void LogDebug   (this ILogger logger, string message) => logger.Log(LogLevel.Debug   , message);
	public static void LogInfo    (this ILogger logger, string message) => logger.Log(LogLevel.Info    , message);
	public static void LogWarning (this ILogger logger, string message) => logger.Log(LogLevel.Warning , message);
	public static void LogError   (this ILogger logger, string message) => logger.Log(LogLevel.Error   , message);
	public static void LogCritical(this ILogger logger, string message) => logger.Log(LogLevel.Critical, message);

	public static void LogException(this ILogger logger, Exception exception, string? message = null)
	{
		var fullMessage = message != null ? $"{message}\n{exception}" : $"{exception}";
		logger.Log(LogLevel.Error, fullMessage);
	}
}
