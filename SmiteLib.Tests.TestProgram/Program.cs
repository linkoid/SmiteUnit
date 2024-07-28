using SmiteLib.Injection;
using System.Reflection;

namespace SmiteLib.Tests.TestProgram;

public static class Program
{
	internal static event EventHandler PostTestsEvent;
	internal static event EventHandler PostLoopEvent;

	private static SmiteInjection _internalInjection;
	private static SmiteInjection _externalInjection;

	public static void Main()
	{
		_internalInjection = new SmiteInjection();
		_internalInjection.EntryPoint();

		_externalInjection = new SmiteInjection(Assembly.LoadFrom("SmiteLib.Tests.dll"));
		_externalInjection.EntryPoint();

		PostTestsEvent?.Invoke(null, EventArgs.Empty);

		var args = Environment.GetCommandLineArgs();
		do
		{
			_internalInjection.UpdatePoint();
			_externalInjection.UpdatePoint();

			Thread.Sleep(1000);
		}
		while (args.Length >= 2 && args[1] == "loop");

		PostLoopEvent?.Invoke(null, EventArgs.Empty);

		_internalInjection.ExitPoint();
		_externalInjection.ExitPoint();
	}
}