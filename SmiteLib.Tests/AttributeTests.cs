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

public class AttributeTests
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
	public void SetUpRunsFirst()
	{
		Console.WriteLine(TestContext.CurrentContext.Test.FullName);
		process.RunTest(SmiteId.Method(AttributeTestsEdge.Test));
		Assert.That(process.Output.ReadToEnd().StartsWith("SetUp"));

	}
}
