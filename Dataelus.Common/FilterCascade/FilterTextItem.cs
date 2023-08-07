using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Filter item (filters on a single column) for a Text table.
	/// </summary>
	public class FilterTextItem : FilterDataBase, IFilterTextItem, ICodedHierarchyNode
	{
		protected bool _isSelectedAll;

		/// <summary>
		/// Gets or sets a value indicating whether the user has selected an option for "Include All".
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool IsSelectedAll {
			get { return _isSelectedAll; }
			set { _isSelectedAll = value; }
		}

		protected bool _allowSelectAll;

		/// <summary>
		/// Gets or sets whether the user is allowed to select an option for "Include All".
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool AllowSelectAll {
			get { return _allowSelectAll; }
			set { _allowSelectAll = value; }
		}

		protected string _displayColumnName;

		/// <summary>
		/// Gets or sets the name of the column used for display to the user.
		/// </summary>
		/// <value>The display name of the column.</value>
		public string DisplayColumnName {
			get {
				return _displayColumnName; }
			set { _displayColumnName = value;
			}
		}

		protected ValueDisplay _displayBy;

		/// <summary>
		/// Gets or sets the method by which the display text is determined.
		/// </summary>
		/// <value>The display by.</value>
		public ValueDisplay DisplayBy {
			get { return _displayBy; }
			set { _displayBy = value; }
		}

		protected List<string> _selectedValues;

		/// <summary>
		/// Gets or sets the selected values.
		/// </summary>
		/// <value>The filter values.</value>
		public List<string> SelectedValues {
			get { return _selectedValues; }
			set { _selectedValues = value; }
		}

		#region ICodedHierarchyNode implementation

		string ICodedHierarchyNode.ItemCode {
			get { return _filterCode; }
			set { _filterCode = value; }
		}

		List<string> ICodedHierarchyNode.ParentItemCodes {
			get { return _parentFilterCodes; }
			set { _parentFilterCodes = value; }
		}

		#endregion

		#region IFilterTextItem implementation

		string[] IFilterItem.ParentFilterCodes { 
			get { return _parentFilterCodes.ToArray (); } 
			set { _parentFilterCodes = new List<string> (value); }
		}

		#endregion

		protected List<string> _parentFilterCodes;

		/// <summary>
		/// Gets or sets the filter codes for parent filters.
		/// </summary>
		/// <value>The parent filter codes.</value>
		public new List<string> ParentFilterCodes {
			get { return _parentFilterCodes; }
			set { _parentFilterCodes = value; }
		}

		protected int _minSelect;

		/// <summary>
		/// Gets or sets the minimum selectable items.
		/// </summary>
		/// <value>The minimum select.</value>
		public int MinSelect {
			get { return _minSelect; }
			set { _minSelect = value; }
		}

		protected int _maxSelect;

		/// <summary>
		/// Gets or sets the maximum selectable items (set to -1 for "unlimited").
		/// </summary>
		/// <value>The max select.</value>
		public int MaxSelect {
			get { return _maxSelect; }
			set { _maxSelect = value; }
		}

		/// <summary>
		/// Copies from the given object.
		/// </summary>
		/// <param name="other">Other.</param>
		public void CopyFrom (IFilterTextItem other)
		{
			FilterTextItemData.CopyTo (this, other);
		}

		/// <summary>
		/// Determines whether the number of selected values within range.
		/// </summary>
		/// <returns><c>true</c> if this instance is selection within range; otherwise, <c>false</c>.</returns>
		public bool IsSelectionWithinRange ()
		{
			return (_selectedValues.Count <= _maxSelect && _selectedValues.Count >= _minSelect);
		}

		/// <summary>
		/// Determines whether the selection is valid.
		/// </summary>
		/// <returns><c>true</c> if this instance is selection valid; otherwise, <c>false</c>.</returns>
		public bool IsSelectionValid ()
		{
			if (_isSelectedAll) { return _allowSelectAll; }
			return IsSelectionWithinRange ();
		}

		public Predicate<Table.RowText> GetFilterPredicate ()
		{
			return GetFilterPredicate (new StringEqualityComparer ());
		}

		public Predicate<Table.RowText> GetFilterPredicate (IEqualityComparer<string> comparer)
		{
			return x => _selectedValues.Contains (x [_columnName], comparer);
		}

		public Func<Table.RowText, bool> GetFilterFunc (IEqualityComparer<string> comparer)
		{
			var pred = GetFilterPredicate (comparer);
			return x => pred (x);
		}


		protected Func<Table.RowText, string> _funcGetTextDisplay;

		/// <summary>
		/// Gets or sets the function to get the display text.
		/// </summary>
		/// <value>The func get item display.</value>
		public Func<Table.RowText, string> FuncGetTextDisplay {
			get { return _funcGetTextDisplay; }
			set { _funcGetTextDisplay = value; }
		}

		/// <summary>
		/// Gets the display text for the given row.
		/// </summary>
		/// <returns>The display text.</returns>
		/// <param name="row">Row.</param>
		public string GetDisplayText (Table.RowText row)
		{
			return GetDisplayText (this, row);
		}

		/// <summary>
		/// Gets the display text for the given row, for the given filter.
		/// </summary>
		/// <returns>The display.</returns>
		/// <param name="filter">Filter.</param>
		/// <param name="row">Row.</param>
		public static string GetDisplayText (FilterTextItem filter, Table.RowText row)
		{
			switch (filter.DisplayBy) {
			case ValueDisplay.Function:
				return filter.FuncGetTextDisplay (row);
			case ValueDisplay.OtherColumn:
				if (String.IsNullOrEmpty (filter.DisplayColumnName))
					return row [filter.ColumnName];
				return row [filter.DisplayColumnName];
			case ValueDisplay.ValueColumn:
				return row [filter.ColumnName];
			default:
				break;
			}
			return null;
		}

		public FilterTextItem ()
		{
			_selectedValues = new List<string> ();
			_displayBy = ValueDisplay.ValueColumn;
			_parentFilterCodes = new List<string> ();
			_maxSelect = 1;
			_minSelect = 1;
			_allowSelectAll = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.FilterTextItem"/> class.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="filterCode">The Filter Code; this is the unique identifier of this filter.</param>
		public FilterTextItem (string columnName, string filterCode)
			: this ()
		{
			_columnName = columnName;
			_filterCode = filterCode;
		}

		public FilterTextItem (string columnName, string filterCode, string[] parentFilterCodes)
			: this (columnName, filterCode)
		{
			_parentFilterCodes = new List<string> (parentFilterCodes);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.FilterTextItem"/> class.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="displayColumnName">The column containing values for display to the user.</param>
		/// <param name="filterCode">Filter code; this is the unique identifier of this filter.</param>
		public FilterTextItem (string columnName, string displayColumnName, string filterCode)
			: this (columnName, filterCode)
		{
			_displayColumnName = displayColumnName;
			if (!String.IsNullOrWhiteSpace (displayColumnName))
				_displayBy = ValueDisplay.OtherColumn;
		}

		public FilterTextItem (string columnName, string displayColumnName, string filterCode, string[] parentFilterCodes)
			: this (columnName, filterCode, displayColumnName)
		{
			_parentFilterCodes = new List<string> (parentFilterCodes);
		}

		public FilterTextItem (FilterTextItem other)
		{
			CopyFrom (other);
		}

		public FilterTextItem (IFilterTextItem item)
		{
			CopyFrom (item);
		}
	}

	public class FilterTextItemEqualityComparer : IEqualityComparer<FilterTextItem>
	{
		IEqualityComparer<string> _comparer;

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.FilterCascade.IFilterTextItemEqualityComparer"/> class.
		/// </summary>
		/// <param name="comparer">Comparer for Filter Codes.</param>
		public FilterTextItemEqualityComparer (IEqualityComparer<string> comparer)
		{
			_comparer = comparer;
		}

		#region IEqualityComparer implementation

		public bool Equals (FilterTextItem x, FilterTextItem y)
		{
			return _comparer.Equals (x.FilterCode, y.FilterCode);
		}

		public int GetHashCode (FilterTextItem obj)
		{
			return obj.FilterCode.GetHashCode ();
		}

		#endregion
	}
}

