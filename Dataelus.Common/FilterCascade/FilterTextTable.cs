using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Filterable text table.
	/// </summary>
	public class FilterTextTable : Dataelus.Table.TextTable
	{
		public FilterTextTable ()
			: base ()
		{
		}

		/// <summary>
		/// Copy Constructor.
		/// </summary>
		/// <param name="other">Other.</param>
		public FilterTextTable (Table.TextTable other)
			: base (other)
		{
		}

		/// <summary>
		/// Gets the Rows which are within the filters.
		/// </summary>
		/// <returns>The items in filter.</returns>
		/// <param name="filters">Filters.</param>
		public virtual List<Table.RowText> GetRowsInFilter (FilterTextItemCollection filters)
		{
			return GetRowsInFilter (filters, new StringEqualityComparer ());
		}

		/// <summary>
		/// Gets the Rows which are within the filters.
		/// </summary>
		/// <returns>The rows in filter.</returns>
		/// <param name="filters">Filters.</param>
		/// <param name="comparer">Comparer.</param>
		public virtual List<Table.RowText> GetRowsInFilter (FilterTextItemCollection filters, IEqualityComparer<string> comparer)
		{
			var lst = _rows.Items;
			foreach (var filter in filters) {
				if (filter.IsSelectionWithinRange ()) {
					lst = lst.Where (filter.GetFilterFunc (comparer)).ToList ();
				}
			}
			return lst;
		}

		/// <summary>
		/// Determines whether the specified filter is applicable to this table.
		/// Searches for the column name in this table.
		/// </summary>
		/// <returns><c>true</c> if this instance is filter applicable the specified filter; otherwise, <c>false</c>.</returns>
		/// <param name="filter">Filter.</param>
		public virtual bool IsFilterApplicable(FilterTextItem filter){
			return (Columns.FindIndex (filter.ColumnName) >= 0);
		}

		/// <summary>
		/// Gets the table of rows which are within the filters.
		/// </summary>
		/// <returns>The table in filter.</returns>
		/// <param name="filters">Filters.</param>
		public virtual FilterTextTable GetTableInFilter (FilterTextItemCollection filters)
		{
			var tbl = new FilterTextTable ();
			tbl.Columns = new Dataelus.Table.ColumnCollection (this.Columns);
			tbl.Rows.AddItems (GetRowsInFilter (filters));
			return tbl;
		}

		/// <summary>
		/// Gets the values by column.
		/// </summary>
		/// <returns>The values by column.</returns>
		/// <param name="columnName">Column name.</param>
		public List<string> GetValuesByColumn (string columnName)
		{
			return GetValuesByColumn (columnName, new StringEqualityComparer ());
		}

		/// <summary>
		/// Gets the values by column.
		/// </summary>
		/// <returns>The values by column.</returns>
		/// <param name="columnName">Column name.</param>
		/// <param name="comparer">Comparer.</param>
		public List<string> GetValuesByColumn (string columnName, IEqualityComparer<string> comparer)
		{
			int colIndex = FindColumnIndex (columnName);
			return GetValuesByColumn (columnName, _rows.Items, colIndex, comparer);
		}

		/// <summary>
		/// Gets the values by column.
		/// </summary>
		/// <returns>The values by column.</returns>
		/// <param name="columnName">Column name.</param>
		/// <param name="filters">Filters.</param>
		/// <param name="comparer">Comparer.</param>
		public List<string> GetValuesByColumn (string columnName, FilterTextItemCollection filters, IEqualityComparer<string> comparer)
		{
			int colIndex = FindColumnIndex (columnName);
			return GetValuesByColumn (columnName, GetRowsInFilter (filters, comparer), colIndex, comparer);
		}

		/// <summary>
		/// Gets the values by column.
		/// </summary>
		/// <returns>The values by column.</returns>
		/// <param name="columnName">Column name.</param>
		/// <param name="lstRows">Lst rows.</param>
		/// <param name="colIndex">Col index.</param>
		/// <param name="comparer">Comparer.</param>
		public List<string> GetValuesByColumn (string columnName, List<Table.RowText> lstRows, int colIndex, IEqualityComparer<string> comparer)
		{
			return lstRows.Select (x => x [colIndex]).Distinct (comparer).ToList ();
		}

		/// <summary>
		/// Gets the value/display dictionary for a selection control.
		/// Value is Dictionary Key; Display is Dictionary Value.
		/// </summary>
		/// <returns>The value display.</returns>
		/// <param name="filters">Filters.</param>
		/// <param name="filterCode">Filter code.</param>
		public Dictionary<string, string> GetValueDisplay (FilterTextItemCollection filters, string filterCode)
		{
			return GetValueDisplay (filters, filterCode, new StringEqualityComparer ());
		}

		/// <summary>
		/// Gets the value/display dictionary for a selection control.
		/// Value is Dictionary Key; Display is Dictionary Value.
		/// </summary>
		/// <returns>The value display.</returns>
		/// <param name="filters">Filters.</param>
		/// <param name="filterCode">Filter code.</param>
		/// <param name="comparer">Comparer for values.</param>
		public Dictionary<string, string> GetValueDisplay (FilterTextItemCollection filters, string filterCode, IEqualityComparer<string> comparer)
		{
			var dict = new Dictionary<string, string> (comparer);

			var filterIndex = filters.FindIndex (filterCode, comparer);
			var filter = filters [filterIndex];

			var rows = GetRowsInFilter (filters.GetParentFilters (filterCode));
			foreach (var row in rows) {
				string value = row [filter.ColumnName];
				if (!dict.ContainsKey (value)) {
					dict.Add (value, filter.GetDisplayText (row));
				}
			}

			return dict;
		}

		/// <summary>
		/// Gets the list values for the given filter (Predicate) and value creator (Func, which creates a string value from a row).
		/// </summary>
		/// <returns>The list values.</returns>
		/// <param name="filter">Filter.</param>
		/// <param name="valueGetter">Value getter.</param>
		public List<string> GetListValues (Predicate<Table.RowText> filter, Func<Table.RowText, string> valueGetter)
		{
			return _rows.Where (x => filter (x)).Select (valueGetter).ToList ();
		}
	}
}

