using NUnit.Framework;
using SmiteUnit.Engine;
using SmiteUnit.Tests.TestProgram;
using System;

namespace SmiteUnit.Tests;

public class SmiteInjectionTests
{
	private SmiteProcess process;

	[SetUp]
	public void SetUp()
	{
		TestContext.WriteLine("SetUp");
		process = new SmiteProcess(Variables.TestProgram)
		{
			UseSubprocess = true
		};
	}

	[Test]
	public void ParseIdError()
	{
		process.RunTest(SmiteId.String("invalid:smiteid"));
		Assert.IsNotEmpty(process.Error.ReadToEnd());
	}

	[Test]
	public void MissingMethodError()
	{
		process.RunTest(SmiteId.Method(typeof(SmiteInjectionTestsEdge), "NonExistentMethod"));
		Assert.IsNotEmpty(process.Error.ReadToEnd());
	}

	[Test]
	public void LoggerOk()
	{
		process.RunTest(
			SmiteId.Method(SmiteInjectionTestsEdge.LoggerOk)
		);
		Assert.IsNotEmpty(process.Output.ReadToEnd());
	}


	[Test]
	public void ExitCodeOnException()
	{
		process.RunTest(
			SmiteId.Method(SmiteInjectionTestsEdge.ThrowException)
		);
		Assert.IsNotEmpty(process.Error.ReadToEnd());
		Assert.That(process.ExitCode, Is.Not.EqualTo(0));
	}
}
