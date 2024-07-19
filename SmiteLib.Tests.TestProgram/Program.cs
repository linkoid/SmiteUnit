using SmiteLib.Injection;
using System.Reflection;

namespace SmiteLib.Tests.TestProgram;

public static class Program
{
	internal static event EventHandler PostTestsEvent;
	internal static event EventHandler PostLoopEvent;

	private static SmiteRunner _internalRunner;
	private static SmiteRunner _externalRunner;

	public static void Main()
	{
		_internalRunner = new SmiteRunner();
		_internalRunner.EntryPoint();

		_externalRunner = new SmiteRunner(Assembly.LoadFrom("SmiteLib.Tests.dll"));
		_externalRunner.EntryPoint();

		PostTestsEvent?.Invoke(null, EventArgs.Empty);

		var args = Environment.GetCommandLineArgs();
		do
		{
			_internalRunner.ExitPoint();
			_externalRunner.ExitPoint();

			Thread.Sleep(1000);
		}
		while (args.Length >= 2 && args[1] == "loop");

		PostLoopEvent?.Invoke(null, EventArgs.Empty);

		_internalRunner.FinalExitPoint();
		_externalRunner.FinalExitPoint();
	}
}