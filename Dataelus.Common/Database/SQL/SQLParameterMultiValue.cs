using System;

namespace Dataelus.Database.SQL
{
	public class SQLParameterMultiValue : ISQLParameterMultiValue
	{
		public SQLParameterMultiValue ()
		{
		}

		public SQLParameterMultiValue (IDBField field, object[] values)
			: this ()
		{
			_field = field;
			_values = values;
		}

		#region ISqlParameterMultiValue implementation

		protected IDBField _field;

		public IDBField Field {
			get { return _field; }
			set { _field = value; }
		}

		protected object[] _values;

		public object[] Values {
			get { return _values; }
			set { _values = value; }
		}

		#endregion
	}
}

