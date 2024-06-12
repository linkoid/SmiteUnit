using System;

namespace SmiteLib.Internal;

internal readonly struct SmiteStringIdentifier : ISmiteId
{
	private readonly string _identifier;
	public SmiteStringIdentifier(string identifier)
	{
		if (identifier == null) throw new ArgumentNullException(nameof(identifier));
		_identifier = identifier;
	}

	public override string ToString() => _identifier;
}
