using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SmiteUnit.Internal;

internal readonly struct SmiteIdentifier : ISmiteId, ISmiteIdFilter
{
	public readonly AssemblyName Assembly;
	public readonly string Type;
	public readonly string Method;

	public SmiteIdentifier(AssemblyName assembly, string type, string method)
	{
		Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
		Type     = type     ?? throw new ArgumentNullException(nameof(type));
		Method   = method   ?? throw new ArgumentNullException(nameof(method));
	}

	public SmiteIdentifier(Type type, string method)
		: this(type.Assembly.GetName(), type.FullName!, method)
	{ }

	public SmiteIdentifier(MethodInfo method)
		: this(method.DeclaringType ?? throw new ArgumentException($"{method} has no declaring type", nameof(method)), 
			  method.Name)
	{ }

	public static SmiteIdentifier Parse(string s)
	{
		if (s == null)
			throw new ArgumentNullException(nameof(s));

		var parts = s.Split(':');
		if (parts.Length != 3)
			throw new FormatException($"String for {nameof(SmiteIdentifier)} must have format 'Assembly:Type:Method'");

		var assembly = new AssemblyName(parts[0].Trim().Trim('"'));
		var type = parts[1].Trim().Trim('"');
		var method = parts[2].Trim().Trim('"');

		return new SmiteIdentifier(assembly, type, method);
	}

	public static SmiteIdentifier Parse(ISmiteId identifier)
	{
		if (identifier == null)
			throw new ArgumentNullException(nameof(identifier));

		if (identifier is SmiteIdentifier smiteIdentifier)
			return smiteIdentifier;

		return Parse(identifier.ToString());
	}

	public override string ToString()
	{
		return $"{Assembly.Name}:{Type}:{Method}";
	}

	bool ISmiteIdFilter.Pass(ISmiteId id)
	{
		if (id is not SmiteIdentifier identifier)
			return false;

		if (Assembly.Name != identifier.Assembly.Name)
			return false;

		if (Type != "" && Type != identifier.Type)
			return false;

		if (Method != "" && Method != identifier.Method)
			return false;

		return true;
	}
}
