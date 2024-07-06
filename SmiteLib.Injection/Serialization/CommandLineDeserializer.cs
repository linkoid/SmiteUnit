using SmiteLib.Logging;
using SmiteLib.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Serialization;

public class CommandLineDeserializer : ISmiteDeserializer<SmiteIdentifier>, IUsesLogger
{
	public ILogger Logger { get; set; } = SmiteLogger.Current;

	internal IEnumerable<SmiteIdentifier> GetTestIds(ISmiteIdFilter? filter)
	{
		string prefix = $"--smitelib.test:";
		foreach (var arg in Environment.GetCommandLineArgs())
		{
			if (!arg.TryStripPrefix(prefix, out var testString))
				continue;

			SmiteIdentifier identifier;
			try
			{
				identifier = SmiteIdentifier.Parse(testString);
			}
			catch (FormatException ex)
			{
				Logger.LogException(ex, $"Exception parsing commandline argument {arg}");
				continue;
			}

			try
			{
				if (!filter?.Pass(identifier) ?? false)
					continue;
			}
			catch (FormatException ex)
			{
				Logger.LogException(ex, $"Exception filtering commandline argument {arg} with filter {filter}");
				continue;
			}

			yield return identifier;
		}
	}

	IEnumerable<SmiteIdentifier> ISmiteDeserializer<SmiteIdentifier>.GetTestIds(ISmiteIdFilter? filter)
		=> GetTestIds(filter);
}
