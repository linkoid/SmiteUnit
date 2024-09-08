using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.TestAdapter;

internal static class TestReflection
{
	public static readonly Type SmiteAttribute = Assembly.Load("SmiteUnit").GetType(typeof(SmiteAttribute).FullName!)!;
	public static readonly Type SmiteTestAttribute = Assembly.Load("SmiteUnit.Framework").GetType(typeof(Framework.SmiteTestAttribute).FullName!)!;

	public static IEnumerable<string> GetLoadedAssemblyPaths()
	{
		return from assembly in AppDomain.CurrentDomain.GetAssemblies()
			   where !assembly.IsDynamic
			   let loadedPath = assembly.Location
			   where loadedPath != null
			   select loadedPath;
	}

	public static MetadataLoadContext LoadContext(string assemblyPath)
	{
		assemblyPath = System.IO.Path.GetFullPath(assemblyPath);
		var paths = GetLoadedAssemblyPaths().Concat(new[] 
		{
			assemblyPath,
			SmiteAttribute.Assembly.Location,
			SmiteTestAttribute.Assembly.Location,
		});
		var resolver = new PathAssemblyResolver(paths);
		return new MetadataLoadContext(resolver);
	}

	public static MetadataLoadContext LoadWithContext(string assemblyPath, out TestAssembly testAssembly)
	{
		assemblyPath = System.IO.Path.GetFullPath(assemblyPath);
		var loadContext = LoadContext(assemblyPath);
		var assembly = loadContext.LoadFromAssemblyPath(assemblyPath);
		testAssembly = new TestAssembly(assembly);
		return loadContext;
	}

	public static bool IsAssignableFromMetadata(this Type type, Type metadata)
	{
		for (Type metadataBase = metadata; metadataBase.BaseType != null; metadataBase = metadataBase.BaseType)
		{
			if (metadataBase.FullName == type.FullName)
				return true;
		}
		return false;
	}
}
