using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.VisualStudio.TestAdapter;

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

	public override string ToString()
	{
		return this != null ? FullName : "null";
	}

	public ISmiteId GetSmiteId()
	{
		return SmiteId.Method(Type, Info.Name);
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
