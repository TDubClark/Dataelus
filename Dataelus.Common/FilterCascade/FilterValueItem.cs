using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	public class FilterValueItem : FilterObjectItemBase, IFilterValueItem
	{
		protected Func<Table.ObjectRow, string> _funcGetTextDisplay;

		/// <summary>
		/// Gets or sets the function to get the display text.
		/// </summary>
		/// <value>The func get item display.</value>
		public Func<Table.ObjectRow, string> FuncGetTextDisplay {
			get { return _funcGetTextDisplay; }
			set { _funcGetTextDisplay = value; }
		}

		public string GetDisplayText (Dataelus.Table.ObjectRow row)
		{
			return GetDisplayText (this, row);
		}

		/// <summary>
		/// Gets the display text for the given row, for the given filter.
		/// </summary>
		/// <returns>The display.</returns>
		/// <param name="filter">Filter.</param>
		/// <param name="row">Row.</param>
		public static string GetDisplayText (FilterValueItem filter, Table.ObjectRow row)
		{
			switch (filter.DisplayBy) {
			case ValueDisplay.Function:
				return filter.FuncGetTextDisplay (row);
			case ValueDisplay.OtherColumn:
				if (String.IsNullOrEmpty (filter.DisplayColumnName))
					return row [filter.ColumnName].ToString ();
				return row [filter.DisplayColumnName].ToString ();
			case ValueDisplay.ValueColumn:
				return row [filter.ColumnName].ToString ();
			default:
				break;
			}
			return null;
		}

		public Predicate<Table.ObjectRow> GetFilterPredicate (IEqualityComparer<object> valueComparer)
		{
			return x => _selectedValues.Contains (x [_columnName], valueComparer);
		}

		public Func<Table.ObjectRow, bool> GetFilterFunc (IEqualityComparer<object> valueComparer)
		{
			var pred = GetFilterPredicate (valueComparer);
			return x => pred (x);
		}

		/// <summary>
		/// Determines whether the number of selected values within range.
		/// </summary>
		/// <returns><c>true</c> if this instance is selection within range; otherwise, <c>false</c>.</returns>
		public bool IsSelectionWithinRange ()
		{
			return (_selectedValues.Count <= _maxSelect && _selectedValues.Count >= _minSelect);
		}

		public void CopyFrom (IFilterValueItem other)
		{
			FilterValueItemData.CopyTo (this, other);
		}

		protected List<object> _selectedValues;

		/// <summary>
		/// Gets or sets the selected values.
		/// </summary>
		/// <value>The filter values.</value>
		public List<object> SelectedValues {
			get { return _selectedValues; }
			set { _selectedValues = value; }
		}

		public FilterValueItem ()
			: base ()
		{
		}
	}

	public class FilterValueItemEqualityComparer : IEqualityComparer<FilterValueItem>
	{
		IEqualityComparer<string> _comparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.IFilterTextItemEqualityComparer"/> class.
		/// </summary>
		/// <param name="comparer">Comparer for Filter Codes.</param>
		public FilterValueItemEqualityComparer (IEqualityComparer<string> comparer)
		{
			_comparer = comparer;
		}

		#region IEqualityComparer implementation

		public bool Equals (FilterValueItem x, FilterValueItem y)
		{
			return _comparer.Equals (x.FilterCode, y.FilterCode);
		}

		public int GetHashCode (FilterValueItem obj)
		{
			return obj.FilterCode.GetHashCode ();
		}

		#endregion
	}
}

