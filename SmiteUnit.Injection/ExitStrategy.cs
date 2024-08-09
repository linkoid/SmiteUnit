using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Injection;

public delegate void ExitStrategy(int exitCode);

public static class ExitStrategies
{
	[DoesNotReturn]
	public static void EnvironmentExit(int exitCode)
	{
		Environment.Exit(exitCode);
	}

	[DoesNotReturn]
	public static void ProcessKill(int exitCode)
	{
		Environment.ExitCode = exitCode;
		Process.GetCurrentProcess().Kill();
		throw new InvalidOperationException();
	}

#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
	[DoesNotReturn]
	public static void ProcessKillEntireProcessTree(int exitCode)
	{
		Environment.ExitCode = exitCode;
		Process.GetCurrentProcess().Kill(true);
		throw new InvalidOperationException();
	}
#endif

	[DoesNotReturn]
	public static void EnvironmentFailFast(int exitCode)
	{
		Environment.ExitCode = exitCode;
		Environment.FailFast($"{nameof(SmiteUnit)} {nameof(EnvironmentFailFast)} exit strategy");
	}
}
