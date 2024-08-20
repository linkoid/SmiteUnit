using NUnit.Framework;
using SmiteUnit.Engine;
using SmiteUnit.Tests.TestProgram;
using System;

namespace SmiteUnit.Tests;

public class AttributeTests
{
	private SmiteProcess process;

	[SetUp]
	public void SetUp()
	{
		TestContext.WriteLine(Environment.CurrentDirectory);
		process = new SmiteProcess(Variables.TestProgram)
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
