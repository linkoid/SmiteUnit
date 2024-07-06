using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.VisualStudio.TestAdapter;

internal class SmiteTestEnumerator
{
	public static IEnumerable<TestMethod> Iterate(Assembly assembly)
	{
		//StaticLogger.LogDebug($"Iterate({assembly})");
		foreach (var type in assembly.GetExportedTypes())
		{
			foreach (var method in Iterate(type))
				yield return method;
		}
	}

	public static IEnumerable<TestMethod> Iterate(Type type)
	{
		//StaticLogger.LogDebug($"Iterate({type})");
		foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
		{
			//StaticLogger.LogDebug($"{method}.GetCustomAttribute<SmiteTestAttribute>()");
			var customAttributeData = method.GetCustomAttributesData();
			var smiteTestAttributeData = customAttributeData.FirstOrDefault(
				attributeData =>
				{
					try
					{
						return attributeData.AttributeType.FullName == TestReflection.SmiteTestAttribute.FullName;
					}
					catch (Exception ex)
					{
						//StaticLogger.LogDebug($"AttributeType error at {type.FullName}.{method.Name}\n\t{ex.Message}");
						return false;
					}
				}
			);
			if (smiteTestAttributeData == null)
				continue;

			yield return new(type, method);
		}
	}
}
