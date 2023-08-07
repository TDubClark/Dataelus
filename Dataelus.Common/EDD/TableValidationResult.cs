using System;

namespace Dataelus.EDD
{
	/// <summary>
	/// Table validation result, per row and column.
	/// </summary>
	public class TableValidationResult : ITableValidationResult
	{
		public TableValidationResult ()
			: this (-1, -1, null, false, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.EDD.TableValidationResult"/> class.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnIndex">Column index.</param>
		/// <param name="valid">If set to <c>true</c> valid.</param>
		/// <param name="message">Message.</param>
		public TableValidationResult (int rowIndex, int columnIndex, bool valid, string message)
			: this (rowIndex, columnIndex, null, valid, message)
		{
		}

		public TableValidationResult (int rowIndex, string columnName, bool valid, string message)
			: this (rowIndex, -1, columnName, valid, message)
		{
		}

		public TableValidationResult (int rowIndex, int columnIndex, string columnName, bool valid, string message)
		{
			this._valid = valid;
			this._message = message;
			this._rowIndex = rowIndex;
			this._columnIndex = columnIndex;
			this._columnName = columnName;
		}

		public TableValidationResult (ITableValidationResult other)
			: this (other.RowIndex, other.ColumnIndex, other.ColumnName, other.Valid, other.Message)
		{
		}

		public override string ToString ()
		{
			return string.Format ("[TableValidationResult: _valid={0}, _rowIndex={1}, _columnIndex={2}]", _valid, _rowIndex, _columnIndex);
		}

		#region ITableValidationResult implementation

		protected bool _valid;

		public bool Valid {
			get { return _valid; }
			set { _valid = value; }
		}

		protected string _message;

		public string Message {
			get { return _message; }
			set { _message = value; }
		}

		protected int _rowIndex;

		public int RowIndex {
			get { return _rowIndex; }
			set { _rowIndex = value; }
		}

		protected int _columnIndex;

		public int ColumnIndex {
			get { return _columnIndex; }
			set { _columnIndex = value; }
		}

		protected string _columnName;

		public string ColumnName {
			get { return _columnName; }
			set { _columnName = value; }
		}

		#endregion
	}
}

