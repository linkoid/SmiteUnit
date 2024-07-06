using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Logging;

internal static class UsesLoggerExtensions
{
	public static void ForceChildrenUseOwnLogger(this IUsesLogger parent, IEnumerable children)
	{
		foreach (var child in children)
		{
			if (child is not IUsesLogger loggerUser)
				continue;

			loggerUser.Logger = parent.Logger;
		}
	}
}
