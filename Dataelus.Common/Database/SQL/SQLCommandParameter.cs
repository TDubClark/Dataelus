using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// Defines one or more parameterized values for a SQL command, on a single field
	/// </summary>
	public class SQLCommandParameter
	{
		public IDBField Field{ get; set; }

		public List<ParamValue> Parameters{ get; set; }

		/// <summary>
		/// Determines whether this instance has one or more parameters.
		/// </summary>
		/// <returns><c>true</c> if this instance has parameters; otherwise, <c>false</c>.</returns>
		public bool HasParameters ()
		{
			return (this.Parameters.Count > 0);
		}

		public SQLCommandParameter ()
		{
			Parameters = new List<ParamValue> ();
		}

		public SQLCommandParameter (IDBField field)
			: this ()
		{
			Field = field;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.Sql.SqlCommandParameter"/> class.
		/// Adds a single Parameter Name/Value Pair
		/// </summary>
		/// <param name="field">Field.</param>
		/// <param name="parameterName">Parameter name.</param>
		/// <param name="value">Value.</param>
		public SQLCommandParameter (IDBField field, string parameterName, object value)
			: this (field)
		{
			AddParameter (parameterName, value);
		}

		public string FieldDataType{ get { return Field.DataType; } }

		public int FieldMaxLen{ get { return Field.MaxLength; } }

		public bool FieldNullable{ get { return Field.Nullable; } }

		/// <summary>
		/// Adds the parameter.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="value">Value.</param>
		public void AddParameter (string name, object value)
		{
			Parameters.Add (new ParamValue (name, value));
		}

		/// <summary>
		/// Gets the parameter names.
		/// </summary>
		/// <returns>The parameter names.</returns>
		public List<string> GetParameterNames ()
		{
			return Parameters.Select (x => x.ParameterName).ToList ();
		}
	}

	public class ParamValue
	{
		public ParamValue (string paramName, object value)
		{
			ParameterName = paramName;
			Value = value;
		}

		public string ParameterName{ get; set; }

		public object Value{ get; set; }
	}
}

