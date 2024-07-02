using System;

namespace SmiteLib.Framework;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SmiteTestAttribute : SmiteMethodAttribute
{
}
