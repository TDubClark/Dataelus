using System;

namespace Dataelus.Search
{
	/// <summary>
	/// Searcher Inteface - determines whether a given object matches a given value
	/// </summary>
	public interface ISearcher
	{
		bool IsMatch (object value);
	}
}

