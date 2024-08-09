using SmiteUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Tests.TestProgram;

internal class CommandLineParserTestsEdge
{
	[SmiteMethod]
	public static void PrintArguments()
	{
		Console.Write(string.Join(",", Environment.GetCommandLineArgs()));
	}
}
