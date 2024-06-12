using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib
{
	public interface ISmiteIdFilter
	{
		public bool Pass(ISmiteId id);
	}
}
