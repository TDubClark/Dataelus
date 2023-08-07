using System;

namespace Dataelus.Table
{
	/// <summary>
	/// Record value.
	/// </summary>
	public class RecordValue : IRecordValue
	{
		public RecordValue ()
			: this (-1, null, null)
		{
		}

		public RecordValue (string columnName, object value)
			: this (-1, columnName, value)
		{
		}

		public RecordValue (int columnId, object value)
			: this (columnId, null, value)
		{
		}

		public RecordValue (int columnId, string columnName, object value)
		{
			_columnID = columnId;
			_columnName = columnName;
			_value = value;
		}

		#region IRecordValue implementation

		protected int _columnID;

		public int ColumnID {
			get { return _columnID; }
			set { _columnID = value; }
		}

		protected string _columnName;

		public string ColumnName {
			get { return _columnName; }
			set { _columnName = value; }
		}

		protected object _value;

		public object Value {
			get { return _value; }
			set { _value = value; }
		}

		#endregion
	}
}

