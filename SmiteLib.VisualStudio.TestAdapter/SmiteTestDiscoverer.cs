using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Adapter;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace SmiteLib.VisualStudio.TestAdapter;

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

		InternalLogger.LogInfo("discovering tests");

		try
		{
			DiscoverTestsInternal(sources, discoveryContext, discoverySink);
		}
		catch (Exception ex)
		{
			InternalLogger.LogError(ex.ToString());
		}

		InternalLogger.LogInfo("finished discovering tests");
	}

	private void DiscoverTestsInternal(IEnumerable<string> sources, IDiscoveryContext discoveryContext, ITestCaseDiscoverySink discoverySink)
	{
		//InternalLogger.LogDebug($"run settings:\n{discoveryContext.RunSettings.SettingsXml}");

		foreach (var source in sources)
		{
			InternalLogger.LogDebug($"Processing {source}");

			using var loadContext = TestReflection.LoadWithContext(source, out var sourceAssembly);
			foreach (var testMethod in sourceAssembly.TestMethods)
			{
				InternalLogger.LogDebug($"Found TestMethod {testMethod}");
				var testCase = new TestCase(testMethod.FullName, SmiteTestExecutor.ExecutorUri, source);
				discoverySink.SendTestCase(testCase);
			}
		}
	}


}
