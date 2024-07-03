using SmiteLib.Framework;
using System;

namespace SmiteLib.Tests;

[SmiteProcess("SmiteLib.Tests.TestProgram.exe")]
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
}
