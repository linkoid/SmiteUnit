using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.VisualStudio.TestAdapter;

internal static class InternalLogger
{
	public static IMessageLogger? Handle;

	public static void LogInfo(string message)
	{
		SendMessage(TestMessageLevel.Informational, message);
	}

	public static void LogError(string message)
	{
		SendMessage(TestMessageLevel.Warning, message);
	}

	public static void LogWarning(string message)
	{
		SendMessage(TestMessageLevel.Error, message);
	}

	[Conditional("DEBUG")]
	public static void LogDebug(string message)
	{
		SendMessage(TestMessageLevel.Informational, message);
	}

	[Conditional("DEBUG")]
	public static void LogValue<T>(T value, [CallerArgumentExpression(nameof(value))] string valueExpression = "value")
	{
		SendMessage(TestMessageLevel.Informational, $"{valueExpression} = {value}");
	}

	private static void SendMessage(TestMessageLevel testMessageLevel, string message)
	{
		Handle?.SendMessage(testMessageLevel, $"SmiteUnit Adapter: {message}");
	}
}
