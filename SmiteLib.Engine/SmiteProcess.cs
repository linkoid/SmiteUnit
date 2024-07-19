using System.Diagnostics;
using System.Collections.Generic;
using SmiteLib.Internal;

namespace SmiteLib
{
	public class SmiteProcess : System.IDisposable
	{
		public IEnumerable<string> Arguments
		{
			get => CommandLineParser.Split(Process.StartInfo.Arguments);
			set => Process.StartInfo.Arguments = CommandLineParser.Join(value);
		}

		public bool UseSubprocess 
		{ 
			get => !Process.StartInfo.UseShellExecute;
			set
			{
				Process.StartInfo.UseShellExecute = !value;
				Process.StartInfo.RedirectStandardInput = value;
				Process.StartInfo.RedirectStandardOutput = value;
				Process.StartInfo.RedirectStandardError = value;
			}
		}

		public int ExitCode => Process.ExitCode;

		public string WorkingDirectory { get => Process.StartInfo.WorkingDirectory; set => Process.StartInfo.WorkingDirectory = value; }

		public int RunTimeout = -1;
		public readonly Process Process = new();
		private readonly string _baseArguments;

		public readonly RedirectionStreamReader Output;
		public readonly RedirectionStreamReader Error;

		public SmiteProcess(string filePath, string arguments = "")
		{
			_baseArguments = arguments;
			Process.StartInfo = new ProcessStartInfo(filePath);

			WorkingDirectory = System.IO.Path.GetDirectoryName(filePath) ?? "";
			UseSubprocess = true;

			Output = new(Process, StandardStream.Output);
			Error = new(Process, StandardStream.Error);
		}

		public bool Run()
		{
			if (!Process.Start())
				return false;

			Output.StartListening();
			Error.StartListening();

			bool exited = Process.WaitForExit(RunTimeout);

			Output.StopListening();
			Error.StopListening();

			return exited;
		}

		public bool RunTest(ISmiteId testId)
		{
			Process.StartInfo.Arguments = $"{_baseArguments} --smitelib.test:{testId}";
			return Run();
		}

		private bool _isDisposed;
		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed) return;

			if (!Process.HasExited)
			{
				Process.Kill(true);
			}

			if (disposing)
			{
				// Dispose managed state (managed objects)
				Output.Dispose();
				Error.Dispose();
				Process.Close();
				Process.Dispose();
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