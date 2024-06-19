using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Tests.TestProgram;

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
}
