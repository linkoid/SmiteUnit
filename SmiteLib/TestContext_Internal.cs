using SmiteLib.Internal;
using SmiteLib.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;

namespace SmiteLib.Framework;

public partial class TestContext
{
	[ThreadStatic]
	private static List<TestContext>? _contextStack;
	private static List<TestContext> ContextStack => _contextStack ??= new();
	public static TestContext? CurrentContext => ContextStack.LastOrDefault();

	internal SmiteTest SmiteTest { get; }
	internal ILogger Logger { get; }
	internal bool IsFinished { get; set; }
	internal bool IsUnfinished { get; set; }


	internal TestContext(SmiteTest test, ILogger logger)
	{
		SmiteTest = test;
		Logger = logger;
	}

	public IContextActivator Activator => new ContextActivator(this);

	internal ActivationContext Activate()
	{
		return new ActivationContext(this);
	}
}

public partial class TestContext
{
	internal readonly struct ActivationContext : IDisposable
	{
		private readonly TestContext _testExecutionContext;

		public ActivationContext(TestContext context)
		{
			_testExecutionContext = context;
			ContextStack.Add(_testExecutionContext);
		}
 
		public void Dispose()
		{
			int lastIndex = ContextStack.LastIndexOf(_testExecutionContext);
			ContextStack.RemoveAt(lastIndex);
		}
	}
}

public partial class TestContext
{
	private readonly struct ContextActivator : IContextActivator
	{
		private readonly TestContext _testExecutionContext;

		public ContextActivator(TestContext context)
		{
			_testExecutionContext = context;
		}

		public IDisposable Activate()
		{
			return _testExecutionContext.Activate();
		}
	}
}
