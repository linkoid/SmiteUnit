using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal
{
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
				StandardStream.Input  => processStartInfo.RedirectStandardInput,
				StandardStream.Output => processStartInfo.RedirectStandardOutput,
				StandardStream.Error  => processStartInfo.RedirectStandardError,
				_ => throw new ArgumentOutOfRangeException(nameof(standardStream)),
			};

		public static void SetEncoding(this ProcessStartInfo processStartInfo, StandardStream standardStream, Encoding? encoding)
		{
			switch (standardStream)
			{
				case StandardStream.Input:
					processStartInfo.StandardInputEncoding = encoding;
					break;
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
				StandardStream.Input  => processStartInfo.StandardInputEncoding,
				StandardStream.Output => processStartInfo.StandardOutputEncoding,
				StandardStream.Error  => processStartInfo.StandardErrorEncoding,
				_ => throw new ArgumentOutOfRangeException(nameof(standardStream)),
			};
		}


	}
}
