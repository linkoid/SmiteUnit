using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.TestAdapter;

internal readonly struct TestAssembly
{
	private readonly Assembly assembly;

	public TestAssembly(Assembly assembly)
	{
		this.assembly = assembly;
	}

	public IEnumerable<TestMethod> TestMethods => SmiteTestEnumerator.Iterate(assembly);
}
