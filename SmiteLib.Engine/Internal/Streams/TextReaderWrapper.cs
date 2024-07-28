using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmiteLib.Internal.Streams
{
	public abstract class TextReaderWrapper : TextReader
	{
		protected abstract TextReader WrappedTextReader { get; }

		public override void Close()
		{
			WrappedTextReader.Close();
		}

		public override int Peek()
		{
			return WrappedTextReader.Peek();
		}

		public override int Read()
		{
			return WrappedTextReader.Read();
		}

		public override int Read(char[] buffer, int index, int count)
		{
			return WrappedTextReader.Read(buffer, index, count);
		}

		public override Task<int> ReadAsync(char[] buffer, int index, int count)
		{
			return WrappedTextReader.ReadAsync(buffer, index, count);
		}

		public override int ReadBlock(char[] buffer, int index, int count)
		{
			return WrappedTextReader.ReadBlock(buffer, index, count);
		}
		public override Task<int> ReadBlockAsync(char[] buffer, int index, int count)
		{
			return WrappedTextReader.ReadBlockAsync(buffer, index, count);
		}

		public override string? ReadLine()
		{
			return WrappedTextReader.ReadLine();
		}

		public override Task<string?> ReadLineAsync()
		{
			return WrappedTextReader.ReadLineAsync();
		}

		public override string ReadToEnd()
		{
			return WrappedTextReader.ReadToEnd();
		}

		public override Task<string> ReadToEndAsync()
		{
			return WrappedTextReader.ReadToEndAsync();
		}

		protected abstract override void Dispose(bool disposing);

#if IMPLEMENTS_NETSTANDARD2_1_OR_GREATER
		public override int Read(Span<char> buffer)
		{
			return WrappedTextReader.Read(buffer);
		}

		public override ValueTask<int> ReadAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
		{
			return WrappedTextReader.ReadAsync(buffer, cancellationToken);
		}

		public override int ReadBlock(Span<char> buffer)
		{
			return WrappedTextReader.ReadBlock(buffer);
		}

		public override ValueTask<int> ReadBlockAsync(Memory<char> buffer, CancellationToken cancellationToken = default)
		{
			return WrappedTextReader.ReadBlockAsync(buffer, cancellationToken);
		}
#endif
	}
}
