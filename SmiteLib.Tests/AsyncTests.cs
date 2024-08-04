using NUnit.Framework;
using SmiteLib.Engine;
using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Tests;


public class AsyncTests
{
	private SmiteProcess process;

	private AsyncTests()
	{

	}

	[SetUp]
	public void SetUp()
	{
		process = new SmiteProcess("SmiteLib.Tests.TestProgram.exe");
	}

	[SmiteSetUp]
	public static async Task SmiteAsyncSetUp()
	{
		await Task.Delay(100);
		Console.WriteLine("Set Up Complete");
	}

	[Test]
	public void SyncMethod()
	{
		[SmiteMethod]
		static void LocalMethod()
		{
			Console.WriteLine("Test Complete");
		}

		process.RunTest(SmiteId.Method(LocalMethod));
		Assert.IsTrue(process.Output.ReadToEnd().Contains("Test Complete"));
	}

	[Test]
	public void AsyncTask()
	{
		[SmiteMethod]
		static async Task LocalMethod()
		{
			await Task.Delay(100);
			Console.WriteLine("Test Complete");
		}

		process.RunTest(SmiteId.Method(LocalMethod));
		Assert.IsTrue(process.Output.ReadToEnd().Contains("Test Complete"));
	}

	[Test]
	public void AsyncValueTask()
	{
		[SmiteMethod]
		static async ValueTask LocalMethod()
		{
			await Task.Delay(100);
			Console.WriteLine("Test Complete");
		}

		process.RunTest(SmiteId.Method(LocalMethod));
		Assert.IsTrue(process.Output.ReadToEnd().Contains("Test Complete"));
	}
}
