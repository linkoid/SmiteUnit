using NUnit.Framework;
using System;
using System.IO;

namespace SmiteUnit.Tests
{
	[SetUpFixture]
	public class SetUpNUnitEnvironment
	{
		[OneTimeSetUp]
		public void SetCurrentDirectory()
		{
			var directory = Path.GetDirectoryName(typeof(SetUpNUnitEnvironment).Assembly.Location);
			Environment.CurrentDirectory = directory;
		}
	}
}
