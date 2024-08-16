namespace SmiteUnit.Docs.Snippets;

#region InjectionExample
using SmiteUnit.Injection;

public static class Program
{
	public static void Main()
	{
		// Near startup a SmiteInjection object should be created and it's EntryPoint() method called.
		// Create it with the name of the assembly that holds the tests.
		var smiteInjection = new SmiteInjection("MyTestAssembly");

		// Call the entry point methods. Tests will start running here. 
		smiteInjection.EntryPoint();

		// If the program is interactive, there is likely some sort of update loop.
		bool updateLoop = true;
		while (updateLoop) 
		{
			// Inside of this update loop, UpdatePoint should be periodically called.
			smiteInjection.UpdatePoint();
		}

		// Finally, before the program exits, ExitPoint() should be called.
		smiteInjection.ExitPoint();

		System.Environment.Exit(0);
	}
}
#endregion
