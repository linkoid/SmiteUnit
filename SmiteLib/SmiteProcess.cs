using System.Diagnostics;
using System.Collections.Generic;
using SmiteLib.Internal;

namespace SmiteLib
{
	public class SmiteProcess
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

		public string WorkingDirectory { get => Process.StartInfo.WorkingDirectory; set => Process.StartInfo.WorkingDirectory = value; }

		public int RunTimeout = -1;

		public readonly Process Process = new();
		private readonly string _baseArguments;

		public readonly RedirectionStreamReader Output;
		public readonly RedirectionStreamReader Error;

		public SmiteProcess(string fileName, string arguments = "")
		{
			_baseArguments = arguments;
			Process.StartInfo = new ProcessStartInfo(fileName);

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
	}
}