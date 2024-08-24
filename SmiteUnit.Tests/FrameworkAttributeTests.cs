using SmiteUnit.Engine;
using SmiteUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace SmiteUnit.Tests;

[SmiteProcess(Variables.TEST_PROGRAM_VARIABLE)]
public class FrameworkAttributeTests
{
	[SmiteSetUp]
	public static void SetUp()
	{
		Console.WriteLine("SetUp");
	}

	[SmiteTest]
	public static void SmiteTestMethod()
	{
		Console.WriteLine("Ok");
	}

	[SmiteTest]
	[ExpectExitCode(1)]
	public static void ExpectExitCode1()
	{
		Environment.Exit(1);
	}

	[SmiteTest]
	[ExpectExitCode(1)]
	[SmiteProcess(OutputEncoding = nameof(Encoding.Unicode))]
	public static void ExplicitEncoding()
	{
		Environment.Exit(1);
	}

	[CustomSmiteTest]
	public static void CustomSmiteTestMethod()
	{
		Console.WriteLine("Ok");
	}

	const string k_importantArgument = "important-argument";
	[SmiteTest, SmiteProcess(Arguments = k_importantArgument)]
	public static void ProcessAttributeArguments()
	{
		if (!Environment.GetCommandLineArgs().Contains(k_importantArgument))
		{
			TestContext.Fail($"The string '{k_importantArgument}' was not passed as an argument");
		}
	}
}
