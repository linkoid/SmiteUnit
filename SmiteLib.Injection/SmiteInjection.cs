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
using System.Threading.Tasks;

namespace SmiteLib.Injection;

public sealed class SmiteInjection : IUsesLogger
{
	public ILogger Logger { get; set; } = SmiteLogger.Current;

	public ISmiteDeserializer[] Deserializers { get; set; } =
	{
		new CommandLineDeserializer(),
	};

	public ExitStrategy ExitStrategy { get; set; } = ExitStrategies.EnvironmentExit;
	public bool UsingDelayedExitStrategy { get; set; } = false;

	private readonly Assembly _assembly;
	private readonly AssemblyName _assemblyName;
	private readonly List<SmiteTest> _tests = new();
	private bool _isExiting = false;

	public SmiteInjection(Assembly assembly)
	{
		_assembly = assembly;
		_assemblyName = _assembly.GetName();
	}

	public SmiteInjection()
		: this(Assembly.GetCallingAssembly())
	{ }

	public SmiteInjection(AssemblyName assemblyName)
		: this(Assembly.Load(assemblyName))
	{ }

	public SmiteInjection(string assemblyName)
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
			_tests.Add(SmiteTest.NotFound(identifier, _assembly));
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
		SmiteTest[] tests =
			(from deserializer in Deserializers
			 from identifier in deserializer.GetTestIds(testFilter)
			 where identifier is SmiteIdentifier
			 let testMethod = GetTestMethod((SmiteIdentifier)identifier)
			 where testMethod != null && ((SmiteMethod)testMethod).Info.IsStatic
			 select new SmiteTest((SmiteMethod)testMethod)
			).ToArray();

		_tests.AddRange(tests);

		_ = RunTestsAsync(tests);

		return tests.Any();
	}

	private async Task RunTestsAsync(IEnumerable<SmiteTest> tests)
	{
		foreach (var test in tests)
		{
			await test.Run();
		}
	}

	public void UpdatePoint()
	{
		if (_tests.Count == 0)
			return;

		if (_tests.All(t => t.Ended))
		{
			ExitNow();
			return;
		}
	}

	public void ExitPoint()
	{
		if (_tests.Count == 0)
			return;

		if (AppDomain.CurrentDomain.IsFinalizingForUnload())
			return;

		if (_tests.Any(t => !t.Ended))
		{
			ExitNow();
			return;
		}
	}

	private void ExitNow()
	{
		try
		{
			ExitStrategy.Invoke(GetExitCode());

			if (UsingDelayedExitStrategy)
				_isExiting = true;
			else
				throw new InvalidOperationException($"The exit strategy '{ExitStrategy.Method.DeclaringType?.Name}.{ExitStrategy.Method.Name}' returned instead of exiting immediately. " +
					$"Set 'UsingDelayedExitStrategy = true' to suppress this error.");
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


