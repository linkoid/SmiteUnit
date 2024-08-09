using SmiteUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.VisualStudio.TestAdapter;

internal readonly struct TestMethod : IEquatable<TestMethod>, IEquatable<string>
{
	public readonly Type Type;
	public readonly MethodInfo Info;

	public TestMethod(Type type, MethodInfo info)
	{
		Type = type;
		Info = info;
	}

	public string FullName => $"{Type.FullName}.{Info.Name}";
	public SmiteProcessAttribute ProcessAttribute => GetProcessAttribute();
	public int ExpectedExitCode => Info.GetCustomAttributeFromData<ExpectExitCodeAttribute>()?.ExitCode ?? 0;

	public override string ToString()
	{
		return this != null ? FullName : "null";
	}

	public ISmiteId GetSmiteId()
	{
		return SmiteId.Method(Type, Info.Name);
	}

	private SmiteProcessAttribute GetProcessAttribute()
	{
		var attributes = Info.GetCustomAttributesData()
			.Concat(Type.GetCustomAttributesData())
			.Concat(Type.Assembly.GetCustomAttributesData());

		var processAttributes = from attribute in attributes
								where attribute.AttributeType.FullName == typeof(SmiteProcessAttribute).FullName
								select attribute;

		InternalLogger.LogValue(processAttributes.Count());

		return new SmiteProcessAttribute()
		{
			FilePath         = processAttributes.FirstArgumentOrDefault<string>(nameof(SmiteProcessAttribute.FilePath        ), 0),
			Arguments        = processAttributes.FirstArgumentOrDefault<string>(nameof(SmiteProcessAttribute.Arguments       ), 1),
			WorkingDirectory = processAttributes.FirstArgumentOrDefault<string>(nameof(SmiteProcessAttribute.WorkingDirectory)   ),
			OutputEncoding   = processAttributes.FirstArgumentOrDefault<string>(nameof(SmiteProcessAttribute.OutputEncoding  )   ),
			ErrorEncoding    = processAttributes.FirstArgumentOrDefault<string>(nameof(SmiteProcessAttribute.ErrorEncoding   )   ),
		};
	}

	public bool Equals(TestMethod other)
	{
		if (Type is null || Info is null)
			return other.Type is null || other.Info is null;

		return FullName.Equals(other.FullName);
	}

	public bool Equals(string? other)
	{
		if (other is null)
			return Type is null || Info is null;

		return FullName.Equals(other);
	}

	public override bool Equals(object? obj)
	{
		if (obj is null && Equals(default(TestMethod)))
			return true;

		if (obj is TestMethod method && Equals(method))
			return true;

		if (obj is string str && Equals(str))
			return true;

		return false;
	}

	public override int GetHashCode()
	{
		return Type != null && Info != null ? FullName.GetHashCode() : 0;
	}

	public static bool operator ==(TestMethod left, TestMethod right)
	{
		return left.Equals(right);
	}
	public static bool operator !=(TestMethod left, TestMethod right)
	{
		return !left.Equals(right);
	}

	public static bool operator ==(TestMethod left, string? right)
	{
		return left.Equals(right);
	}
	public static bool operator !=(TestMethod left, string? right)
	{
		return !left.Equals(right);
	}
}
