using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Injection;

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

	[DoesNotReturn]
	public static void ProcessKillEntireProcessTree(int exitCode)
	{
		Environment.ExitCode = exitCode;
		Process.GetCurrentProcess().Kill(true);
		throw new InvalidOperationException();
	}

	[DoesNotReturn]
	public static void EnvironmentFailFast(int exitCode)
	{
		Environment.ExitCode = exitCode;
		Environment.FailFast($"{nameof(SmiteLib)} {nameof(EnvironmentFailFast)} exit strategy");
	}
}
