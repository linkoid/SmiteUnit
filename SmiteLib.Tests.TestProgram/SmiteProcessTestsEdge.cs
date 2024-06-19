using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Tests.TestProgram;

internal static class SmiteProcessTestsEdge
{
	[SmiteMethod]
	public static void PrintManyLines()
	{
		PrintManyLinesUsing(Console.Out);
		PrintManyLinesUsing(Console.Error);
	}

	public static void PrintManyLinesUsing(TextWriter writer)
	{
		for (int i = 0; i < 50; i++)
		{
			writer.WriteLine($"Line {i}");
		}
		writer.Flush();
	}
}
