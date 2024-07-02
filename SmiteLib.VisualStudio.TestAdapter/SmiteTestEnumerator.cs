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
	public static readonly Type SmiteTestAttributeType = typeof(Framework.SmiteTestAttribute);

	public static IMessageLogger? Logger;

	public static IEnumerable<TestMethod> Iterate(Assembly assembly)
	{
		//Logger?.SendMessage(TestMessageLevel.Informational, $"Iterate({assembly})");
		foreach (var type in assembly.GetExportedTypes())
		{
			foreach (var method in Iterate(type))
				yield return method;
		}
	}

	public static IEnumerable<TestMethod> Iterate(Type type)
	{
		//Logger?.SendMessage(TestMessageLevel.Informational, $"Iterate({type})");
		foreach (var method in type.GetMethods(BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance))
		{
			//Logger?.SendMessage(TestMessageLevel.Informational, $"{method}.GetCustomAttribute<SmiteTestAttribute>()");
			var customAttributeData = method.GetCustomAttributesData();
			var smiteTestAttributeData = customAttributeData.FirstOrDefault(
				attributeData =>
				{
					try
					{
						return attributeData.AttributeType.FullName == SmiteTestAttributeType.FullName;
					}
					catch (Exception ex)
					{
						Logger?.SendMessage(TestMessageLevel.Informational, $"AttributeType error at {type.FullName}.{method.Name}\n\t{ex.Message}");
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
