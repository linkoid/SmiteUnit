using NUnit.Framework;
using SmiteLib.Engine;
using SmiteLib.Tests.TestProgram;
using System;

namespace SmiteLib.Tests;

public class AttributeTests
{
	private SmiteProcess process;

	[SetUp]
	public void SetUp()
	{
		TestContext.WriteLine(Environment.CurrentDirectory);
		process = new SmiteProcess("SmiteLib.Tests.TestProgram.exe")
		{
			UseSubprocess = true,
		};
	}

	[Test]
	public void SetUpRunsFirst()
	{
		Console.WriteLine(TestContext.CurrentContext.Test.FullName);
		process.RunTest(SmiteId.Method(AttributeTestsEdge.Test));
		Assert.That(process.Output.ReadToEnd().StartsWith("SetUp"));

	}
}
