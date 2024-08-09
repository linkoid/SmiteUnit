using SmiteUnit.Internal;
using System.Reflection;
using System;

namespace SmiteUnit;

public static class SmiteId
{
	public static ISmiteId Method(Delegate methodDelegate)
	{
		var method = methodDelegate.Method
			?? throw new ArgumentException($"Cannot target an anonymous method", nameof(methodDelegate));
		return Method(method);
	}

	public static ISmiteId Method(MethodInfo method)
		=> TryValidate(new SmiteIdentifier(method));

	public static ISmiteId Method(Type type, string method)
		=> TryValidate(new SmiteIdentifier(type, method));

	public static ISmiteId Method(AssemblyName assembly, string type, string method)
		=> TryValidate(new SmiteIdentifier(assembly, type, method));

	private static SmiteIdentifier TryValidate(SmiteIdentifier identifier)
	{
		if (SmiteMethod.TryFind(identifier, out var smiteMethod))
		{
			smiteMethod.Validate();
		}
		return identifier;
	}

	public static ISmiteId String(string identifier) => new SmiteStringIdentifier(identifier);
}
