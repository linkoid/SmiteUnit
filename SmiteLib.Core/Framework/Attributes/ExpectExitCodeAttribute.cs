using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Framework;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public sealed class ExpectExitCodeAttribute : SmiteAttribute
{
	public readonly int ExitCode = 0;

	public ExpectExitCodeAttribute(int exitCode)
	{
		ExitCode = exitCode;
	}

	//public ExpectExitCodeAttribute(bool expectExitCodeZero)
	//{
	//	if (!expectExitCodeZero)
	//	{
	//		ExitCode = null;
	//	}
	//}
}
