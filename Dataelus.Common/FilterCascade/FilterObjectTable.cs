using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.FilterCascade
{
	public class FilterObjectTable : Dataelus.Table.ObjectTable
	{
		public FilterObjectTable ()
			: base ()
		{
		}

		public FilterObjectTable (Table.ObjectTable other)
			: base (other)
		{
		}


		protected IEqualityComparer<object> _valueComparer;

		/// <summary>
		/// Gets or sets the value comparer.
		/// </summary>
		/// <value>The value comparer.</value>
		public IEqualityComparer<object> ValueComparer {
			get{ return _valueComparer; }
			set{ _valueComparer = value; }
		}

		/// <summary>
		/// Gets the Rows which are within the filters.
		/// </summary>
		/// <returns>The items in filter.</returns>
		/// <param name="filters">Filters.</param>
		public virtual List<Table.ObjectRow> GetRowsInFilter (FilterValueItemCollection filters)
		{
			return GetRowsInFilter (filters, _valueComparer);
		}

		/// <summary>
		/// Gets the Rows which are within the filters.
		/// </summary>
		/// <returns>The rows in filter.</returns>
		/// <param name="filters">Filters.</param>
		/// <param name="comparer">Comparer.</param>
		public virtual List<Table.ObjectRow> GetRowsInFilter (FilterValueItemCollection filters, IEqualityComparer<object> comparer)
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
		public virtual bool IsFilterApplicable (FilterValueItem filter)
		{
			return (Columns.FindIndex (filter.ColumnName) >= 0);
		}

		/// <summary>
		/// Gets the table of rows which are within the filters.
		/// </summary>
		/// <returns>The table in filter.</returns>
		/// <param name="filters">Filters.</param>
		public virtual FilterObjectTable GetTableInFilter (FilterValueItemCollection filters)
		{
			var tbl = new FilterObjectTable ();
			tbl.Columns = new Dataelus.Table.ObjectColumnCollection (this.Columns);
			tbl.Rows.AddItems (GetRowsInFilter (filters));
			return tbl;
		}

		/// <summary>
		/// Gets the values by column.
		/// </summary>
		/// <returns>The values by column.</returns>
		/// <param name="columnName">Column name.</param>
		public List<object> GetValuesByColumn (string columnName)
		{
			return GetValuesByColumn (columnName, _valueComparer);
		}

		/// <summary>
		/// Gets the values by column.
		/// </summary>
		/// <returns>The values by column.</returns>
		/// <param name="columnName">Column name.</param>
		/// <param name="comparer">Comparer.</param>
		public List<object> GetValuesByColumn (string columnName, IEqualityComparer<object> comparer)
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
		public List<object> GetValuesByColumn (string columnName, FilterValueItemCollection filters, IEqualityComparer<object> comparer)
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
		public List<object> GetValuesByColumn (string columnName, List<Table.ObjectRow> lstRows, int colIndex, IEqualityComparer<object> comparer)
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
		public Dictionary<object, string> GetValueDisplay (FilterValueItemCollection filters, string filterCode)
		{
			return GetValueDisplay (filters, filterCode, _valueComparer);
		}

		public Dictionary<object, string> GetValueDisplay (FilterValueItemCollection filters, string filterCode, IEqualityComparer<object> valueComparer)
		{
			return GetValueDisplay (filters, filterCode, new StringEqualityComparer (), valueComparer);
		}

		/// <summary>
		/// Gets the value/display dictionary for a selection control.
		/// Value is Dictionary Key; Display is Dictionary Value.
		/// </summary>
		/// <returns>The value display.</returns>
		/// <param name="filters">Filters.</param>
		/// <param name="filterCode">Filter code.</param>
		/// <param name="comparer">Comparer for values.</param>
		public Dictionary<object, string> GetValueDisplay (FilterValueItemCollection filters, string filterCode, IEqualityComparer<string> comparer, IEqualityComparer<object> valueComparer)
		{
			var dict = new Dictionary<object, string> (valueComparer);

			var filterIndex = filters.FindIndex (filterCode, comparer);
			var filter = filters [filterIndex];

			var rows = GetRowsInFilter (filters.GetParentFilters (filterCode));
			foreach (var row in rows) {
				object value = row [filter.ColumnName];
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
		public List<object> GetListValues (Predicate<Table.ObjectRow> filter, Func<Table.ObjectRow, object> valueGetter)
		{
			return _rows.Where (x => filter (x)).Select (valueGetter).ToList ();
		}
	}
}

