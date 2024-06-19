using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Tests.TestProgram;

internal static class SmiteIdTestsEdge
{
	[SmiteMethod]
	public static void ValidSmiteMethod()
	{
		Console.WriteLine("Ok!");
	}


	public static void InvalidSmiteMethod()
	{
		Console.WriteLine("Not ok!");
	}
}
