using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Collection of Text-based Filter definitions.
	/// </summary>
	public class FilterTextItemCollection : CollectionBase<FilterTextItem>
	{
		public FilterTextItemCollection ()
			: base ()
		{
		}

		public FilterTextItemCollection (System.Collections.Generic.IEnumerable<IFilterTextItem> otherItems)
			: base ()
		{
			foreach (var item in otherItems) {
				Add (new FilterTextItem (item));
			}
		}

		/// <summary>
		/// Copies from the array of other items.
		/// </summary>
		/// <param name="otherItems">Other items.</param>
		public void CopyFrom (IEnumerable<IFilterTextItem> otherItems)
		{
			var comparer = new StringEqualityComparer ();
			foreach (var other in otherItems) {
				CopyFrom (other, comparer);
			}
		}

		/// <summary>
		/// Copies from the other item.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="comparer">Comparer.</param>
		public void CopyFrom (IFilterTextItem other, IEqualityComparer<string> comparer)
		{
			int index = FindIndex (other.FilterCode, comparer);
			if (index < 0)
				return;
			_items [index].CopyFrom (other);
		}

		public void CopyFrom (FilterTextItemDataCollection dataItems)
		{
			var comparer = new StringEqualityComparer ();
			foreach (var data in dataItems) {
				CopyFrom (data, comparer);
			}
		}


		public FilterTextItemCollection GetChildFilters (string filterCode)
		{
			var filters = new FilterTextItemCollection ();

			var comparer = new StringEqualityComparer ();

			int index = FindIndex (filterCode, comparer);
			if (index >= 0)
				AddChildren (filters, _items [index].FilterCode, comparer);
			return filters;
		}

		private void AddChildren (FilterTextItemCollection filterChildren, string filterCode, IEqualityComparer<string> comparer)
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


		/// <summary>
		/// Gets the parent filters for the given filter code.
		/// </summary>
		/// <returns>The parent filters.</returns>
		/// <param name="filterCode">Filter code.</param>
		public FilterTextItemCollection GetParentFilters (string filterCode)
		{
			var filters = new FilterTextItemCollection ();

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
		private void AddFilterParents (FilterTextItemCollection parentFilters, string filterCode, IEqualityComparer<string> comparer)
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
		private void AddParents (FilterTextItemCollection parentFilters, FilterTextItem item, IEqualityComparer<string> comparer)
		{
			if (item.ParentFilterCodes != null) {
				// Add any parents
				foreach (var code in item.ParentFilterCodes) {
					AddFilterParents (parentFilters, code, comparer);
				}
			}
		}

		/// <summary>
		/// Finds the index of the item according to the given code.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="filterCode">Filter code.</param>
		/// <param name="comparer">Comparer.</param>
		public int FindIndex (string filterCode, IEqualityComparer<string> comparer)
		{
			return _items.FindIndex (x => comparer.Equals (x.FilterCode, filterCode));
		}

		public void AddUnique (IEnumerable<FilterTextItem> values)
		{
			foreach (var value in values) {
				AddUnique (value);
			}
		}

		/// <summary>
		/// Adds the item (if unique); returns true if added.
		/// </summary>
		/// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
		/// <param name="value">Value.</param>
		public bool AddUnique (FilterTextItem value)
		{
			return AddUnique (value, new FilterTextItemEqualityComparer (new StringEqualityComparer ()));
		}

		/// <summary>
		/// Adds the item (if unique); returns true if added.
		/// </summary>
		/// <returns><c>true</c>, if item was added, <c>false</c> otherwise.</returns>
		/// <param name="value">Value.</param>
		/// <param name="comparer">Comparer.</param>
		public bool AddUnique (FilterTextItem value, IEqualityComparer<FilterTextItem> comparer)
		{
			if (!_items.Contains (value, comparer)) {
				Add (value);
				return true;
			}
			return false;
		}

		/// <summary>
		/// Determines whether all filters are within their specified range.
		/// </summary>
		/// <returns><c>true</c> if all filters within range; otherwise, <c>false</c>.</returns>
		public bool IsAllFiltersWithinRange ()
		{
			return _items.All (x => x.IsSelectionWithinRange ());
		}

		/// <summary>
		/// Determines whether all filters are valid (includes range checking).
		/// </summary>
		/// <returns><c>true</c> if all filters valid; otherwise, <c>false</c>.</returns>
		public bool IsAllFiltersValid ()
		{
			return _items.All (x => x.IsSelectionValid ());
		}

		#region Convenient Overloads

		/// <summary>
		/// Add a new FilterTextItem using the specified columnName and filterCode.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="filterCode">Filter code; this is the unique identifier of this filter.</param>
		public void Add (string columnName, string filterCode)
		{
			Add (new FilterTextItem (columnName, filterCode));
		}

		/// <summary>
		/// Add a new FilterTextItem using the specified columnName, displayColumnName and filterCode.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="displayColumnName">The column containing values for display to the user.</param>
		/// <param name="filterCode">Filter code; this is the unique identifier of this filter.</param>
		public void Add (string columnName, string displayColumnName, string filterCode)
		{
			Add (new FilterTextItem (columnName, displayColumnName, filterCode));
		}

		/// <summary>
		/// Creates a new FilterTextItem.
		/// </summary>
		/// <returns>The new.</returns>
		/// <param name="columnName">Column name.</param>
		/// <param name="filterCode">Filter code.</param>
		public FilterTextItem CreateNew (string columnName, string filterCode)
		{
			return new FilterTextItem (columnName, filterCode);
		}

		public FilterTextItem CreateNew (string columnName, string filterCode, string[] parentFilterCodes)
		{
			return new FilterTextItem (columnName, filterCode, parentFilterCodes);
		}

		public FilterTextItem CreateNew (string columnName, string filterCode, string displayColumnName, string[] parentFilterCodes)
		{
			return new FilterTextItem (columnName, filterCode, displayColumnName, parentFilterCodes);
		}

		#endregion

		public static string SerializeJson (FilterTextItemCollection obj)
		{
			return Newtonsoft.Json.JsonConvert.SerializeObject (obj);
		}

		public static FilterTextItemCollection DeserializeJson (string jsonString)
		{
			return (FilterTextItemCollection)Newtonsoft.Json.JsonConvert.DeserializeObject (jsonString);
		}
	}
}

