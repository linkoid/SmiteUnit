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
		Logger = logger ?? throw new ArgumentNullException(nameof(logger));

		logger.SendMessage(TestMessageLevel.Informational, "discovering tests");

		try
		{
			DiscoverTestsInternal(sources, discoveryContext, logger, discoverySink);
		}
		catch (Exception ex)
		{
			logger.SendMessage(TestMessageLevel.Error, ex.ToString());
		}

		logger.SendMessage(TestMessageLevel.Informational, "finished discovering tests");
	}

	private void DiscoverTestsInternal(IEnumerable<string> sources, IDiscoveryContext discoveryContext, IMessageLogger logger, ITestCaseDiscoverySink discoverySink)
	{
		//logger.SendMessage(TestMessageLevel.Informational, $"run settings:\n{discoveryContext.RunSettings.SettingsXml}");

		foreach (var source in sources)
		{
			logger.SendMessage(TestMessageLevel.Informational, $"Processing {source}");

			using var loadContext = TestReflection.LoadWithContext(source, out var sourceAssembly);
			foreach (var testMethod in sourceAssembly.TestMethods)
			{
				logger.SendMessage(TestMessageLevel.Informational, $"Found TestMethod {testMethod}");
				var testCase = new TestCase(testMethod.FullName, SmiteTestExecutor.ExecutorUri, source);
				discoverySink.SendTestCase(testCase);
			}
		}
	}


}
