using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace SmiteLib.Internal;

internal static partial class CommandLineParser
{
	public static IEnumerable<string> Split(string arguments)
	{
		var results = new List<string>();
		SplitIntoList(arguments, results);
		return results;
	}

	public static void SplitIntoList(string arguments, List<string> results)
	{
		ProcessInternal.ParseArgumentsIntoList(arguments, results);
	}
}
