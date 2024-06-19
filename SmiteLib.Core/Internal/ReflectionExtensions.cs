using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal;

internal static class ReflectionExtensions
{
	public static Assembly? LoadAssembly(AssemblyName? assemblyName)
	{
		if (assemblyName == null)
			return null;

		try
		{
			return Assembly.Load(assemblyName);
		}
		catch 
		{ 
			return null; 
		}
	}

	public static Type? FindType(this Assembly assembly, string name)
	{
		Type? bestMatch = null;
		int bestPriority = int.MaxValue;
		foreach (var type in assembly.GetTypes())
		{
			for (int i = 0; i < Math.Min(bestPriority, _typeMatchers.Length); i++)
			{
				if (!_typeMatchers[i].Invoke(type, name))
					continue;
				bestMatch = type;
				bestPriority = i;
				break;
			}
			if (bestPriority == 0)
				break;
		}
		return bestMatch;
	}
	private static readonly Func<Type, string, bool>[] _typeMatchers =
	{
		(t, s) => t.FullName?.Equals(s, StringComparison.InvariantCulture) ?? false,
		(t, s) => t.FullName?.Equals(s, StringComparison.InvariantCultureIgnoreCase) ?? false,
		(t, s) => t.Name.Equals(s, StringComparison.InvariantCulture),
		(t, s) => t.Name.Equals(s, StringComparison.InvariantCultureIgnoreCase),
	};
}
