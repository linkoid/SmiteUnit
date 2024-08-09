using System.Collections.Generic;
using System.Reflection;

namespace SmiteUnit.TestAdapter;

internal static class CustomAttributeDataExtensions
{
	public static T? FirstArgumentOrDefault<T>(this IEnumerable<CustomAttributeData> attributes, string nameofArgument, int position = -1)
	{
		foreach (var attribute in attributes)
		{
			if (position >= 0 && position < attribute.ConstructorArguments.Count)
			{
				return (T?)attribute.ConstructorArguments[position].Value;
			}

			foreach (var namedArgument in attribute.NamedArguments)
			{
				if (namedArgument.MemberName != nameofArgument)
					continue;

				return (T?)namedArgument.TypedValue.Value;
			}
		}

		return default;
	}

	public static T? GetCustomAttributeFromData<T>(this MemberInfo member)
		where T : System.Attribute
	{
		System.Type type = typeof(T);
		foreach (var data in member.CustomAttributes)
		{
			System.Type dataType;
			try
			{
				dataType = data.AttributeType;
			}
			catch { continue; }

			if (dataType.FullName != type.FullName)
				continue;

			T? attribute = data.ConstructAs<T>();
			if (attribute is null)
				continue;

			return attribute;
		}

		return null;
	}

	public static T? ConstructAs<T>(this CustomAttributeData attributeData)
		where T : System.Attribute
	{
		System.Type type = typeof(T);

		System.Type dataType;
		ConstructorInfo dataConstructor;
		try
		{
			dataType = attributeData.AttributeType;
			dataConstructor = attributeData.Constructor;
		}
		catch { return null; }

		var matchingConstructor = FindMatchingMethod(dataConstructor, type.GetConstructors());
		if (matchingConstructor is null)
			return null;

		object?[] parameters = new object[attributeData.ConstructorArguments.Count];
		for (int i = 0; i < parameters.Length; i++)
		{
			parameters[i] = attributeData.ConstructorArguments[i].Value;
		}

		if (matchingConstructor.Invoke(parameters) is not T attribute)
			return null;
		
		foreach (var namedArgument in attributeData.NamedArguments)
		{
			if (namedArgument.IsField)
			{
				if (type.GetField(namedArgument.MemberName) is not FieldInfo field)
					continue;
				field.SetValue(attribute, namedArgument.TypedValue.Value);
			}
			else if (type.GetProperty(namedArgument.MemberName) is PropertyInfo property)
			{
				property.GetSetMethod()?.Invoke(attribute, new object?[] { namedArgument.TypedValue.Value });
			}
		}

		return attribute;
	}

	private static T? FindMatchingMethod<T>(T method, IEnumerable<T> searchMethods)
		where T : MethodBase
	{
		var parameters = method.GetParameters();

		foreach (var checkMethod in searchMethods)
		{
			if (method.Name != checkMethod.Name)
				continue;

			var checkParameters = checkMethod.GetParameters();
			if (parameters.Length != checkParameters.Length)
				continue;

			for (int i = 0; i < checkParameters.Length;  i++)
			{
				var parameter = parameters[i];
				var checkParameter = checkParameters[i];
				try
				{
					if (parameter.ParameterType.FullName != checkParameter.ParameterType.FullName)
						goto continueMethodLoop;
				}
				catch { goto continueMethodLoop; }
			}

			// else
			return checkMethod;

		continueMethodLoop:
			continue;
		}

		return null;
	}
}
