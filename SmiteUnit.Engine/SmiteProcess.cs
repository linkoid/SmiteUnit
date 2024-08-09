using System.Diagnostics;
using System.Collections.Generic;
using SmiteUnit.Internal;

namespace SmiteUnit.Engine
{
	public class SmiteProcess : System.IDisposable
	{
		public ArgumentList Arguments { get; }

		public bool UseSubprocess 
		{ 
			get => !_process.StartInfo.UseShellExecute;
			set
			{
				_process.StartInfo.UseShellExecute = !value;
				_process.StartInfo.RedirectStandardInput = value;
				_process.StartInfo.RedirectStandardOutput = value;
				_process.StartInfo.RedirectStandardError = value;
			}
		}

		public bool HasExited => _process.HasExited;
		public int ExitCode => _process.ExitCode;

		public string WorkingDirectory { get => _process.StartInfo.WorkingDirectory; set => _process.StartInfo.WorkingDirectory = value; }

		public int RunTimeout { get; set; } = -1;

		[System.Obsolete]
		public Process InternalProcess => _process;


		internal readonly Process _process = new();

		public readonly RedirectionStreamReader Output;
		public readonly RedirectionStreamReader Error;

		public SmiteProcess(string filePath, string arguments = "")
		{
			_process.StartInfo = new ProcessStartInfo(filePath);
			Arguments = new(arguments);

			WorkingDirectory = System.IO.Path.GetDirectoryName(filePath) ?? "";
			UseSubprocess = true;

			Output = new(_process, StandardStream.Output);
			Error = new(_process, StandardStream.Error);
		}

		public bool Run()
		{
			_process.StartInfo.Arguments = Arguments.ToString();
			return RunInternal();
		}

		public bool RunTest(ISmiteId testId)
		{
			_process.StartInfo.Arguments = $"{Arguments} --SmiteUnit.test:{testId}";
			return RunInternal();
		}

		private bool RunInternal()
		{
			if (!_process.Start())
				return false;

			Output.StartListening();
			Error.StartListening();

			bool exited = _process.WaitForExit(RunTimeout);

			Output.StopListening();
			Error.StopListening();

			return exited;
		}

		private bool _isDisposed;
		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed) return;

			if (!_process.HasExited)
			{
#if NET5_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
				_process.Kill(true);
#else
				_process.Kill();
#endif
			}

			if (disposing)
			{
				// Dispose managed state (managed objects)
				Output.Dispose();
				Error.Dispose();
				_process.Close();
				_process.Dispose();
			}

			_isDisposed = true;
		}

		public void Dispose()
		{
			Dispose(disposing: true);
			System.GC.SuppressFinalize(this);
		}

		~SmiteProcess()
		{
			Dispose(disposing: false);
		}
	}
}