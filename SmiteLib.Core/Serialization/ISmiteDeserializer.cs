using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;

namespace SmiteLib.Serialization;

[InternalImplementationOnly]
public interface ISmiteDeserializer
{
	public IEnumerable<ISmiteId> GetTestIds(ISmiteIdFilter? filter);
}

[InternalImplementationOnly]
public interface ISmiteDeserializer<out T> : ISmiteDeserializer
	where T : ISmiteId
{
	public new IEnumerable<T> GetTestIds(ISmiteIdFilter? filter);

	IEnumerable<ISmiteId> ISmiteDeserializer.GetTestIds(ISmiteIdFilter? filter)
	{
		return GetTestIds(filter).Cast<ISmiteId>();
	}
}
