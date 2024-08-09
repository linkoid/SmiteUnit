using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Framework;

public interface ITestInfo
{
	public bool Failed { get; }
}
