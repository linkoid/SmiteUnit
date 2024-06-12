using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteLib.Internal;

internal static class ArgumentExtensions
{
    public static bool TryStripPrefix(this string prefixedArg, string prefix, out string arg)
    {
        if (prefixedArg.StartsWith(prefix, StringComparison.InvariantCultureIgnoreCase))
        {
            arg = prefixedArg.Substring(prefix.Length);
            return true;
        }
        else
        {
            arg = prefixedArg;
            return false;
        }
    }
}
