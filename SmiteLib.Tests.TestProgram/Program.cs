using SmiteLib.Injection;
using System.Reflection;

namespace SmiteLib.Tests.TestProgram;

public static class Program
{
	internal static event EventHandler PostTestsEvent;

	public static void Main()
	{
		var internalRunner = new SmiteRunner();
		internalRunner.EntryPoint();

		var externalRunner = new SmiteRunner(Assembly.LoadFrom("SmiteLib.Tests.dll"));
		externalRunner.EntryPoint();

		PostTestsEvent?.Invoke(null, EventArgs.Empty);

		internalRunner.ExitPoint();
		externalRunner.ExitPoint();

		Thread.Sleep(1000);

		internalRunner.FinalExitPoint();
		externalRunner.FinalExitPoint();
	}
}