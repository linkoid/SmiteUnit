using NUnit.Framework;
using SmiteUnit.Engine;
using SmiteUnit.Internal;
using SmiteUnit.Tests.TestProgram;
using System;
using System.IO;

namespace SmiteUnit.Tests;

public class SmiteProcessTests
{
	[Test]
	public void StandardIO()
	{
		string expected;
		using (var stream = new MemoryStream())
		{
			SmiteProcessTestsEdge.PrintManyLinesUsing(new StreamWriter(stream));
			stream.Position = 0;
			expected = new StreamReader(stream).ReadToEnd();
		}
		var testProgram = Environment.ExpandEnvironmentVariables("%SMITEUNIT_TESTS_TESTPROGRAM_EXE%");
		var process = new SmiteProcess(testProgram)
		{
			RunTimeout = 10000,
			UseSubprocess = true,
		};
		process.RunTest(SmiteId.Method(SmiteProcessTestsEdge.PrintManyLines));

		var output = process.Output.ReadToEnd();
		Assert.AreEqual(expected, output);

		var error = process.Error.ReadToEnd();
		Assert.AreEqual(expected, error);
	}

	[Test]
	public void ExitOnCompletion()
	{
		var testProgram = Environment.ExpandEnvironmentVariables("%SMITEUNIT_TESTS_TESTPROGRAM_EXE%");
		var process = new SmiteProcess(testProgram, "loop")
		{
			RunTimeout = 10000,
			UseSubprocess = true,
		};
		process.RunTest(SmiteId.Method(SmiteProcessTestsEdge.PrintManyLines));
		Assert.IsTrue(process.HasExited);
	}

	[Test]
	[System.Diagnostics.CodeAnalysis.SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP017:Prefer using", 
		Justification = "Simplicity")]
	public void DisposeEndsChildProcess()
	{
		var process = new SmiteProcess(Variables.TestProgram, "loop")
		{
			RunTimeout = 1,
			UseSubprocess = true,
		};
		process.Run();

		var childProcess = System.Diagnostics.Process.GetProcessById(process.InternalProcess.Id);

		process.Dispose();

		Assert.IsTrue(childProcess.HasExited);
	}
}
