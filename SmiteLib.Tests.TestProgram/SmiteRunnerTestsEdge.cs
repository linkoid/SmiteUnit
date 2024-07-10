using SmiteLib.Framework;
using SmiteLib.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Tests.TestProgram;

internal static class SmiteRunnerTestsEdge
{
	[SmiteMethod]
	public static void HelloWorld()
	{
		Console.WriteLine("Hello World!");
	}

	[SmiteMethod]
	public static void LoggerOk()
	{
		SmiteLogger.Current.LogInfo("Ok");
	}

	[SmiteMethod]
	public static void ThrowException()
	{
		throw new Exception("This test always throws an exception");
	}
}
