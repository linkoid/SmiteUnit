using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal;

internal delegate Task AsyncAction();

internal class SmiteTest : ITestInfo
{
	public event AsyncAction? SetUp;
	public SmiteMethod Method { get; }
	public bool Failed { get => _failed; set => _failed |= value; }
	private bool _failed;

	public bool Running => _started && !Ended;
	private bool _started = false;
	public bool Ended => _context.IsFinished || Failed;

	private readonly TestContext _context;
	private Task _task;

	public static SmiteTest NotFound(SmiteIdentifier identifier, Assembly? assembly = null)
	{
		SmiteTest notFoundTest = new SmiteTest();
		notFoundTest.Failed = true;
		return notFoundTest;
	}

	private SmiteTest()
	{
		_context = new TestContext(this, Logging.SmiteLogger.Current);
	}

	public SmiteTest(SmiteMethod method)
		: this()
	{
		Method = method;
		HookSetUpMethods(null);
	}

	public Task Run()
	{
		_task = RunAsync();
		return _task;
	}

	private async Task RunAsync()
	{
		using var _ = _context.Activate();
		try
		{
			_started = true;
			if (SetUp != null) await SetUp.Invoke();

			var result = Method.Invoke();
			if (result is Task task) await task;
#if IMPLEMENTS_NETSTANDARD2_1_OR_GREATER
			else if (result is ValueTask valueTask) await valueTask;
#endif
		}
		catch (TargetInvocationException ex)
		{
			TestContext.Fail(ex.InnerException ?? ex);
		}
		catch (Exception ex)
		{
			TestContext.Fail(ex);
		}
		_context.IsFinished |= !_context.IsUnfinished;
	}

	private void HookSetUpMethods(object target)
	{
		var bindingFlags = BindingFlags.Static | BindingFlags.Instance 
			| BindingFlags.Public | BindingFlags.NonPublic;


		var setUpMethods =
			from method in Method.Type.GetMethods(bindingFlags)
			where !Method.Info.IsStatic || method.IsStatic
			where method.GetCustomAttribute<SmiteSetUpAttribute>() != null
			select method;

		foreach (var method in setUpMethods)
		{
			AsyncAction asyncAction;
			if (typeof(Task).IsAssignableFrom(method.ReturnType))
			{
				asyncAction = method.CreateDelegate<AsyncAction>();
			}
#if IMPLEMENTS_NETSTANDARD2_1_OR_GREATER 
			else if (typeof(ValueTask).IsAssignableFrom(method.ReturnType))
			{
				var valueAsyncAction = method.CreateDelegate<Func<ValueTask>>();
				asyncAction = () => valueAsyncAction.Invoke().AsTask();
			}
#endif
			else
			{
				var action = method.CreateDelegate<Action>();
				asyncAction = () => { action.Invoke(); return Task.CompletedTask; };
			}

			SetUp += asyncAction;
		}
	}
}
