using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Internal.Streams;

internal class ReadOnlyMemoryStream : StreamWrapper
{
	private readonly MemoryStream _memoryStream;
	private MemoryStream _wrappedStream;

	public ReadOnlyMemoryStream(MemoryStream stream)
	{
		_memoryStream = stream;
		_wrappedStream = new MemoryStream(Array.Empty<byte>(), 0, 0, false, true);
	}
	protected override Stream WrappedStream
	{
		get
		{
			var currentBuffer = _memoryStream.GetBuffer();
			if (currentBuffer != _wrappedStream.GetBuffer()
				|| _memoryStream.Length != _wrappedStream.Length)
			{
				var newStream = new MemoryStream(currentBuffer, 0, (int)_memoryStream.Length, false, true);
				newStream.Position = _wrappedStream.Position;
				_wrappedStream = newStream;
			}

			return _wrappedStream;
		}
	}

	protected override bool WrapperCanRead => _memoryStream.CanRead;
	protected override bool WrapperCanSeek => _memoryStream.CanSeek;

	public override void Close()
		=> throw new NotSupportedException();

	protected override void Dispose(bool disposing)
		=> throw new NotSupportedException();
}
