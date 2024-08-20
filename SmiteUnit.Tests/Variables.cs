using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Tests;

internal static class Variables
{
	public const string TEST_PROGRAM_VARIABLE = "%SMITEUNIT_TESTS_TESTPROGRAM_EXE%";
	public static string TestProgram => Environment.ExpandEnvironmentVariables(TEST_PROGRAM_VARIABLE);
}
