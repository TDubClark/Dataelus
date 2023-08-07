using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Represents a cell range for a table, determined as an array of Row Indexes and either Column Names or Column Indexes
	/// </summary>
	public class CellRange
	{
		/// <summary>
		/// Gets or sets the column names.
		/// </summary>
		/// <value>The column names.</value>
		public string[] ColumnNames{ get; set; }

		/// <summary>
		/// Gets or sets the column indexes.
		/// </summary>
		/// <value>The index of the column.</value>
		public int[] ColumnIndex{ get; set; }

		/// <summary>
		/// Gets or sets the row indexes.
		/// </summary>
		/// <value>The index of the row.</value>
		public int[] RowIndex{ get; set; }


		public bool IsColumnNames ()
		{
			return IsArrayContent (this.ColumnNames);
		}

		public bool IsColumnIndex ()
		{
			return IsArrayContent (this.ColumnIndex);
		}

		public bool IsRowIndex ()
		{
			return IsArrayContent (this.RowIndex);
		}

		/// <summary>
		/// Determines whether the given array has content (not null and length > 0).
		/// </summary>
		/// <returns><c>true</c> if this instance is array content the specified arr; otherwise, <c>false</c>.</returns>
		/// <param name="arr">Arr.</param>
		protected bool IsArrayContent (Array arr)
		{
			return arr != null && arr.Length > 0;
		}

		public CellRange ()
		{
			this.RowIndex = null;
			this.ColumnIndex = null;
			this.ColumnNames = null;
		}

		public CellRange (int[] rowIndex, int[] columnIndex)
			: this ()
		{
			this.RowIndex = rowIndex;
			this.ColumnIndex = columnIndex;
		}

		public CellRange (int[] rowIndex, string[] columnNames)
			: this ()
		{
			this.RowIndex = rowIndex;
			this.ColumnNames = columnNames;
		}

		public CellRange (int[] rowIndex)
			: this (rowIndex, new int[]{ })
		{
		}

		public CellRange (int rowIndex)
			: this (new int[]{ rowIndex }, new int[]{ })
		{
		}

		public CellRange (int rowIndex, int columnIndex)
			: this (new int[]{ rowIndex }, new int[]{ columnIndex })
		{
		}

		public CellRange (int rowIndex, string columnName)
			: this (new int[]{ rowIndex }, new string[]{ columnName })
		{
		}
	}
}

