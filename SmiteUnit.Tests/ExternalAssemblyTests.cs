using NUnit.Framework;
using SmiteUnit.Engine;
using SmiteUnit.Framework;
using System;
using System.Reflection;

namespace SmiteUnit.Tests;

public class ExternalAssemblyTests
{
	private SmiteProcess process;

	[SetUp]
	public void SetUp()
	{
		process = new SmiteProcess("SmiteUnit.Tests.TestProgram.exe")
		{
			WorkingDirectory = "."
		};
		Console.WriteLine($"Environment.CurrentDirectory = {Environment.CurrentDirectory}");
		Console.WriteLine($"Assembly.GetExecutingAssembly().GetName() = {Assembly.GetExecutingAssembly().GetName()}");
	}

	[Test]
	public void ExternalStaticMethod()
	{
		process.RunTest(SmiteId.Method(StaticMethod));

		Assert.IsNotEmpty(process.Output.ReadToEnd());
	}

	[Framework.SmiteMethod]
	static void StaticMethod()
	{
		Console.WriteLine("Ok");
	}

	[Test]
	public void InlineStaticMethod()
	{
		[Framework.SmiteMethod]
		static void InlineMethod()
		{
			Console.WriteLine("Ok");
		}
		process.RunTest(SmiteId.Method(InlineMethod));

		Assert.IsNotEmpty(process.Output.ReadToEnd());
	}

	[Test, Ignore("Not yet supported.")]
	public void AnonymousStaticMethod()
	{
		process.RunTest(SmiteId.Method([Framework.SmiteMethod] static () =>
		{
			Console.WriteLine("Ok");
		}));

		Assert.IsNotEmpty(process.Output.ReadToEnd());
	}
}
