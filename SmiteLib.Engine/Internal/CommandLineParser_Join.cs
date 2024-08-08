using System;
using System.Diagnostics.CodeAnalysis;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#if !IMPLEMENTS_NETSTANDARD2_1_OR_GREATER
using ValueStringBuilder = System.Text.StringBuilder;
#endif

namespace SmiteLib.Internal;

internal static partial class CommandLineParser
{
	[SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP001:Dispose created", Justification = "ValueStringBuilder.ToString() calls Dispose()")]
	[SuppressMessage("IDisposableAnalyzers.Correctness", "IDISP003:Dispose previous before re-assigning")]
	public static string Join(IEnumerable<string> arguments)
	{
#if IMPLEMENTS_NETSTANDARD2_1_OR_GREATER
		Span<char> initialBuffer = stackalloc char[256];
#else
		int initialBuffer = 256;
#endif
		var stringBuilder = new ValueStringBuilder(initialBuffer);
		AppendArguments(ref stringBuilder, arguments);
		return stringBuilder.ToString();
	}

	public static void AppendArguments(ref ValueStringBuilder stringBuilder, IEnumerable<string> arguments)
	{
		foreach (var argument in arguments)
		{
			PasteArguments.AppendArgument(ref stringBuilder, argument);
		}
	}
}
