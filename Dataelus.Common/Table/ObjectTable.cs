using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Table
{
	/// <summary>
	/// Table of Objects.
	/// </summary>
	public class ObjectTable : ITable, IEquatable<ObjectTable>, IEquatable<ObjectTable, String>
	{
		/// <summary>
		/// Gets or sets the object value at the specified rowIndex and columnIndex.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnIndex">Column index.</param>
		public object this [int rowIndex, int columnIndex] {
			get { return _rows [rowIndex] [columnIndex]; }
			set { _rows [rowIndex] [columnIndex] = value; }
		}

		/// <summary>
		/// Gets or sets the object value at the specified rowIndex and columnName.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		public object this [int rowIndex, string columnName] {
			get { return _rows [rowIndex] [FindColumnIndex (columnName)]; }
			set { _rows [rowIndex] [FindColumnIndex (columnName)] = value; }
		}


		#region Serialization Methods

		/// <summary>
		/// Assigns the parent table (this) to every row.
		/// </summary>
		protected void AssignRowsParentTable ()
		{
			if (_rows != null) {
				for (int i = 0; i < _rows.Count; i++) {
					_rows [i].ParentTable = this;
				}
			}
			if (_rowsDeleted != null) {
				for (int i = 0; i < _rowsDeleted.Count; i++) {
					_rowsDeleted [i].ParentTable = this;
				}
			}
		}

		/// <summary>
		/// Run this function after deserialization.
		/// </summary>
		public void RunPostDeserialization ()
		{
			AssignRowsParentTable ();
		}

		#endregion


		protected ObjectColumnCollection _columns;

		/// <summary>
		/// Gets or sets the columns.
		/// </summary>
		/// <value>The columns.</value>
		public ObjectColumnCollection Columns {
			get { return _columns; }
			set { _columns = value; }
		}

		/// <summary>
		/// Gets the column count.
		/// </summary>
		/// <value>The column count.</value>
		public int ColumnCount { 
			get { return _columns.Count; }
		}

		protected ObjectRowCollection _rows;

		/// <summary>
		/// Gets or sets the rows.
		/// </summary>
		/// <value>The rows.</value>
		public ObjectRowCollection Rows {
			get { return _rows; }
			set {
				_rows = value;
				AssignRowsParentTable ();
			}
		}

		protected ObjectRowCollection _rowsDeleted;

		/// <summary>
		/// Gets or sets the rows deleted.
		/// </summary>
		/// <value>The rows deleted.</value>
		public ObjectRowCollection RowsDeleted {
			get { return _rowsDeleted; }
			set { _rowsDeleted = value; }
		}

		protected RowDeletedAction _rowDeletedBehavior;

		/// <summary>
		/// Gets or sets the row deleted behavior: what happens to a row when it is deleted (default = move to deleted list).
		/// Only applies if tracking edits.
		/// </summary>
		/// <value>The row deleted behavior.</value>
		public RowDeletedAction RowDeletedBehavior {
			get { return _rowDeletedBehavior; }
			set { _rowDeletedBehavior = value; }
		}

		/// <summary>
		/// Gets the row count.
		/// </summary>
		/// <value>The row count.</value>
		public int RowCount { 
			get { return _rows.Count; }
		}

		public RecordValueCollection GetRecordValues (int rowIndex)
		{
			var valueList = new RecordValueCollection (rowIndex);
			var record = _rows [rowIndex];
			for (int i = 0; i < record.Values.Count; i++) {
				var col = this.Columns [i];
				valueList.Add (new RecordValue (col.ColumnID, col.ColumnName, record.Values [i]));
			}

			return valueList;
		}

		/// <summary>
		/// Creates a new row.
		/// This is the preferred way to add rows to this table.
		/// </summary>
		/// <returns>The row.</returns>
		public ObjectRow CreateRow ()
		{
			return new ObjectRow (this, _columns.Count);
		}

		/// <summary>
		/// Adds the column.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		public void AddColumn (string columnName)
		{
			AddColumn (new ObjectColumn (columnName));
		}

		/// <summary>
		/// Adds the column.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="dataType">Data type.</param>
		public void AddColumn (string columnName, Type dataType)
		{
			AddColumn (new ObjectColumn (columnName, dataType));
		}

		/// <summary>
		/// Adds the column.
		/// </summary>
		/// <param name="column">Column object.</param>
		public ObjectColumn AddColumn (ObjectColumn column)
		{
			var newCol = _columns.AddColumn (column);

			for (int i = 0; i < _rows.Count; i++) {
				_rows [i].Values.Add (null);
			}
			for (int i = 0; i < _rowsDeleted.Count; i++) {
				_rowsDeleted [i].Values.Add (null);
			}
			return newCol;
		}

		/// <summary>
		/// Removes the column.
		/// </summary>
		/// <returns><c>true</c>, if column was removed, <c>false</c> otherwise.</returns>
		/// <param name="columnName">Column name.</param>
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

		/// <summary>
		/// Removes the column.
		/// </summary>
		/// <returns><c>true</c>, if column was removed, <c>false</c> otherwise.</returns>
		/// <param name="column">Column.</param>
		public bool RemoveColumn (ObjectColumn column)
		{
			return RemoveColumn (column.ColumnName);
		}

		/// <summary>
		/// Adds the row.
		/// </summary>
		/// <param name="row">Row.</param>
		public ObjectRow AddRow (ObjectRow row)
		{
			if (IsTrackingEdits ())
				row.EditState = RowEditState.Inserted;
			_rows.Add (row);
			return row;
		}

		public void InsertRow (int index, ObjectRow row)
		{
			if (IsTrackingEdits ())
				row.EditState = RowEditState.Inserted;
			_rows.Insert (index, row);
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

		/// <summary>
		/// Finds the index of the column.
		/// </summary>
		/// <returns>The column index.</returns>
		/// <param name="columnName">Column name.</param>
		public int FindColumnIndex (string columnName)
		{
			return _columns.FindIndex (columnName);
		}

		public int FindColumnIndexByID (int columnID)
		{
			return _columns.FindIndexByID (columnID);
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

		/// <summary>
		/// Determines whether any edits have been tracked.
		/// </summary>
		/// <returns><c>true</c> if this instance is edits; otherwise, <c>false</c>.</returns>
		public bool IsEdits ()
		{
			if (IsTrackingEdits ()) {
				return _rows.Any (x => x.IsEdited ());
			}
			return false;
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

		public IEqualityComparer<object> GetEqualityComparer (int columnIndex)
		{
			return _columns [columnIndex].EqualityComparer ?? EqualityComparer<object>.Default;
		}

		/// <summary>
		/// Removes the row at the given index.
		/// </summary>
		/// <param name="index">Index.</param>
		public void RemoveRowAt (int index)
		{
			_rows.RemoveAt (index);
		}

		/// <summary>
		/// Deletes the row at the given index (marks as deleted, if tracking edits).
		/// </summary>
		/// <param name="index">Index.</param>
		public void DeleteRow (int index)
		{
			bool isRemove = true;

			if (this.IsTrackingEdits ()) {
				// First, move to the deleted row (if applicable)

				// Mark as a deleted row
				var item = _rows.Items [index];
				item.EditState = RowEditState.Deleted;

				switch (_rowDeletedBehavior) {
					case RowDeletedAction.MarkDeletedOnly:
					// Do not remove from the list
						isRemove = false;
						break;
					case RowDeletedAction.MoveToDeletedList:
						_rowsDeleted.Add (item);
						break;
					case RowDeletedAction.RemoveCompletely:
						break;
					default:
						break;
				}
			}

			if (isRemove)
				RemoveRowAt (index);
		}

		/// <summary>
		/// Occurs when this row is changed.
		/// </summary>
		public event RowChangedEventHandler RowChanged;

		/// <summary>
		/// Raises the row changed event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void OnRowChanged (ObjectRowChangedEventArgs args)
		{
			if (RowChanged != null) {
				RowChanged (this, args);
			}
		}

		/// <summary>
		/// Occurs when (immediately before) a row is added.
		/// </summary>
		public event RowAddedEventHandler RowAdded;

		/// <summary>
		/// Raises the row added event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void OnRowAdded (ObjectRowAddedEventArgs args)
		{
			if (RowAdded != null) {
				RowAdded (this, args);
			}
		}

		/// <summary>
		/// Occurs when (immediately before) a row is deleted.
		/// </summary>
		public event RowDeletedEventHandler RowDeleted;

		/// <summary>
		/// Raises the row deleted event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void OnRowDeleted (ObjectRowDeletedEventArgs args)
		{
			if (RowDeleted != null) {
				RowDeleted (this, args);
			}
		}

		void _rows_RowChanged (object sender, ObjectRowChangedEventArgs args)
		{
			OnRowChanged (args);
		}

		void _rows_RowAdded (object sender, ObjectRowAddedEventArgs args)
		{
			OnRowAdded (args);
		}

		void _rows_RowDeleted (object sender, ObjectRowDeletedEventArgs args)
		{
			OnRowDeleted (args);
		}

		public ObjectTable ()
			: this (new ObjectColumnCollection (), new ObjectRowCollection ())
		{
		}

		/// <summary>
		/// Copy Constructor.
		/// Initializes a new instance of the <see cref="Dataelus.Table.ObjectTable"/> class.
		/// </summary>
		/// <param name="other">Other.</param>
		public ObjectTable (ObjectTable other)
			: this (new ObjectColumnCollection (other.Columns), null)
		{
			_rows = new ObjectRowCollection (this, other.Rows);
			_rows.RowAdded += _rows_RowAdded;
			_rows.RowDeleted += _rows_RowDeleted;
			_rows.RowChanged += _rows_RowChanged;
			_rowsDeleted = new ObjectRowCollection (this, other.RowsDeleted);
			_rowDeletedBehavior = RowDeletedAction.MoveToDeletedList;
		}

		public ObjectTable (ObjectColumnCollection columns, ObjectRowCollection rows)
		{
			_columns = columns;
			_rows = rows;
			_rows.RowAdded += _rows_RowAdded;
			_rows.RowDeleted += _rows_RowDeleted;
			_rowsDeleted = new ObjectRowCollection ();
			_rowDeletedBehavior = RowDeletedAction.MoveToDeletedList;
		}


		/// <summary>
		/// Adds (appends) the records from the given table to this table.
		/// Uses column names to match tables.
		/// </summary>
		/// <param name="table">Table.</param>
		/// <param name="logger">The error logger</param>
		public void AddTableRecords (ObjectTable table, Dataelus.Log.IErrorLogger logger)
		{
			var indexMapper = new Dictionary<int, int> ();

			// Add the columns to the new table
			foreach (var column in table.Columns) {
				int colIndex = _columns.FindIndex (column.ColumnName);
				if (colIndex < 0)
					continue;

				// Key=This column; Value=other column
				indexMapper.Add (colIndex, column.ColumnIndex);
			}

			foreach (var row in table.Rows) {
				var newRow = this.CreateRow ();
				foreach (var colmap in indexMapper) {
					try {
						var value = row.Values [colmap.Value];
						newRow.Values [colmap.Key] = value;
					} catch (Exception ex) {
						logger.LogError ("ObjectTable", "AddTableRecords", String.Format ("Mapping index {0:d}/{2:D} => {1:d}/{3:d}; Exception: {4}", colmap.Key, colmap.Value, row.Values.Count, newRow.Values.Count, ex.Message));
					}
				}
				_rows.Add (newRow);
			}
		}


		/// <summary>
		/// Gets the sub table, using the given column names and the given predicate.
		/// </summary>
		/// <returns>The sub table.</returns>
		/// <param name="columnNames">Column names.</param>
		/// <param name="filter">Filter.</param>
		public ObjectTable GetSubTable (IEnumerable<string> columnNames, Predicate<ObjectRow> rowFilter)
		{
			var tbl = new ObjectTable ();

			// Create a mapping from the column index of each target table column and this table column
			// Mapper:
			//   Key   = target table column index
			//   Value = this   table column index
			var indexMapper = new Dictionary<int, int> ();

			// Add the columns to the new table
			foreach (var columnName in columnNames) {
				int colIndex = _columns.FindIndex (columnName);
				if (colIndex < 0)
					continue;
				// Add column and stor index
				indexMapper.Add (tbl.AddColumn (new ObjectColumn (_columns [colIndex])).ColumnIndex, colIndex);
			}

			// Populate each row
			foreach (var row in _rows) {
				if (rowFilter (row)) {
					var newRow = tbl.CreateRow ();

					foreach (var imap in indexMapper) {
						newRow [imap.Key] = row [imap.Value];
					}

					tbl.AddRow (newRow);
				}
			}

			return tbl;
		}

		/// <summary>
		/// Maps the table to a new one-column TextTable.
		/// </summary>
		/// <returns>The table text.</returns>
		/// <param name="newColumn">New column.</param>
		/// <param name="valueMapper">Value mapper.</param>
		public TextTable MapTableText (Column newColumn, Func<ObjectRow, string> valueMapper)
		{
			var tbl = new TextTable ();

			tbl.AddColumn (newColumn);

			foreach (var row in _rows) {
				var newRow = tbl.CreateRow ();

				newRow.Values [newColumn.ColumnIndex] = valueMapper (row);

				tbl.AddRow (newRow);
			}

			return tbl;
		}

		/// <summary>
		/// Maps the table.
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="newColumnName">New column name.</param>
		/// <param name="newColumnType">New column type.</param>
		/// <param name="valueMapper">Value mapper.</param>
		public ObjectTable MapTable (string newColumnName, Type newColumnType, Func<ObjectRow, object> valueMapper)
		{
			return MapTable (new ObjectColumn (newColumnName, newColumnType), valueMapper);
		}

		/// <summary>
		/// Maps the table to a new one-column table.
		/// </summary>
		/// <returns>The table.</returns>
		/// <param name="newColumn">New column.</param>
		/// <param name="valueMapper">Value mapper.</param>
		public ObjectTable MapTable (ObjectColumn newColumn, Func<ObjectRow, object> valueMapper)
		{
			var tbl = new ObjectTable ();

			tbl.AddColumn (newColumn);

			foreach (var row in _rows) {
				var newRow = tbl.CreateRow ();

				newRow.Values [newColumn.ColumnIndex] = valueMapper (row);

				tbl.AddRow (newRow);
			}

			return tbl;
		}

		#region IEquatable implementation

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectTable"/> is equal to the current <see cref="Dataelus.Table.ObjectTable"/>.
		/// </summary>
		/// <param name="other">The <see cref="Dataelus.Table.ObjectTable"/> to compare with the current <see cref="Dataelus.Table.ObjectTable"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Dataelus.Table.ObjectTable"/> is equal to the current
		/// <see cref="Dataelus.Table.ObjectTable"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (ObjectTable other)
		{
			return Equals (other, new StringEqualityComparer ());
		}

		#endregion

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectTable"/> is equal to the current <see cref="Dataelus.Table.ObjectTable"/>.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="comparer">Comparer.</param>
		public bool Equals (ObjectTable other, IEqualityComparer<string> comparer)
		{
			return this.Columns.Equals (other.Columns, comparer)
			&& this.Rows.Equals (other.Rows);
		}
	}
}

