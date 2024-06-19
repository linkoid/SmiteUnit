using NUnit.Framework;
using SmiteLib.Tests.TestProgram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
}
