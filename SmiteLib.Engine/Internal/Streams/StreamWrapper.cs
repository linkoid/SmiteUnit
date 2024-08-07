using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace SmiteLib.Internal.Streams;

internal abstract class StreamWrapper : Stream
{
    protected abstract Stream WrappedStream { get; }

	protected virtual bool WrapperCanRead => true;
	protected virtual bool WrapperCanSeek => true;
	protected virtual bool WrapperCanWrite => true;
	protected virtual bool WrapperCanTimeout => true;
	

	#region Abstract Overrides
	public override bool CanRead => WrapperCanRead && WrappedStream.CanRead;
    public override bool CanSeek => WrapperCanSeek && WrappedStream.CanSeek;
    public override bool CanWrite => WrapperCanWrite && WrappedStream.CanWrite;
    public override long Length => WrappedStream.Length;
    public override long Position
	{
		get => WrappedStream.Position;
		set => WrappedStream.Position = WrapperCanSeek ? value : throw new NotSupportedException();
	}
    public override int Read(byte[] buffer, int offset, int count)
        => WrapperCanRead ? WrappedStream.Read(buffer, offset, count) : throw new NotSupportedException();
    public override long Seek(long offset, SeekOrigin origin)
        => WrapperCanSeek ? WrappedStream.Seek(offset, origin) : throw new NotSupportedException();
    public override void SetLength(long value)
        => WrappedStream.SetLength(CanRead ? value : throw new NotSupportedException());
	public override void Write(byte[] buffer, int offset, int count)
		=> WrappedStream.Write(buffer, offset, WrapperCanWrite ? count : throw new NotSupportedException());
	public override void Flush()
	{
		if (!CanWrite) throw new NotSupportedException();
		WrappedStream.Flush();
	}
	#endregion

	#region Virtual Overrides
	public override bool CanTimeout => WrapperCanTimeout && WrappedStream.CanTimeout;
	public override int ReadTimeout
	{
		get => WrapperCanTimeout ? WrappedStream.ReadTimeout : throw new NotSupportedException();
		set => WrappedStream.ReadTimeout = WrapperCanTimeout ? value : throw new NotSupportedException();
	}
	public override int WriteTimeout
	{
		get => WrapperCanTimeout ? WrappedStream.WriteTimeout : throw new NotSupportedException();
		set => WrappedStream.WriteTimeout = WrapperCanTimeout ? value : throw new NotSupportedException();
	}
	public override Task CopyToAsync(Stream destination, int bufferSize, CancellationToken cancellationToken)
		=> WrapperCanRead ? WrappedStream.CopyToAsync(destination, bufferSize, cancellationToken) : throw new NotSupportedException();
	public override int ReadByte()
		=> WrapperCanRead ? WrappedStream.ReadByte() : throw new NotSupportedException();
	public override IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
		=> WrapperCanRead ? WrappedStream.BeginRead(buffer, offset, count, callback, state) : throw new NotSupportedException();
	public override int EndRead(IAsyncResult asyncResult)
		=> WrapperCanRead ? WrappedStream.EndRead(asyncResult) : throw new NotSupportedException();
	public override Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		=> WrapperCanRead ? WrappedStream.ReadAsync(buffer, offset, count, cancellationToken) : throw new NotSupportedException();
	public override void WriteByte(byte value)
		=> WrappedStream.WriteByte(CanWrite ? value : throw new NotSupportedException());
	public override IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback? callback, object? state)
		=> WrapperCanWrite ? WrappedStream.BeginWrite(buffer, offset, count, callback, state) : throw new NotSupportedException();
	public override void EndWrite(IAsyncResult asyncResult)
		=> WrappedStream.EndWrite(CanWrite ? asyncResult : throw new NotSupportedException());
	public override Task WriteAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
		=> WrapperCanWrite ? WrappedStream.WriteAsync(buffer, offset, count, cancellationToken) : throw new NotSupportedException();
	public override Task FlushAsync(CancellationToken cancellationToken)
		=> WrapperCanWrite ? WrappedStream.FlushAsync(cancellationToken) : throw new NotSupportedException();

#if NETSTANDARD2_1_OR_GREATER || NET
	public override void CopyTo(Stream destination, int bufferSize)
		=> WrappedStream.CopyTo(destination, WrapperCanRead ? bufferSize : throw new NotSupportedException());
	public override int Read(Span<byte> buffer)
		=> WrapperCanRead ? WrappedStream.Read(buffer) : throw new NotSupportedException();
	public override ValueTask<int> ReadAsync(Memory<byte> buffer, CancellationToken cancellationToken = default)
		=> WrapperCanRead ? WrappedStream.ReadAsync(buffer, cancellationToken) : throw new NotSupportedException();
	public override void Write(ReadOnlySpan<byte> buffer)
		=> WrappedStream.Write(CanWrite ? buffer : throw new NotSupportedException());
	public override ValueTask WriteAsync(ReadOnlyMemory<byte> buffer, CancellationToken cancellationToken = default)
		=> WrapperCanWrite ? WrappedStream.WriteAsync(buffer, cancellationToken) : throw new NotSupportedException();
#endif

	#endregion

	public abstract override void Close();
	protected abstract override void Dispose(bool disposing);
}
