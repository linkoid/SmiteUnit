using System.Reflection;
using System.Runtime.Loader;

namespace SmiteLib.Tests.TestProgram;

public static class Program
{
	internal static event EventHandler PostTestsEvent;

	public static void Main()
	{
		var internalRunner = new Framework.SmiteRunner();
		internalRunner.RunAllStaticTests();

		var externalRunner = new Framework.SmiteRunner(Assembly.LoadFrom("SmiteLib.Tests.dll"));
		externalRunner.RunAllStaticTests();

		PostTestsEvent?.Invoke(null, EventArgs.Empty);

		Thread.Sleep(1000);
	}
}