using System;
using System.Text;

namespace SmiteLib.Framework;

[AttributeUsage(AttributeTargets.Assembly | AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
public class SmiteProcessAttribute : SmiteAttribute
{
	public string? FilePath { get; init; }
	public string? Arguments { get; init; }
	public string? WorkingDirectory { get; init; }
	public string? OutputEncoding { get; init; }
	public string? ErrorEncoding { get; init; }

	public SmiteProcessAttribute(string filePath, string? arguments = null)
	{
		FilePath = filePath;
		Arguments = arguments;
	}

	public SmiteProcessAttribute()
	{ }

}
