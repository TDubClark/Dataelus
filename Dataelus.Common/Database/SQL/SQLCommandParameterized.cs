using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Database.SQLLanguage;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// A parameterized SQL command for SQL Server or any RDBMS which can accept named parameters.
	/// </summary>
	public class SQLCommandParameterized : ISQLCommandParameterized
	{
		/// <summary>
		/// Gets or sets the sql where clauses.
		/// </summary>
		/// <value>The sql where clauses.</value>
		public List<SQLWhereClause> SqlWhereClauses { get; set; }

		/// <summary>
		/// Gets or sets the command parameters.
		/// </summary>
		/// <value>The command parameters.</value>
		public List<SQLCommandParameter> CommandParameters { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLCommandParameterized"/> class.
		/// </summary>
		public SQLCommandParameterized ()
		{
			CommandParameters = new List<SQLCommandParameter> ();
			SqlWhereClauses = new List<SQLWhereClause> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLCommandParameterized"/> class.
		/// </summary>
		/// <param name="values">The SQL Command Value Parameters - an object with both Single and Multi-value parameters, with their associated values.</param>
		public SQLCommandParameterized (ISQLCommandValueParameters values, SQLParameterNameBuilder prmBuilder)
			: this ()
		{
			GetSQLPart (values, prmBuilder);
		}

		public SQLCommandParameterized (string tableName, Dictionary<string, object> fieldValues, Database.DBFieldCollection dbFields)
			: this (tableName, fieldValues, dbFields, new SQLParameterNameBuilder ())
		{
		}

		public SQLCommandParameterized (string tableName, Dictionary<string, object> fieldValues, Database.DBFieldCollection dbFields, SQLParameterNameBuilder prmBuilder)
			: this (new SQLCommandValueParameters (tableName, fieldValues, dbFields), prmBuilder)
		{
		}

		/// <summary>
		/// Constructor for a set of parameters on a single table.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldValues">Field values.</param>
		/// <param name="fieldMultiValues">Field multi values.</param>
		/// <param name="dbFields">Db fields.</param>
		public SQLCommandParameterized (string tableName, Dictionary<string, object> fieldValues, Dictionary<string, object[]> fieldMultiValues, Database.DBFieldCollection dbFields)
			: this (tableName, fieldValues, fieldMultiValues, dbFields, new SQLParameterNameBuilder ())
		{
		}

		public SQLCommandParameterized (string tableName, Dictionary<string, object> fieldValues, Dictionary<string, object[]> fieldMultiValues, Database.DBFieldCollection dbFields, SQLParameterNameBuilder prmBuilder)
			: this (new SQLCommandValueParameters (tableName, fieldValues, fieldMultiValues, dbFields), prmBuilder)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLCommandParameterized"/> class.
		/// </summary>
		/// <param name="values">The SQL Command Value Parameters - an object with both Single and Multi-value parameters, with their associated values.</param>
		/// <param name="fieldBuilder">The Field builder delegate.</param>
		/// <param name="includeSchema">If set to <c>true</c> include schema.</param>
		public SQLCommandParameterized (ISQLCommandValueParameters values, SQLFieldBuilder fieldBuilder, bool includeSchema, SQLParameterNameBuilder prmBuilder)
			: this ()
		{
			GetSQLPart (values, fieldBuilder, includeSchema, prmBuilder);
		}

		public List<string> GetCommandParamFieldNames ()
		{
			return this.CommandParameters.Select (x => x.Field.FieldName).ToList ();
		}

		public List<string> GetCommandParameterNames ()
		{
			return this.CommandParameters.Select (x => x.Parameters.First ().ParameterName).ToList ();
		}

		/// <summary>
		/// Gets the field values assigned only; using only the first parameter (or Null if no parameters).
		/// </summary>
		/// <returns>The field values.</returns>
		public SQLFieldValueCollection GetFieldValues ()
		{
			return new SQLFieldValueCollection (this.CommandParameters.Select (x => new SQLFieldValue (x.Field, x.Parameters.FirstOrDefault ())).ToList ());
		}

		/// <summary>
		/// Gets the field values assigned only; using only the first parameter.
		/// </summary>
		/// <returns>The field values assigned only.</returns>
		public SQLFieldValueCollection GetFieldValuesAssignedOnly ()
		{
			return new SQLFieldValueCollection (this.CommandParameters.Where (x => x.HasParameters ()).Select (x => new SQLFieldValue (x.Field, x.Parameters.First ())).ToList ());
		}

		/// <summary>
		/// Gets the sql parts and finalized SQL Command Parameters.
		/// </summary>
		/// <param name="values">Values.</param>
		public virtual void GetSQLPart (ISQLCommandValueParameters values, SQLParameterNameBuilder prmBuilder)
		{
			GetSQLPart (values, new SQLFieldBuilder (new SQLBuilder ().GetSQLField), false, prmBuilder);
		}

		/// <summary>
		/// Gets the sql parts and finalized SQL Command Parameters.
		/// </summary>
		/// <param name="values">The SQL Command Value Parameters - an object with both Single and Multi-value parameters, with their associated values.</param>
		/// <param name="fieldBuilder">Field builder.</param>
		/// <param name="includeSchema">If set to <c>true</c> include schema.</param>
		public virtual void GetSQLPart (ISQLCommandValueParameters values, SQLFieldBuilder fieldBuilder, bool includeSchema, SQLParameterNameBuilder prmBuilder)
		{
			CommandParameters = new List<SQLCommandParameter> ();

			SqlWhereClauses = new List<SQLWhereClause> ();

			string paramBase = "@prm";

			// Use a Field Index, to number each field for which we have parameterized value(s)
			int fieldIndex = 0;

			foreach (var item in values.MultiValueParams) {
				// A single database field (Schema.Table.Field), for which we have multiple values ... WHERE _ IN ()
				var field = fieldBuilder (item.Field, includeSchema);

				var prm = new SQLCommandParameter (item.Field);
				for (int i = 0; i < item.Values.Length; i++) {
					string paramName = prmBuilder.GetUnique (String.Format ("{0}F{1}V{2}", paramBase, fieldIndex, i));
					prm.Parameters.Add (new ParamValue (paramName, item.Values [i]));
				}
				CommandParameters.Add (prm);

				SqlWhereClauses.Add (new SQLWhereClause (item.Field,
						String.Format ("{0} IN ({1})", new Object[] { field, String.Join (", ", prm.GetParameterNames ()) })
					));
				fieldIndex++;
			}

			foreach (var item in values.ValueParams) {
				// A single database field (Schema.Table.Field), for which we have multiple values ... WHERE _ = ''
				var field = fieldBuilder (item.Field, includeSchema);

				string paramName = prmBuilder.GetUnique (String.Format ("{0}F{1}", paramBase, fieldIndex));
				CommandParameters.Add (new SQLCommandParameter (item.Field, paramName, item.Value));


				string sql;
				if (item.Value == null)
					sql = String.Format ("{0} IS NULL", field);
				else
					sql = String.Format ("{0} = {1}", field, paramName);
				
				SqlWhereClauses.Add (new SQLWhereClause (item.Field, sql));
				fieldIndex++;
			}
		}

		/// <summary>
		/// Gets the SQL where clause (with all components joined by "AND")
		/// </summary>
		/// <returns>The SQL where clause.</returns>
		public string GetSQLWhereClause ()
		{
			return String.Join (" AND ", this.SqlWhereClauses.Select (x => x.Sql).ToList ());
		}
	}

	/// <summary>
	/// Delegate which builds a single SQL field (ex: "[SCHEMA].[TABLE].[FIELD]").
	/// </summary>
	public delegate string SQLFieldBuilder (IDBField field, bool includeSchema);
}

