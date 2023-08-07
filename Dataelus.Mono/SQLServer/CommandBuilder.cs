using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

using Dataelus.Database.SQL;

namespace Dataelus.Mono.SQLServer
{
	public class CommandBuilder : SQLBuilder, ISQLServerCommandBuilder
	{
		public Database.DBFieldCollection DatabaseFields { get; set; }

		public CommandBuilder (Dataelus.Database.DBFieldCollection databaseFields)
			: this ()
		{
			if (databaseFields == null)
				throw new ArgumentNullException ("databaseFields");
			this.DatabaseFields = databaseFields;
		}

		public CommandBuilder ()
		{
		}

		#region ISQLServerCommandBuilder implementation

		public SqlCommand GetCommandInsert (string tableName, SQLCommandParameterized cmdParam, Dictionary<string, object> insertValues)
		{
			return GetCommandInsert (tableName, cmdParam);
		}

		public SqlCommand GetCommandUpdate (string tableName, SQLCommandParameterized cmdParamValue, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions, Dictionary<string, object> updateValues)
		{
			return GetCommandUpdateSet (tableName, cmdParamValue, cmdParamCondition);
		}

		public SqlCommand GetCommandDelete (string tableName, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions)
		{
			return GetCommandDelete (tableName, cmdParamCondition);
		}

		public SqlCommand GetCommandSelect (string tableName, IEnumerable<string> fieldNames, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions)
		{
			return GetCommandSelect (tableName, false, fieldNames, cmdParamCondition);
		}

		public SqlCommand GetCommandSelectCount (string tableName, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions)
		{
			// Build the WHERE-clause parameters
			SQLFieldValueCollection whereFieldValues = cmdParamCondition.GetFieldValuesAssignedOnly ();

			string whereList = String.Join (" AND ", GetSQLEqualityStatements (whereFieldValues));

			var tableName2 = Enbracket (tableName);

			// Create SQL
			string sql = String.Format ("SELECT COUNT(*) FROM {0} WHERE {1}", tableName2, whereList);

			// Create command object
			var command = new SqlCommand (sql);

			command.AddParameters (whereFieldValues.GetWithoutNullValues ());

			return command;
		}

		#endregion

		public SqlCommand GetCommandDelete (string tableName, Dictionary<string, object> conditions)
		{
			SQLCommandParameterized commandParametersConditions = new SQLCommandParameterized (tableName, conditions, this.DatabaseFields);

			return GetCommandDelete (tableName, commandParametersConditions);
		}

		public SqlCommand GetCommandDelete (string tableName, SQLCommandParameterized cmdParamCondition)
		{
			// Build SQL statement
			SQLFieldValueCollection whereFieldValues;
			string sql = GetSQLDeleteSingleTable (tableName, cmdParamCondition, out whereFieldValues);

			// Build command object
			var command = new SqlCommand (sql);

			// Add parameters
			command.AddParameters (whereFieldValues.GetWithoutNullValues ());

			return command;
		}

		/// <summary>
		/// Gets the INSERT INTO [table] ( _ ) VALUES ( _ ) command.
		/// </summary>
		/// <returns>The insert command.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="insertValues">Insert values.</param>
		public SqlCommand GetCommandInsert (string tableName, Dictionary<string, object> insertValues)
		{
			var commandParameters = new SQLCommandParameterized (tableName, insertValues, this.DatabaseFields);

			return GetCommandInsert (tableName, commandParameters);
		}

		/// <summary>
		/// Gets the INSERT INTO [table] ( _ ) VALUES ( _ ) command.
		/// </summary>
		/// <returns>The command insert.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="commandParameters">Command parameters.</param>
		public SqlCommand GetCommandInsert (string tableName, SQLCommandParameterized commandParameters)
		{
			SQLFieldValueCollection fieldValues;
			string sql = GetSQLInsert (tableName, commandParameters, out fieldValues);

			var command = new SqlCommand (sql);

			command.AddParameters (fieldValues);

			return command;
		}

		/// <summary>
		/// Gets the UPDATE _ SET _ command.
		/// </summary>
		/// <returns>The insert command.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="insertValues">Insert values.</param>
		/// <param name="conditions">The conditions of the set</param>
		public SqlCommand GetCommandUpdateSet (string tableName, Dictionary<string, object> insertValues, Dictionary<string, object> conditions)
		{
			var namebuilder = new SQLParameterNameBuilder ();
			SQLCommandParameterized commandParametersSet = new SQLCommandParameterized (tableName, insertValues, this.DatabaseFields, namebuilder);
			SQLCommandParameterized commandParametersConditions = new SQLCommandParameterized (tableName, conditions, this.DatabaseFields, namebuilder);

			return GetCommandUpdateSet (tableName, commandParametersSet, commandParametersConditions);
		}

		/// <summary>
		/// Gets the UPDATE _ SET _ command.
		/// </summary>
		/// <returns>The command update set.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="commandParametersSet">Command parameters for the SET clause.</param>
		/// <param name="commandParametersConditions">Command parameters for the WHERE clause.</param>
		public SqlCommand GetCommandUpdateSet (string tableName, SQLCommandParameterized commandParametersSet, SQLCommandParameterized commandParametersConditions)
		{
			// Get the collection Field/Parameter object pairs : SET field names
			var fieldValuesSet = commandParametersSet.GetFieldValuesAssignedOnly ();
			// Get the collection Field/Parameter object pairs : WHERE conditions
			var fieldValuesCondition = commandParametersConditions.GetFieldValuesAssignedOnly ();

			// Assign a SQL statement
			string sql = GetSQLUpdateSet (tableName, fieldValuesSet, fieldValuesCondition);

			var command = new SqlCommand (sql);

			// Add parameters for SET
			command.AddParameters (fieldValuesSet);

			// Add parameters for WHERE
			command.AddParameters (fieldValuesCondition.GetWithoutNullValues ());

			return command;
		}

		/// <summary>
		/// Gets the SELECT command.
		/// </summary>
		/// <returns>The select command.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="isDistinct">If set to <c>true</c> is distinct.</param>
		/// <param name="selectFields">Select fields.</param>
		/// <param name="conditions">Conditions.</param>
		public SqlCommand GetCommandSelect (string tableName, bool isDistinct, IEnumerable<string> selectFields, Dictionary<string, object> conditions)
		{
			return GetCommandSelect (tableName, isDistinct, selectFields, conditions, null);
		}

		/// <summary>
		/// Gets the SELECT command.
		/// </summary>
		/// <returns>The select command.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="isDistinct">If set to <c>true</c> is distinct.</param>
		/// <param name="selectFields">Select fields.</param>
		/// <param name="conditions">Conditions.</param>
		/// <param name="multiValueConditions">Conditions which have multiple values per field.</param>
		public SqlCommand GetCommandSelect (string tableName, bool isDistinct, IEnumerable<string> selectFields, Dictionary<string, object> conditions, Dictionary<string, object[]> multiValueConditions)
		{
			// First, create a parameter object
			var commandParameters = new SQLCommandParameterized (tableName, conditions, multiValueConditions, this.DatabaseFields);

			return GetCommandSelect (tableName, isDistinct, selectFields, commandParameters);
		}

		/// <summary>
		/// Gets the SELECT command.
		/// </summary>
		/// <returns>The command select.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="isDistinct">If set to <c>true</c> is distinct.</param>
		/// <param name="selectFields">Select fields.</param>
		/// <param name="commandParameters">Command parameters.</param>
		public SqlCommand GetCommandSelect (string tableName, bool isDistinct, IEnumerable<string> selectFields, SQLCommandParameterized commandParameters)
		{
			// Get the SQL Statement
			string sql = GetSQLSelect (tableName, isDistinct, selectFields, commandParameters);

			// Create a Command with the SQL object
			var command = new SqlCommand (sql);

			// Add the parameters to the command
			command.AddParameters (commandParameters);

			return command;
		}

		/// <summary>
		/// Gets the SELECT command.
		/// </summary>
		/// <returns>The select command.</returns>
		/// <param name="joinTables">The joined tables.</param>
		/// <param name="selectFieldsAs">Select fields as.</param>
		public SqlCommand GetCommandSelect (string joinTables, Dictionary<string, string> selectFieldsAs)
		{
			return new SqlCommand (GetSQLSelect (joinTables, selectFieldsAs));
		}

		public SqlCommand GetCommandSelect (string joinTables, bool isDistinct, Dictionary<string, string> selectFieldsAs, Dictionary<Dataelus.Database.IDBFieldSimple, object> conditions)
		{
			return GetCommandSelect (conditions, parameters => GetSQLSelect (joinTables, isDistinct, selectFieldsAs, parameters));
		}

		public SqlCommand GetCommandSelect (string joinTables, bool isDistinct, string sqlFieldList, Dictionary<Dataelus.Database.IDBFieldSimple, object> conditions)
		{
			return GetCommandSelect (conditions, parameters => GetSQLSelect (joinTables, isDistinct, sqlFieldList, parameters));
		}

		protected internal SqlCommand GetCommandSelect (Dictionary<Dataelus.Database.IDBFieldSimple, object> conditions, Func<SQLCommandParameterized, string> sqlCreator)
		{
			var nameBuilder = new SQLParameterNameBuilder ();

			var prm = new SQLCommandValueParameters ();
			foreach (var condition in conditions) {
				prm.ValueParams.Add (new SQLParameterValue (this.DatabaseFields.Find (condition.Key), condition.Value));
			}

			var parameters = new SQLCommandParameterized (prm, nameBuilder);

			var sql = sqlCreator (parameters);

			var cmd = new SqlCommand (sql);

			cmd.AddParameters (parameters);

			return cmd;
		}

		public SqlCommand GetCommand (string sqlText, AdHocSqlParameterCollection parameters)
		{
			var typeConverter = new SQLServerDataTypeConverter ();

			var cmd = new SqlCommand (sqlText);
			foreach (var item in parameters) {
				cmd.AddParameter (item, this.DatabaseFields, typeConverter);
			}
			return cmd;
		}

		public SqlCommand GetCommand (string sqlText, IEnumerable<AdHocSqlParameter> parameters)
		{
			return GetCommand (sqlText, new AdHocSqlParameterCollection (parameters));
		}

		public SqlCommand GetCommand (string sqlText, string paramTableName, string paramFieldName, string paramName, object paramValue)
		{
			return GetCommand (sqlText, new AdHocSqlParameter (paramTableName, paramFieldName, paramName, paramValue));
		}

		public SqlCommand GetCommand (string sqlText, params AdHocSqlParameter[] parameters)
		{
			return GetCommand (sqlText, new AdHocSqlParameterCollection (parameters));
		}
	}

	/// <summary>
	/// Stores information for an Ad-hoc SQL parameter.
	/// </summary>
	public class AdHocSqlParameter
	{
		public string TableName { get; set; }

		public string FieldName { get; set; }

		public string ParamName { get; set; }

		public object ParamValue { get; set; }

		public AdHocSqlParameter ()
		{
			
		}

		public AdHocSqlParameter (string tableName, string fieldName, string paramName, object paramValue)
		{
			this.TableName = tableName;
			this.FieldName = fieldName;
			this.ParamName = paramName;
			this.ParamValue = paramValue;
		}
	}

	/// <summary>
	/// Collection of Ad-hoc SQL parameters.
	/// </summary>
	public class AdHocSqlParameterCollection : Dataelus.ListBase<AdHocSqlParameter>
	{
		public AdHocSqlParameterCollection ()
			: base ()
		{
		}

		public AdHocSqlParameterCollection (IEnumerable<AdHocSqlParameter> collection)
			: base (collection)
		{
		}


		public void AddNew (string tableName, string fieldName, string paramName, object paramValue)
		{
			Add (new AdHocSqlParameter (tableName, fieldName, paramName, paramValue));
		}
	}
}

