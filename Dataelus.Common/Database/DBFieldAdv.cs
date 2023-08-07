using System;

namespace Dataelus.Database
{
	public class DBFieldAdv : DBField, IDBFieldAdv
	{
		public DBFieldAdv ()
			: base ()
		{
			_autoNumber = false;
		}

		public DBFieldAdv (IDBField other)
			: base (other)
		{
			_autoNumber = false;
		}

		public DBFieldAdv (string tableName, string fieldName, string dataType, int maxLength, bool nullable, int order)
			: base (tableName, fieldName, dataType, maxLength, nullable, order)
		{
			_autoNumber = false;
		}

		public DBFieldAdv (string schemaName, string tableName, string fieldName, string dataType, int maxLength, bool nullable, int order)
			: base (schemaName, tableName, fieldName, dataType, maxLength, nullable, order)
		{
			_autoNumber = false;
		}

		#region IDBFieldAdv implementation

		protected bool _autoNumber;

		/// <summary>
		/// Gets or sets whether this field is populated by an auto number.
		/// </summary>
		/// <value>true</value>
		/// <c>false</c>
		public bool AutoNumber {
			get { return _autoNumber; }
			set { _autoNumber = value; }
		}

		#endregion
	}
}

