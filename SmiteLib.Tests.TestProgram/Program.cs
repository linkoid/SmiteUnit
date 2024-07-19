using SmiteLib.Injection;
using System.Reflection;

namespace SmiteLib.Tests.TestProgram;

public static class Program
{
	internal static event EventHandler PostTestsEvent;

	private static SmiteRunner _internalRunner;
	private static SmiteRunner _externalRunner;

	public static void Main()
	{
		_internalRunner = new SmiteRunner();
		_internalRunner.EntryPoint();

		_externalRunner = new SmiteRunner(Assembly.LoadFrom("SmiteLib.Tests.dll"));
		_externalRunner.EntryPoint();

		PostTestsEvent?.Invoke(null, EventArgs.Empty);

		AppDomain.CurrentDomain.ProcessExit += OnExit;

		while (true)
		{
			_internalRunner.ExitPoint();
			_externalRunner.ExitPoint();

			Thread.Sleep(1000);
		}
	}

	private static void OnExit(object sender, EventArgs e)
	{
		_internalRunner.FinalExitPoint();
		_externalRunner.FinalExitPoint();
	}
}