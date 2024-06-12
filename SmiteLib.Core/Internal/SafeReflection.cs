using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal;

internal static class SafeReflection
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
}
