namespace System.Runtime.CompilerServices
{
	/// <summary>
	/// Indicates that the interface may only appear in the base clause of a type
	/// in the same assembly as the attributed interface or a type in
	/// an assembly that has InternalsVisibleTo the attributed interface's
	/// assembly.
	/// </summary>
	[AttributeUsage(AttributeTargets.Interface, AllowMultiple = false)]
	internal sealed class InternalImplementationOnlyAttribute : Attribute
	{
	}
}