using System;
using System.Linq;
using System.Reflection;
using SmiteLib.Logging;
using SmiteLib.Internal;
using SmiteLib.Serialization;

namespace SmiteLib.Edge;

public sealed class SmiteRunner : IUsesLogger
{
	public ILogger Logger { get; set; } = SmiteLogger.Current;

	public ISmiteDeserializer[] Deserializers { get; set; } =
	{
		new CommandLineDeserializer(),
	};

	private readonly Assembly _assembly;
	private readonly AssemblyName _assemblyName;

	public SmiteRunner(Assembly assembly)
	{
		_assembly = assembly;
		_assemblyName = _assembly.GetName();
	}

	public SmiteRunner()
		: this(Assembly.GetCallingAssembly())
	{ }

	public SmiteRunner(AssemblyName assemblyName)
		: this(Assembly.Load(assemblyName))
	{ }

	public SmiteRunner(string assemblyName)
		: this(Assembly.Load(assemblyName))
	{ }

	private SmiteMethod? GetTestMethod(SmiteIdentifier identifier)
	{
		try
		{
			return SmiteMethod.Find(identifier, _assembly);
		}
		catch (Exception ex)
		{
			Logger.LogException(ex);
			return null;
		}
	}

	public bool RunAllStaticTests()
	{
		this.ForceChildrenUseOwnLogger(Deserializers);

		var testFilter = new SmiteIdentifier(_assemblyName, "", "");
		var testMethods =
			from deserializer in Deserializers
			from identifier in deserializer.GetTestIds(testFilter)
			where identifier is SmiteIdentifier
			let testMethod = GetTestMethod((SmiteIdentifier)identifier)
			where testMethod != null && ((SmiteMethod)testMethod).Info.IsStatic
			select (SmiteMethod)testMethod;

		bool ranTests = false;
		foreach (var testMethod in testMethods)
		{
			testMethod.Invoke();
			ranTests = true;

		}
		return ranTests;
	}

	public void RunTest(ISmiteId identifier)
	{
		var testMethod = SmiteMethod.Find(SmiteIdentifier.Parse(identifier), _assembly);
		testMethod.Invoke();
	}
}
