using System;

namespace Dataelus.Search
{
	/**
 * The objective is to be able ot search a collection on successive filters
**/

	/// <summary>
	/// Interface for a searchable collection.
	/// </summary>
	public interface ISearchCollection
	{
		int FindIndexFirst (object criteria);

		int FindIndexNext (int startIndex, ISearchFilterPriorityCollection filters, ISearcherPriorityCollection searchers);

		int[] FindIndexAll (ISearcherPriorityCollection searchers);

		int[] FindIndexAll (ISearchFilterPriorityCollection filters, ISearcherPriorityCollection searchers);
	}
}

