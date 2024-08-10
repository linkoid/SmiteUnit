using SmiteUnit.Engine.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmiteUnit.Engine;

public sealed class ArgumentList : IList<string>
{
	private readonly List<string> _list;

	internal ArgumentList(string arguments)
	{
		_list = new(CommandLineParser.Split(arguments));
	}

	internal ArgumentList(IEnumerable<string> arguments)
	{
		_list = new(arguments);
	}

	public override string ToString()
	{
		return CommandLineParser.Join(_list);
	}

	public string this[int index] { get => ((IList<string>)_list)[index]; set => ((IList<string>)_list)[index] = value; }

	public int Count => ((ICollection<string>)_list).Count;

	public bool IsReadOnly => ((ICollection<string>)_list).IsReadOnly;

	public void Add(string item)
	{
		((ICollection<string>)_list).Add(item);
	}

	public void Clear()
	{
		((ICollection<string>)_list).Clear();
	}

	public bool Contains(string item)
	{
		return ((ICollection<string>)_list).Contains(item);
	}

	public void CopyTo(string[] array, int arrayIndex)
	{
		((ICollection<string>)_list).CopyTo(array, arrayIndex);
	}

	public IEnumerator<string> GetEnumerator()
	{
		return ((IEnumerable<string>)_list).GetEnumerator();
	}

	public int IndexOf(string item)
	{
		return ((IList<string>)_list).IndexOf(item);
	}

	public void Insert(int index, string item)
	{
		((IList<string>)_list).Insert(index, item);
	}

	public bool Remove(string item)
	{
		return ((ICollection<string>)_list).Remove(item);
	}

	public void RemoveAt(int index)
	{
		((IList<string>)_list).RemoveAt(index);
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return ((IEnumerable)_list).GetEnumerator();
	}
}
