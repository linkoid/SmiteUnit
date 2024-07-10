using NUnit.Framework;
using SmiteLib.Tests.TestProgram;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Tests;

public class SmiteRunnerTests
{
	private SmiteProcess process;

	[SetUp]
	public void SetUp()
	{
		TestContext.WriteLine("SetUp");
		process = new SmiteProcess("SmiteLib.Tests.TestProgram.exe")
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
		process.RunTest(SmiteId.Method(typeof(SmiteRunnerTestsEdge), "NonExistentMethod"));
		Assert.IsNotEmpty(process.Error.ReadToEnd());
	}

	[Test]
	public void LoggerOk()
	{
		process.RunTest(
			SmiteId.Method(SmiteRunnerTestsEdge.LoggerOk)
		);
		Assert.IsNotEmpty(process.Output.ReadToEnd());
	}


	[Test]
	public void ExitCodeOnException()
	{
		process.RunTest(
			SmiteId.Method(SmiteRunnerTestsEdge.ThrowException)
		);
		Assert.IsNotEmpty(process.Error.ReadToEnd());
		Assert.That(process.ExitCode, Is.Not.EqualTo(0));
	}
}
