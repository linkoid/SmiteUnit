using SmiteUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Tests.TestProgram;

internal class TestContextTestsEdge
{
	[SmiteMethod]
	public static void Fail()
	{
		TestContext.Fail("Failed successfully");
	}

	[SmiteMethod]
	public static void WrapEventHandler()
	{
		Program.PostTestsEvent += TestContext.WrapEventHandler((_, _) =>
		{
			Console.WriteLine("Wrapped delegate ran");
		});
	}

	[SmiteMethod]
	public static void SetUnfinished()
	{
		Program.PostLoopEvent += TestContext.WrapEventHandler((_, _) =>
		{
			Console.WriteLine("Set Finished");
			TestContext.Finished();
		});

		Console.WriteLine("Set Unfinished");
		TestContext.Unfinished();
	}
}
