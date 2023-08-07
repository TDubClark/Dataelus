using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	public class FilterValueItemCollection : CollectionBase<FilterValueItem>
	{
		public FilterValueItemCollection ()
			: base ()
		{
		}

		public int FindIndex (string filterCode, IEqualityComparer<string> comparer)
		{
			return _items.FindIndex (x => comparer.Equals (x.FilterCode, filterCode));
		}

		public FilterValueItemCollection GetParentFilters (string filterCode)
		{
			var filters = new FilterValueItemCollection ();

			var comparer = new StringEqualityComparer ();

			int index = FindIndex (filterCode, comparer);
			if (index >= 0)
				AddParents (filters, _items [index], comparer);
			return filters;
		}

		/// <summary>
		/// Adds the filter (according to the code) and its parents.
		/// </summary>
		/// <param name="parentFilters">Parent filters.</param>
		/// <param name="filterCode">Filtercode.</param>
		/// <param name="comparer">Comparer.</param>
		private void AddFilterParents (FilterValueItemCollection parentFilters, string filterCode, IEqualityComparer<string> comparer)
		{
			int index = FindIndex (filterCode, comparer);
			if (index < 0)
				return;

			var thisItem = _items [index];

			// Add this item
			if (parentFilters.AddUnique (thisItem)) {
				// This item did not exist before, so the algorithm can keep going
				AddParents (parentFilters, thisItem, comparer);
			}
		}

		/// <summary>
		/// Adds the parents.
		/// </summary>
		/// <param name="parentFilters">Parent filters.</param>
		/// <param name="item">Item.</param>
		/// <param name="comparer">Comparer.</param>
		private void AddParents (FilterValueItemCollection parentFilters, FilterValueItem item, IEqualityComparer<string> comparer)
		{
			if (item.ParentFilterCodes != null) {
				// Add any parents
				foreach (var code in item.ParentFilterCodes) {
					AddFilterParents (parentFilters, code, comparer);
				}
			}
		}

		public bool AddUnique (FilterValueItem value)
		{
			return AddUnique (value, new FilterValueItemEqualityComparer (new StringEqualityComparer ()));
		}

		/// <summary>
		/// Adds the item (if unique); returns true if added.
		/// </summary>
		/// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
		/// <param name="value">Value.</param>
		/// <param name="comparer">Comparer.</param>
		public bool AddUnique (FilterValueItem value, IEqualityComparer<FilterValueItem> comparer)
		{
			if (!_items.Contains (value, comparer)) {
				Add (value);
				return true;
			}
			return false;
		}

		private void AddChildren (FilterValueItemCollection filterChildren, string filterCode, IEqualityComparer<string> comparer)
		{
			foreach (var item in _items) {
				// Find all items where the given filtercode is listed as the parent; add them to the list, and 
				if (item.ParentFilterCodes.Contains (filterCode, comparer)) {
					if (filterChildren.AddUnique (item)) {
						AddChildren (filterChildren, item.FilterCode, comparer);
					}
				}
			}
		}

		public FilterValueItemCollection GetChildFilters (string filterCode)
		{
			var filters = new FilterValueItemCollection ();

			var comparer = new StringEqualityComparer ();

			int index = FindIndex (filterCode, comparer);
			if (index >= 0)
				AddChildren (filters, _items [index].FilterCode, comparer);
			return filters;
		}
	}
}

