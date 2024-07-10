using SmiteLib.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal;

internal class SmiteTest : ITestInfo
{
	public event Action? SetUp;
	public SmiteMethod Method { get; }
	public bool Failed { get => _failed; set => _failed |= value; }
	private bool _failed;

	public bool Running => _started && !Ended;
	private bool _started = false;
	public bool Ended => _context.IsFinished;

	private readonly TestContext _context;

	public SmiteTest(SmiteMethod method)
	{
		Method = method;
		HookSetUpMethods(null);
		_context = new TestContext(this, Logging.SmiteLogger.Current);
	}

	public void Run()
	{
		using var _ = _context.Activate();
		try
		{
			_started = true;
			SetUp?.Invoke();
			Method.Invoke();
		}
		catch (TargetInvocationException ex)
		{
			TestContext.Fail(ex.InnerException ?? ex);
		}
		catch (Exception ex)
		{
			TestContext.Fail(ex);
		}
		_context.IsFinished |= !_context.IsNotFinished;
	}

	private void HookSetUpMethods(object target)
	{
		var bindingFlags = BindingFlags.Static | BindingFlags.Instance 
			| BindingFlags.Public | BindingFlags.NonPublic;

		var setUpDelegates =
			from method in Method.Type.GetMethods(bindingFlags)
			where !Method.Info.IsStatic || method.IsStatic
			where method.GetCustomAttribute<SmiteSetUpAttribute>() != null
			select method.CreateDelegate<Action>(target);

		foreach (var action in setUpDelegates)
		{
			SetUp += action;
		}
	}
}
