using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Tests.TestProgram;

internal class CommandLineParserTestsEdge
{
	[SmiteMethod]
	public static void PrintArguments()
	{
		Console.Write(string.Join(",", Environment.GetCommandLineArgs()));
	}
}
