using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// A collection of Filter text rules.
	/// </summary>
	public class FilterTextRuleCollection : CollectionBase<FilterTextRule>
	{
		public FilterTextRuleCollection ()
			: base ()
		{
		}

		public bool AssignData (IFilterData other, IEqualityComparer<string> comparer)
		{
			int index = FindIndex (other, comparer);
			if (index < 0)
				return false;

			_items [index].CopyFrom (other);
			return true;
		}

		public int FindIndex (IFilterData other, IEqualityComparer<string> comparer)
		{
			return _items.FindIndex (x => x.Equals (other, comparer));
		}
	}
}

