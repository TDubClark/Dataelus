using System;

namespace Dataelus.Search
{
	/// <summary>
	/// A search filter.
	/// </summary>
	public interface ISearchFilter
	{
		bool IsMeetCriteria (object value);
	}
}

