// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project below for more information.
// https://github.com/dotnet/roslyn/tree/main?tab=MIT-1-ov-file

namespace System.Runtime.CompilerServices
{
    /// <summary>
    /// This is a marker attribute that can be put on an interface to denote that only internal implementations
    /// of that interface should exist.
    /// </summary>
    [AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
    internal sealed class InternalImplementationOnlyAttribute : Attribute
    {
    }
}
