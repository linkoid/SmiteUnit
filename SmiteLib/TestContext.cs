using SmiteLib.Internal;
using SmiteLib.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace SmiteLib.Framework;

public partial class TestContext
{
	public ITestInfo Test => SmiteTest;

	[MemberNotNull("CurrentContext")]
	private static void EnsureValidContext()
	{
		if (CurrentContext == null)
			throw new InvalidOperationException($"{nameof(TestContext)} methods can only be used inside a valid test context.");
	}

	public static void Finished()
	{
		EnsureValidContext();
		CurrentContext.IsFinished = true;
	}

	public static void Unfinished()
	{
		EnsureValidContext();
		CurrentContext.IsFinished = false;
		CurrentContext.IsUnfinished = true;
	}

	public static void Fail(string? message = null)
	{
		EnsureValidContext();
		CurrentContext.SmiteTest.Failed = true;
		if (message != null)
		{
			CurrentContext.Logger.LogError(message);
		}
	}

	public static void Fail(Exception ex)
	{
		EnsureValidContext();
		CurrentContext.SmiteTest.Failed = true;
		CurrentContext.Logger.LogException(ex);
	}

	public void WrapInvoke(Action @delegate)
	{
		using var _ = this.Activate();
		try
		{
			@delegate.Invoke();
		}
		catch (Exception ex)
		{
			TestContext.Fail(ex);
			throw;
		}
	}

	public TResult WrapInvoke<TResult>(Func<TResult> @delegate)
	{
		TResult result = default;
		WrapInvoke(void () => result = @delegate());
		return result;
	}

	public static Func<object?[], object?> WrapDelegate(Delegate @delegate)
	{
		EnsureValidContext();
		var currentContext = CurrentContext;
		return (args) => currentContext.WrapInvoke(() => @delegate.DynamicInvoke(args));
	}

	public static Action WrapAction(Action action)
	{
		EnsureValidContext();
		var currentContext = CurrentContext;
		return () => currentContext.WrapInvoke(() => action.Invoke());
	}

	public static EventHandler WrapEventHandler(EventHandler eventHandler)
	{
		EnsureValidContext();
		var currentContext = CurrentContext;
		return (a, b) => currentContext.WrapInvoke(() => eventHandler.Invoke(a, b));
	}

	public static EventHandler<T> WrapEventHandler<T>(EventHandler<T> eventHandler)
	{
		EnsureValidContext();
		var currentContext = CurrentContext;
		return (a, b) => currentContext.WrapInvoke(() => eventHandler.Invoke(a, b));
	}
}
