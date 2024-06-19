using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal;

internal static class MethodInfoExtensions
{
	public static T CreateDelegate<T>(this MethodInfo method)
		where T : Delegate
	{
		return (T)(object)Delegate.CreateDelegate(typeof(T), method);
	}
}
