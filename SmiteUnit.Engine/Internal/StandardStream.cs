using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Engine.Internal;


internal enum StandardStream
{
	Input = 0,
	Output = 1,
	Error = 2,
}

internal static class StandardStreamExtensions
{
	public static TextWriter GetConsoleWriter(this StandardStream standardStream)
		=> standardStream switch
		{
			StandardStream.Output => Console.Out,
			StandardStream.Error => Console.Error,
			_ => throw new NotSupportedException(),
		};
}
