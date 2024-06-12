using System.Collections;

namespace SmiteLib.Logging;

public interface IUsesLogger
{
    public ILogger Logger { get; set; }
}

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
