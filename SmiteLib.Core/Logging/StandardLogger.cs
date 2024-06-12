using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Logging;

internal class StandardLogger : SmiteLogger
{
	public readonly StreamWriter OutputStream;
	public readonly StreamWriter ErrorStream;

	public StandardLogger()
	{
		OutputStream = new(Console.OpenStandardOutput())
		{
			AutoFlush = true,
		};

		ErrorStream = new(Console.OpenStandardError())
		{
			AutoFlush = true,
		};
	}

	protected override void WriteOutput(string message)
	{
		OutputStream.WriteLine(message);
	}

	protected override void WriteError(string message)
	{
		ErrorStream.WriteLine(message);
	}
}
