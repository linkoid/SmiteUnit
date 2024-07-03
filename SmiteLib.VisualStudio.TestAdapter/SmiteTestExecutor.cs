using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace SmiteLib.VisualStudio.TestAdapter;

[ExtensionUri(ExecutorUriString)]
public sealed class SmiteTestExecutor : ITestExecutor
{
	///<summary>
	/// The Uri used to identify the NUnitExecutor
	///</summary>
	public const string ExecutorUriString = "executor://SmiteTestExecutor";

	public static readonly Uri ExecutorUri = new Uri(ExecutorUriString);

	public void RunTests(IEnumerable<TestCase> tests, IRunContext runContext, IFrameworkHandle frameworkHandle)
	{
		foreach (var testCase in tests)
		{
			frameworkHandle.SendMessage(TestMessageLevel.Informational, $"Test Case: {testCase.FullyQualifiedName} Source: {testCase.Source}");
			using var loadContext = TestReflection.LoadWithContext(testCase.Source, out var testAssembly);
			InternalLogger.Handle = frameworkHandle;

			var testMethod = testAssembly.TestMethods.FirstOrDefault(x => { frameworkHandle.SendMessage(TestMessageLevel.Informational, $"{x} == {testCase.FullyQualifiedName} is {x == testCase.FullyQualifiedName}"); return x == testCase.FullyQualifiedName; } );
			RunTest(testMethod, testCase, runContext, frameworkHandle);
		}
	}

	public void RunTests(IEnumerable<string> sources, IRunContext runContext, IFrameworkHandle frameworkHandle)
	{
		throw new NotImplementedException();
	}

	private void RunTest(TestMethod testMethod, TestCase testCase, IRunContext runContext, IFrameworkHandle frameworkHandle)
	{
		if (testMethod == null)
		{
			frameworkHandle.RecordResult(new TestResult(testCase)
			{
				Outcome = TestOutcome.NotFound,
				ErrorMessage = $"Could not find test {testCase.FullyQualifiedName} ({testMethod})",
			});
			return;
		}


		var processPath = testMethod.ProcessAttribute.FilePath ?? testCase.Source;
		frameworkHandle.SendMessage(TestMessageLevel.Informational, $"Process Path: {processPath}");
		var process = new SmiteProcess(processPath);

		TestResult? result = null;
		var stopwatch = Stopwatch.StartNew();
		DateTime startTime = DateTime.Now;
		DateTime endTime;
		try
		{
			process.RunTest(testMethod.GetSmiteId());
		}
		catch (Exception ex)
		{
			result = new TestResult(testCase)
			{
				Outcome = TestOutcome.Failed,
				ErrorMessage = ex.Message,
			};
		}
		stopwatch.Stop();
		endTime = DateTime.Now;

		result ??= new TestResult(testCase)
		{
			Outcome = process.Process.ExitCode == testMethod.ExpectedExitCode ? TestOutcome.Passed : TestOutcome.Failed,
		};

		result.Messages.Add(new(TestResultMessage.StandardOutCategory  , process.Output.ReadToEnd()));
		result.Messages.Add(new(TestResultMessage.StandardErrorCategory, process.Error .ReadToEnd()));

		result.StartTime = startTime;
		result.EndTime = endTime;
		result.Duration = stopwatch.Elapsed;
		frameworkHandle.RecordResult(result);
	}

	public void Cancel() 
	{
		//throw new NotImplementedException();
	}
}
