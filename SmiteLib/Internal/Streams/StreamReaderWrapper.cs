using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SmiteLib.Internal.Streams
{
	public abstract class StreamReaderWrapper : TextReaderWrapper
	{
		protected sealed override TextReader WrappedTextReader => WrappedStreamReader;
		protected abstract StreamReader WrappedStreamReader { get; }

		public virtual Stream BaseStream => WrappedStreamReader.BaseStream;
		public virtual Encoding CurrentEncoding => WrappedStreamReader.CurrentEncoding;


	}
}
