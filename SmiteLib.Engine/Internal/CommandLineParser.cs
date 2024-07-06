using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal
{
	internal static class CommandLineParser
	{
		public static IEnumerable<string> Split(string commandline)
		{
			return System.CommandLine.Parsing.CommandLineStringSplitter.Instance.Split(commandline);
		}

		public static string Join(IEnumerable<string> parts)
		{
			return string.Join(' ', parts.Select(FormatPart));
		}

		public static string FormatPart(string part)
		{
			return Enclose(Escape(part));
		}

		public static string Escape(string part)
		{
			return part
				//.Replace(@"\", @"\\")
				.Replace(@"""", @"\""");
		}

		public static string Enclose(string part)
		{
			if (part.Contains(' '))
			{
				part = $@"""{part}""";
			}

			return part;
		}
	}
}
