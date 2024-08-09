using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Logging;

public abstract class SmiteLogger : ILogger
{
	public static readonly SmiteLogger Default = new StandardLogger();

	public static SmiteLogger Current = Default;

	internal SmiteLogger() { }

	public void Log(LogLevel logLevel, object? data)
	{
		if (logLevel >= LogLevel.None)
			return;

		string stringData = data?.ToString() ?? "null";

		if (logLevel >= LogLevel.Error)
		{
			WriteError(stringData);
		}
		else
		{
			WriteOutput(stringData);
		}
	}

	protected abstract void WriteOutput(string message);

	protected abstract void WriteError(string message);
}
