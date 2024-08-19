using System;

namespace SmiteUnit.Docs.Snippets;


#region FrameworkExample
using SmiteUnit.Framework;

// The SmiteProcessAttribute tells the test adapter which program to start
[SmiteProcess("MyExecutable.exe", "arguments")]
public static class MySmiteTests
{
	// The SmiteSetUpAttribute marks methods that should run before any test methods
	[SmiteSetUp]
	public static void MySetUpMethod()
	{
		Console.WriteLine("Running my set up method!");
	}

	// The SmiteTestAttribute marks methods that can be run as tests
	[SmiteTest]
	public static void MyHelloWorldTest()
	{
		Console.WriteLine("Hello World!");
	}
}
#endregion