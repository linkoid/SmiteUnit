using SmiteUnit.Engine;
using SmiteUnit.Framework;
using System;
using System.Text;

namespace SmiteUnit.Tests;

[SmiteProcess("SmiteUnit.Tests.TestProgram.exe")]
public class SmiteAttributeTests
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
}
