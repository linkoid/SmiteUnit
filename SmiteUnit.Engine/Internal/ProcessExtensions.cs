using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Engine.Internal;

internal static class ProcessExtensions
{
	public static StreamReader GetStreamReader(this Process process, StandardStream standardStream)
		=> standardStream switch
		{
			StandardStream.Output => process.StandardOutput,
			StandardStream.Error => process.StandardError,
			_ => throw new ArgumentOutOfRangeException(nameof(standardStream)),
		};

	public static bool GetRedirect(this ProcessStartInfo processStartInfo, StandardStream standardStream)
		=> standardStream switch
		{
			StandardStream.Input => processStartInfo.RedirectStandardInput,
			StandardStream.Output => processStartInfo.RedirectStandardOutput,
			StandardStream.Error => processStartInfo.RedirectStandardError,
			_ => throw new ArgumentOutOfRangeException(nameof(standardStream)),
		};

	public static void SetEncoding(this ProcessStartInfo processStartInfo, StandardStream standardStream, Encoding? encoding)
	{
		switch (standardStream)
		{
			case StandardStream.Input:
#if IMPLEMENTS_NETSTANDARD2_1_OR_GREATER
				processStartInfo.StandardInputEncoding = encoding;
				break;
#else

#endif
			case StandardStream.Output:
				processStartInfo.StandardOutputEncoding = encoding;
				break;
			case StandardStream.Error:
				processStartInfo.StandardErrorEncoding = encoding;
				break;
			default:
				throw new ArgumentOutOfRangeException(nameof(standardStream));
		}
	}

	public static Encoding? GetEncoding(this ProcessStartInfo processStartInfo, StandardStream standardStream)
	{
		return standardStream switch
		{
#if IMPLEMENTS_NETSTANDARD2_1_OR_GREATER
			StandardStream.Input => processStartInfo.StandardInputEncoding,
#else
			StandardStream.Input => throw new NotSupportedException("Current runtime does not support StandardInputEncoding"),
#endif
			StandardStream.Output => processStartInfo.StandardOutputEncoding,
			StandardStream.Error => processStartInfo.StandardErrorEncoding,
			_ => throw new ArgumentOutOfRangeException(nameof(standardStream)),
		};
	}


}
