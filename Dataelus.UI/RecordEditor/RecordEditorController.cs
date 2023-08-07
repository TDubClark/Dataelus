using System;

namespace Dataelus.UI.RecordEditor
{
	public class RecordEditorController : IRecordEditorController
	{
		protected Dataelus.Table.ObjectTable _data;

		protected IRecordEditorView _viewObject;

		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		public IRecordEditorView ViewObject {
			get { return _viewObject; }
			set { _viewObject = value; }
		}

		protected int _rowIndex;

		/// <summary>
		/// Gets or sets the index of the row.
		/// </summary>
		/// <value>The index of the row.</value>
		public int RowIndex {
			get { return _rowIndex; }
			set { _rowIndex = value; }
		}

		public RecordEditorController (Table.ObjectTable data, int rowIndex)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			if (rowIndex < 0)
				throw new ArgumentOutOfRangeException ("rowIndex", String.Format ("index cannot be less than zero (value={0:d})", rowIndex));
			
			_data = data;
			_rowIndex = rowIndex;
		}

		protected virtual void SaveToDatabase ()
		{
			// Save the data to the database
			throw new NotImplementedException ();
		}

		#region IRecordEditorController implementation

		public virtual void LoadView ()
		{
			_viewObject.LoadFormRow (_data.Columns
				, new System.Collections.Generic.Dictionary<string, string> ()
				, null, _data, _rowIndex);
		}

		public virtual void Save ()
		{
			_viewObject.UnloadFormDataRow (_data, _rowIndex);
			SaveToDatabase ();
		}

		public virtual bool ValidateValue (string fieldName, object value, out string message)
		{
			message = null;
			try {
				_data [_rowIndex, fieldName] = value;
				return true;
			} catch (FormatException fex) {
				message = fex.Message;
			}
			return false;
		}

		public virtual bool ValidateValue (int fieldID, object value, out string message)
		{
			int index = _data.FindColumnIndexByID (fieldID);
			if (index < 0) {
				message = String.Format ("Field not found (field ID={0:d})", fieldID);
				return false;
			}
			int columnIndex = _data.Columns [index].ColumnIndex;

			return ValidateValueByColumnIndex (columnIndex, value, out message);
		}

		public object RecordData {
			get { return _data; }
			set { _data = (Dataelus.Table.ObjectTable)value; }
		}

		#endregion

		protected virtual bool ValidateValueByColumnIndex (int columnIndex, object value, out string message)
		{
			try {
				_data [_rowIndex, columnIndex] = value;
				message = null;
				return true;
			} catch (FormatException fex) {
				message = fex.Message;
			}
			return false;
		}
	}
}

