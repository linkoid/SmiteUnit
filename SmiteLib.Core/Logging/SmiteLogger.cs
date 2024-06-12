using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Logging;

public abstract class SmiteLogger : ILogger
{
	public static readonly SmiteLogger Default = new StandardLogger();

	public static SmiteLogger Current = Default;

	internal SmiteLogger() { }

	public void Log(LogLevel logLevel, string message)
	{
		if (logLevel >= LogLevel.None)
			return;

		if (logLevel >= LogLevel.Error)
		{
			WriteError(message);
		}
		else
		{
			WriteOutput(message);
		}
	}

	protected abstract void WriteOutput(string message);

	protected abstract void WriteError(string message);
}
