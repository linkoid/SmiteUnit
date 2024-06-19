namespace SmiteLib.Tests.TestProgram;

public static class Program
{
	internal static event EventHandler PostTestsEvent;

	public static void Main()
	{
		var runner = new Framework.SmiteRunner();
		runner.RunAllStaticTests();

		PostTestsEvent?.Invoke(null, EventArgs.Empty);

		Thread.Sleep(1000);
	}
}