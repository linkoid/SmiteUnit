using System;

namespace SmiteUnit.Framework;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SmiteTestAttribute : SmiteMethodAttribute
{
}
