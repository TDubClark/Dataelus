using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Database.SQLLanguage;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// Sql command value parameters (single values and multi-values).
	/// </summary>
	public class SQLCommandValueParameters : ISQLCommandValueParameters
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLCommandValueParameters"/> class.
		/// </summary>
		public SQLCommandValueParameters ()
		{
			ValueParams = new SQLParameterValueCollection ();
			MultiValueParams = new SQLParameterMultiValueCollection ();
		}

		public SQLCommandValueParameters (string tableName, Dictionary<string, object> fieldValues, Database.DBFieldCollection dbFields)
			: this ()
		{
			if (fieldValues != null) {
				foreach (var item in fieldValues) {
					ValueParams.Add (new SQLParameterValue (dbFields.Find (tableName, item.Key), item.Value));
				}
			}
		}

		public SQLCommandValueParameters (string tableName, Dictionary<string, object> fieldValues, Dictionary<string, object[]> fieldMultiValues, Database.DBFieldCollection dbFields)
			: this ()
		{
			if (fieldValues != null) {
				foreach (var item in fieldValues) {
					ValueParams.Add (new SQLParameterValue (dbFields.Find (tableName, item.Key), item.Value));
				}
			}
			if (fieldMultiValues != null) {
				foreach (var item in fieldMultiValues) {
					MultiValueParams.Add (new SQLParameterMultiValue (dbFields.Find (tableName, item.Key), item.Value));
				}
			}
		}

		/// <summary>
		/// Gets or sets the value parameters.
		/// Represents database fields, each with a single value.
		/// </summary>
		/// <value>The value parameters.</value>
		public SQLParameterValueCollection ValueParams { get; set; }

		/// <summary>
		/// Gets or sets the multi value parameters.
		/// Represents database fields, each with multiple values.
		/// </summary>
		/// <value>The multi value parameters.</value>
		public SQLParameterMultiValueCollection MultiValueParams { get; set; }
	}
	
}
