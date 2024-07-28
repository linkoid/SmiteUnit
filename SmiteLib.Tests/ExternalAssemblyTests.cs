using NUnit.Framework;
using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Tests;

public class ExternalAssemblyTests
{
	private SmiteProcess process;

	[SetUp]
	public void SetUp()
	{
		process = new SmiteProcess("SmiteLib.Tests.TestProgram.exe")
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
