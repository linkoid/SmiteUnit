using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using SmiteLib.Engine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

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

		TestResult? result = null;
		Stopwatch? stopwatch = null;
		DateTime startTime = default;
		DateTime endTime = default;
		try
		{
			var processAttribute = testMethod.ProcessAttribute;
			var processPath = processAttribute.FilePath ?? testCase.Source;
			processPath = Environment.ExpandEnvironmentVariables(processPath);
			frameworkHandle.SendMessage(TestMessageLevel.Informational, $"Process Path: {processPath}");
			var process = new SmiteProcess(processPath);
			if (processAttribute.WorkingDirectory is not null)
				process.WorkingDirectory = processAttribute.WorkingDirectory;
			TrySetEncoding(process.Output, processAttribute.OutputEncoding, nameof(processAttribute.OutputEncoding));
			TrySetEncoding(process.Error , processAttribute.ErrorEncoding , nameof(processAttribute.ErrorEncoding ));

			stopwatch = Stopwatch.StartNew();
			try
			{
				startTime = DateTime.Now;
				process.RunTest(testMethod.GetSmiteId());
			}
			finally
			{
				stopwatch?.Stop();
				endTime = DateTime.Now;
			}

			result ??= new TestResult(testCase)
			{
				Outcome = process.ExitCode == testMethod.ExpectedExitCode ? TestOutcome.Passed : TestOutcome.Failed,
			};

			result.Messages.Add(new(TestResultMessage.StandardOutCategory, process.Output.ReadToEnd()));
			result.Messages.Add(new(TestResultMessage.StandardErrorCategory, process.Error.ReadToEnd()));
		}
		catch (Exception ex)
		{
			result = new TestResult(testCase)
			{
				Outcome = TestOutcome.Failed,
				ErrorMessage = ex.Message,
			};
		}

		result.StartTime = startTime;
		result.EndTime = endTime;
		result.Duration = stopwatch?.Elapsed ?? default;
		frameworkHandle.RecordResult(result);
	}

	public void Cancel() 
	{
		//throw new NotImplementedException();
	}

	private static void TrySetEncoding(Internal.RedirectionStreamReader redirectionStreamReader, string? encodingName,
		[CallerArgumentExpression(nameof(encodingName))] string argumentName = "Encoding")
	{
		if (string.IsNullOrEmpty(encodingName))
			return;

		Encoding encoding = encodingName switch
		{
			nameof(Encoding.ASCII           ) => Encoding.ASCII           ,
			nameof(Encoding.BigEndianUnicode) => Encoding.BigEndianUnicode,
			nameof(Encoding.Default         ) => Encoding.Default         ,
#if !NETSTANDARD
			nameof(Encoding.Latin1          ) => Encoding.Latin1          ,
#endif
			nameof(Encoding.Unicode         ) => Encoding.Unicode         ,
			nameof(Encoding.UTF32           ) => Encoding.UTF32           ,
			nameof(Encoding.UTF7            ) => Encoding.UTF7            ,
			nameof(Encoding.UTF8            ) => Encoding.UTF8            ,
			_ => throw new ArgumentException($"Invalid encoding '{encodingName}'", argumentName),
		};

		redirectionStreamReader.Encoding = encoding;
	}
}
