using NUnit.Framework;
using SmiteLib.Engine;
using SmiteLib.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SmiteLib.Tests;

public class CommandLineParserTests
{
	[TestCaseSource(typeof(CommandLineTestsData), nameof(CommandLineTestsData.SplitTestData))]
	public IEnumerable<string> SplitExample(string commandline)
	{
		var process = new SmiteProcess("SmiteLib.Tests.TestProgram.exe", commandline);
		process.RunTest(SmiteId.Method(TestProgram.CommandLineParserTestsEdge.PrintArguments));
		Console.WriteLine($"Output position={process.Output.BaseStream.Position} length={process.Output.BaseStream.Length}");
		var output = process.Output.ReadToEnd();
		Console.WriteLine("Output:");
		Console.WriteLine(output);
		return output.Split(',')[1..new Index(1, true)];
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
}

public static class CommandLineTestsData
{
	private static readonly Dictionary<string, IEnumerable<string>> CommandLinePairs = new()
	{
		[@"MyApp alpha beta"                                ] = new[] { @"MyApp", @"alpha", @"beta"                         },
		[@"MyApp ""alpha with spaces"" ""beta with spaces"""] = new[] { @"MyApp", @"alpha with spaces", @"beta with spaces" },
		[@"MyApp 'alpha with spaces' beta"                  ] = new[] { @"MyApp", @"'alpha", @"with", @"spaces'", @"beta"   },
		[@"MyApp \\\alpha \\\\""beta"                       ] = new[] { @"MyApp", @"\\\alpha", @"\\beta"                    },
		[@"MyApp \\\\\""alpha \""beta"                      ] = new[] { @"MyApp", @"\\""alpha", @"""beta"                   },
	};

	public static IEnumerable SplitTestData => 
		from pair in CommandLinePairs
		select new TestCaseData(pair.Key).Returns(pair.Value);

	public static IEnumerable JoinTestData =>
		from pair in CommandLinePairs
		select new TestCaseData(pair.Value).Returns(pair.Key);
}