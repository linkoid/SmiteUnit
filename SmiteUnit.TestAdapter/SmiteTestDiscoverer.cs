using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using SmiteUnit.TestAdapter.Extensions;
using Microsoft.TestPlatform.AdapterUtilities.ManagedNameUtilities;

namespace SmiteUnit.TestAdapter;

[Category("managed")]
[FileExtension(".dll")]
[FileExtension(".exe")]
[DefaultExecutorUri(SmiteTestExecutor.ExecutorUriString)]
public sealed class SmiteTestDiscoverer : ITestDiscoverer
{
	internal static IMessageLogger Logger;

	public void DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
	{
		InternalLogger.Handle = logger;
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));

		InternalLogger.LogInfo($"discovering tests");

		try
		{
			foreach (var testCase in DiscoverTestsInternal(sources, discoveryContext))
            {
                discoverySink.SendTestCase(testCase);
			}
		}
		catch (Exception ex)
		{
			InternalLogger.LogError(ex.ToString());
		}

		InternalLogger.LogInfo("finished discovering tests");
	}

	internal static IEnumerable<TestCase> DiscoverTests(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger)
	{
		InternalLogger.Handle = logger;
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));
		return DiscoverTestsInternal(sources, discoveryContext);
	}

	private static IEnumerable<TestCase> DiscoverTestsInternal(IEnumerable<string> sources, IDiscoveryContext discoveryContext)
	{
		//InternalLogger.LogDebug($"run settings:\n{discoveryContext.RunSettings.SettingsXml}");

		foreach (var source in sources)
		{
			InternalLogger.LogDebug($"Processing {source}");

			using var loadContext = TestReflection.LoadWithContext(source, out var sourceAssembly);
			foreach (var testMethod in sourceAssembly.TestMethods)
			{
				InternalLogger.LogDebug($"Found TestMethod {testMethod}");

				var testCase = new TestCase(testMethod.FullyQualifiedName, SmiteTestExecutor.ExecutorUri, source);

				ManagedNameHelper.GetManagedName(testMethod.Info, out string managedTypeName, out string managedMethodName);//, out string?[] hierarchyValues);
				testCase.SetManagedType(managedTypeName);
				testCase.SetManagedMethod(managedMethodName);
				//testCase.SetHierarchy(hierarchyValues);

				yield return testCase;
			}
		}
	}
}
