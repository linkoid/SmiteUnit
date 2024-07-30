using NUnit.Framework;
using SmiteLib.Engine;
using SmiteLib.Internal;
using SmiteLib.Tests.TestProgram;
using System;

namespace SmiteLib.Tests;

public class SmiteIdTests
{
	[Test]
	public void ValidSmiteMethod()
	{
		var process = new SmiteProcess("SmiteLib.Tests.TestProgram.exe");
		process.RunTest(SmiteId.Method(SmiteIdTestsEdge.ValidSmiteMethod));
	}

	[Test]
	public void InvalidSmiteMethodThrows()
	{
		var process = new SmiteProcess("SmiteLib.Tests.TestProgram.exe");
		try
		{
			var targetId = SmiteId.Method(SmiteIdTestsEdge.InvalidSmiteMethod);
			process.RunTest(targetId);
			Assert.Fail($"Attempt to target {targetId} did not raise exception");
		}
		catch (Exception ex)
		{
			TestContext.WriteLine(ex);
		}
	}
}