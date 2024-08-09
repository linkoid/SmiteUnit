using NUnit.Framework;
using SmiteUnit.Engine;
using SmiteUnit.Internal;
using SmiteUnit.Tests.TestProgram;
using System;

namespace SmiteUnit.Tests;

public class SmiteIdTests
{
	[Test]
	public void ValidSmiteMethod()
	{
		var process = new SmiteProcess("SmiteUnit.Tests.TestProgram.exe");
		process.RunTest(SmiteId.Method(SmiteIdTestsEdge.ValidSmiteMethod));
	}

	[Test]
	public void InvalidSmiteMethodThrows()
	{
		var process = new SmiteProcess("SmiteUnit.Tests.TestProgram.exe");
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