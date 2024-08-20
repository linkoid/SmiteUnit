using NUnit.Framework;
using SmiteUnit.Engine;
using SmiteUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Tests;


public class AsyncTests
{
	private SmiteProcess process;

	[SetUp]
	public void SetUp()
	{
		process = new SmiteProcess(Variables.TestProgram);
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

#if NETCOREAPP3_1_OR_GREATER || NET6_0_OR_GREATER
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
#endif
}
