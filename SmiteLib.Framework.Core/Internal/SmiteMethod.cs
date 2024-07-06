using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal;

internal readonly struct SmiteMethod
{
	public readonly Type Type;
	public readonly MethodInfo Info;
	public readonly SmiteMethodAttribute? Attribute;

	private static (Type, MethodInfo) FindTypeAndMethod(SmiteIdentifier identifier, Assembly? assembly = null)
	{
		assembly ??= Assembly.Load(identifier.Assembly);

		Type type = assembly.FindType(identifier.Type)
			?? throw new TypeLoadException($"Could not find type '{identifier.Type}' in assembly '{assembly}'");

		var bindingFlags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance;
		MethodInfo method = type.GetMethod(identifier.Method, bindingFlags)
			?? throw new MissingMethodException(identifier.Type, identifier.Method);

		return (type, method);
	}

	public static bool TryFind(SmiteIdentifier identifier, out SmiteMethod smiteMethod, Assembly? assembly = null)
	{
		try
		{
			(Type type, MethodInfo method) = FindTypeAndMethod(identifier, assembly);
			smiteMethod = new SmiteMethod(type, method);
			return true;
		}
		catch
		{
			smiteMethod = default;
			return false;
		}
	}

	public static SmiteMethod Find(SmiteIdentifier identifier, Assembly? assembly = null)
	{
		(Type type, MethodInfo method) = FindTypeAndMethod(identifier, assembly);

		var smiteMethod = new SmiteMethod(type, method);
		smiteMethod.Validate();

		return smiteMethod;
	}

	private SmiteMethod(Type type, MethodInfo method)
	{
		Type = type ?? throw new ArgumentNullException(nameof(type));
		Info = method ?? throw new ArgumentNullException(nameof(method));
		Attribute = method.GetCustomAttribute<SmiteMethodAttribute>();
	}

	[MemberNotNull(nameof(Attribute))]
	internal void Validate()
	{
		if (Info.IsAbstract)
			throw new InvalidOperationException("Cannot target abstract methods");

		if (!Info.IsStatic)
			throw new NotImplementedException("Instance methods are not currently supported");

		if (Info.GetParameters().Any())
			throw new NotImplementedException("Methods with arguments are not currently supported");

		if (Attribute == null)
			throw new MethodAccessException($"Target method '{Info.Name}' does not have the required {nameof(SmiteMethodAttribute)}");
	}

	public void Invoke()
	{
		Info.Invoke(null, null);
	}
}
