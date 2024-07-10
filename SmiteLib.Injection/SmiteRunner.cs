using System;
using System.Linq;
using System.Reflection;
using SmiteLib.Logging;
using SmiteLib.Internal;
using SmiteLib.Serialization;
using System.Collections;
using System.Collections.Generic;
using SmiteLib.Framework;
using System.Diagnostics.CodeAnalysis;

namespace SmiteLib.Injection;

public sealed class SmiteRunner : IUsesLogger
{
	public ILogger Logger { get; set; } = SmiteLogger.Current;

	public ISmiteDeserializer[] Deserializers { get; set; } =
	{
		new CommandLineDeserializer(),
	};

	public ExitStrategy ExitStrategy { get; set; } = ExitStrategies.EnvironmentExit;

	private readonly Assembly _assembly;
	private readonly AssemblyName _assemblyName;
	private readonly List<SmiteTest> _tests = new();

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

	public void RunTest(ISmiteId identifier)
	{
		var testMethod = SmiteMethod.Find(SmiteIdentifier.Parse(identifier), _assembly);
		var test = new SmiteTest(testMethod);
		_tests.Add(test);
		test.Run();
	}

	public bool EntryPoint()
	{
		this.ForceChildrenUseOwnLogger(Deserializers);

		var testFilter = new SmiteIdentifier(_assemblyName, "", "");
		var tests =
			from deserializer in Deserializers
			from identifier in deserializer.GetTestIds(testFilter)
			where identifier is SmiteIdentifier
			let testMethod = GetTestMethod((SmiteIdentifier)identifier)
			where testMethod != null && ((SmiteMethod)testMethod).Info.IsStatic
			select new SmiteTest((SmiteMethod)testMethod);

		bool ranTests = false;
		foreach (var test in tests)
		{
			_tests.Add(test);
			test.Run();
			ranTests = true;
		}
		return ranTests;
	}

	public void ExitPoint()
	{
		if (_tests.Count == 0)
			return;

		if (_tests.All(t => t.Ended) && _tests.Any(t => t.Failed))
		{
			ExitNow();
		}
	}

	public void FinalExitPoint()
	{
		if (_tests.Count == 0)
			return;

		if (AppDomain.CurrentDomain.IsFinalizingForUnload())
			return;

		if (_tests.Any(t => !t.Ended))
		{
			ExitNow();
		}
	}

	[DoesNotReturn]
	private void ExitNow()
	{
		try
		{
			ExitStrategy.Invoke(GetExitCode());
			throw new InvalidOperationException($"The exit strategy '{ExitStrategy}' returned instead of exiting the application");
		}
		catch (Exception ex)
		{
			Logger.LogException(ex, "Exception while exiting: ");
			throw;
		}
	}

	private int GetExitCode()
	{
		byte errorByte = 0;
		int failedTests = _tests.Count(t => t.Failed);
		if (failedTests > 0)
		{
			errorByte = (byte)(failedTests & 127);
		}

		int errorCode = 0;
		if (errorByte != 0)
		{
			errorCode = ('s' << 8*3) | ('m' << 8*2) | ('i' << 8*1) | errorByte;
		}
		return errorCode;
	}
}
