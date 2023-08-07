using System;

namespace Dataelus.Database.SQL
{
	public class SQLParameterValue : ISQLParameterValue
	{
		public SQLParameterValue ()
		{
		}

		public SQLParameterValue (IDBField field, object value)
			: this ()
		{
			_field = field;
			_value = value;
		}

		#region ISqlParameterValue implementation

		protected IDBField _field;

		public IDBField Field {
			get { return _field; }
			set { _field = value; }
		}

		protected object _value;

		public object Value {
			get { return _value; }
			set { _value = value; }
		}

		#endregion
	}
}

