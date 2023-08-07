using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// SQL field value.
	/// </summary>
	public class SQLFieldValue
	{
		/// <summary>
		/// Gets or sets the field.
		/// </summary>
		/// <value>The field.</value>
		public IDBField Field { get; set; }

		/// <summary>
		/// Gets or sets the parameter.
		/// </summary>
		/// <value>The parameter.</value>
		public ParamValue Parameter { get; set; }

		public SQLFieldValue ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLFieldValue"/> class.
		/// </summary>
		/// <param name="field">Field.</param>
		/// <param name="parameter">Parameter.</param>
		public SQLFieldValue (IDBField field, ParamValue parameter)
		{
			this.Field = field;
			this.Parameter = parameter;
		}
	}

	public class SQLFieldValueCollection : ListBase<SQLFieldValue>
	{
		public SQLFieldValueCollection ()
			: base ()
		{
		}

		public SQLFieldValueCollection (IEnumerable<SQLFieldValue> collection)
			: base (collection)
		{
		}

		/// <summary>
		/// Gets a new instance which excludes parameters with a Null value
		/// </summary>
		/// <returns>The without null values.</returns>
		public SQLFieldValueCollection GetWithoutNullValues ()
		{
			return new SQLFieldValueCollection (_items.Where (x => x.Parameter.Value != null).ToArray ());
		}
	}
}

