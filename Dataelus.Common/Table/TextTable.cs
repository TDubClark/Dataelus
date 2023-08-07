using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Table class for storing text in columns.
	/// </summary>
	public class TextTable : ITable
	{
		/// <summary>
		/// Gets or sets the Text value at the specified rowIndex and columnIndex.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnIndex">Column index.</param>
		public string this [int rowIndex, int columnIndex] {
			get{ return _rows [rowIndex] [columnIndex]; }
			set{ _rows [rowIndex] [columnIndex] = value; }
		}

		protected ColumnCollection _columns;

		public ColumnCollection Columns {
			get { return _columns; }
			set { _columns = value; }
		}

		public int ColumnCount { 
			get { return _columns.Count; }
		}

		protected RowTextCollection _rows;

		public RowTextCollection Rows {
			get { return _rows; }
			set { _rows = value; }
		}

		public int RowCount { 
			get { return _rows.Count; }
		}

		/// <summary>
		/// Creates a new row.
		/// This is the preferred way to add rows to this table.
		/// </summary>
		/// <returns>The row.</returns>
		public RowText CreateRow ()
		{
			return new RowText (this, _columns.Count);
		}

		public Column AddColumn (string columnName)
		{
			return AddColumn (new Column (columnName));
		}

		public Column AddColumn (Column column)
		{
			var newCol = _columns.AddColumn (column);

			for (int i = 0; i < _rows.Count; i++) {
				_rows [i].Values.Add (null);
			}
			return newCol;
		}

		public bool RemoveColumn (string columnName)
		{
			int index = _columns.FindIndex (columnName);
			if (index < 0)
				return false;

			_columns.Items.RemoveAt (index);

			// Clear the row item
			for (int i = 0; i < _rows.Count; i++) {
				_rows [i].Values.RemoveAt (index);
			}
			return true;
		}

		public bool RemoveColumn (Column column)
		{
			return RemoveColumn (column.ColumnName);
		}

		public void AddRow (RowText row)
		{
			_rows.Add (row);
		}

		/// <summary>
		/// Whether the given column name is contained in this table.
		/// </summary>
		/// <returns><c>true</c>, if column was containsed, <c>false</c> otherwise.</returns>
		/// <param name="columnName">Column name.</param>
		public bool ContainsColumn (string columnName)
		{
			return FindColumnIndex (columnName) >= 0;
		}

		public int FindColumnIndex (string columnName)
		{
			return _columns.FindIndex (columnName);
		}

		EditTrackingState _editTracking = EditTrackingState.NotTracking;

		public bool IsTrackingEdits ()
		{
			return (_editTracking == EditTrackingState.Tracking);
		}

		public void StartEditTracking ()
		{
			_editTracking = EditTrackingState.Tracking;
		}

		public void StopEditTracking ()
		{
			_editTracking = EditTrackingState.NotTracking;
		}

		public void StopEditTracking (bool clearEditState)
		{
			StopEditTracking ();
			if (clearEditState) {
				ClearEditState ();
			}
		}

		public void ClearEditState ()
		{
			for (int r = 0; r < _rows.Count; r++) {
				_rows [r].EditState = RowEditState.Undefined;
			}
		}

		public void RemoveRowAt (int index)
		{
			_rows.Items.RemoveAt (index);
		}

		public TextTable ()
		{
			_columns = new ColumnCollection ();
			_rows = new RowTextCollection ();
		}

		public TextTable (TextTable other)
		{
			_columns = new ColumnCollection (other.Columns);
			_rows = new RowTextCollection (this, other.Rows);
		}
	}
}

