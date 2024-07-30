using NUnit.Framework;
using SmiteLib.Engine;
using SmiteLib.Tests.TestProgram;
using System;
using System.Linq;

namespace SmiteLib.Tests;

public class TestContextTests
{
	private SmiteProcess process;

	[SetUp]
	public void SetUp()
	{
		process = new SmiteProcess("SmiteLib.Tests.TestProgram.exe")
		{
			UseSubprocess = true
		};
	}

	[Test]
	public void Fail()
	{
		process.RunTest(SmiteId.Method(TestContextTestsEdge.Fail));
		Assert.That(process.Error.ReadToEnd().Contains("Failed successfully"));
	}

	[Test]
	public void WrapDelegate()
	{
		process.RunTest(SmiteId.Method(TestContextTestsEdge.WrapEventHandler));
		Assert.That(process.Output.ReadToEnd().Contains("Wrapped delegate ran"));
	}

	[Test]
	public void SetUnfinished()
	{
		process.RunTest(SmiteId.Method(TestContextTestsEdge.SetUnfinished));
		Assert.That(process.Output.ReadToEnd().Contains("Set Finished"));
	}
}
