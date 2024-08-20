using NUnit.Framework;
using SmiteUnit.Engine;
using SmiteUnit.Engine.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SmiteUnit.Tests;

public class CommandLineParserTests
{
	[TestCaseSource(typeof(CommandLineTestsData), nameof(CommandLineTestsData.SplitExampleTestData))]
	public IEnumerable<string> SplitExample(string commandline)
	{
		using var process = new SmiteProcess(Variables.TestProgram, commandline);
		process.RunTest(SmiteId.Method(TestProgram.CommandLineParserTestsEdge.PrintArguments));
		Console.WriteLine($"Ran with arguments: {process.InternalProcess.StartInfo.Arguments}");
		Assert.That(process.ExitCode, Is.Zero);
		var output = process.Output.ReadToEnd();
		Console.WriteLine("Output:");
		Console.WriteLine(output);
		string[] splitOutput = output.Split(',');
		return splitOutput.Length > 1 ? splitOutput[1..^1] : Array.Empty<string>();
	}

	[TestCaseSource(typeof(CommandLineTestsData), nameof(CommandLineTestsData.SplitTestData))]
	public IEnumerable<string> Split(string commandline)
	{
		return CommandLineParser.Split(commandline);
	}

	[TestCaseSource(typeof(CommandLineTestsData), nameof(CommandLineTestsData.JoinTestData))]
	public string Join(IEnumerable<string> parts)
	{
		return CommandLineParser.Join(parts);
	}

	[TestCaseSource(typeof(CommandLineTestsData), nameof(CommandLineTestsData.JoinTestData))]
	public string ArgumentListToString(IEnumerable<string> parts)
	{
		return new ArgumentList(parts).ToString();
	}

	[Test, Ignore("Known bug")]
	public void UnclosedQuoteInArguments()
	{
		var unclosedQuote = "\"This quoted argument is never closed!   ";
		using var process = new SmiteProcess(Variables.TestProgram, unclosedQuote);
		process.RunTest(SmiteId.Method(TestProgram.CommandLineParserTestsEdge.PrintArguments));
		Assert.That(process.ExitCode, Is.Zero);
		Console.WriteLine($"Output position={process.Output.BaseStream.Position} length={process.Output.BaseStream.Length}");
		var output = process.Output.ReadToEnd();
		Console.WriteLine("Output:");
		Console.WriteLine(output);
		Assert.That(output.Contains(unclosedQuote));
	}
}

public static class CommandLineTestsData
{
	private static readonly Dictionary<string, IEnumerable<string>> CommandLinePairs = new()
	{
		[@"MyApp alpha beta"                                ] = new[] { @"MyApp", @"alpha", @"beta"                         },
		[@"MyApp ""alpha with spaces"" ""beta with spaces"""] = new[] { @"MyApp", @"alpha with spaces", @"beta with spaces" },
		[@"MyApp 'alpha with spaces' beta"                  ] = new[] { @"MyApp", @"'alpha", @"with", @"spaces'", @"beta"   },
		[@"MyApp \\\\\""alpha \""beta"                      ] = new[] { @"MyApp", @"\\""alpha", @"""beta"                   },
	};

	private static readonly TestCaseData[] SplitOnlyTestData =
	{
		new TestCaseData(@"MyApp \\\alpha \\\\""beta").Returns(new[] { @"MyApp", @"\\\alpha", @"\\beta" }),
	};

	private static readonly TestCaseData[] JoinOnlyTestData =
	{
		//new TestCaseData(arg: new[] {@"MyApp", @"\\\alpha", @"\\""beta" }).Returns(@"MyApp \\\alpha \\\\\""beta"), // redundant
	};

	public static IEnumerable SplitExampleTestData =>
		(from pair in CommandLinePairs
		 select new TestCaseData(pair.Key).Returns(pair.Value));

	public static IEnumerable SplitTestData =>
		(from pair in CommandLinePairs
		 select new TestCaseData(pair.Key).Returns(pair.Value))
		.Concat(SplitOnlyTestData);

	public static IEnumerable JoinTestData =>
		(from pair in CommandLinePairs
		 select new TestCaseData(pair.Value).Returns(pair.Key))
		.Concat(JoinOnlyTestData);
}