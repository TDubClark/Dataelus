using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

using Dataelus.Database;
using Dataelus.Mono.Extensions;
using Dataelus.Database.SQLLanguage;

namespace Dataelus.Mono
{
	/// <summary>
	/// Database Querier class.
	/// </summary>
	public class DBQuerier : IDBQuerier2
	{
		/// <summary>
		/// The connection string.
		/// </summary>
		protected string _connectionString;

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
		public string ConnectionString {
			get { return _connectionString; }
			set { _connectionString = value; }
		}

		/// <summary>
		/// The connection timeout.
		/// </summary>
		protected int _connectionTimeout;

		/// <summary>
		/// Gets or sets the connection timeout.
		/// </summary>
		/// <value>The connection timeout.</value>
		public int ConnectionTimeout {
			get { return _connectionTimeout; }
			set { _connectionTimeout = value; }
		}

		/// <summary>
		/// The command timeout.
		/// </summary>
		protected int _commandTimeout;

		/// <summary>
		/// Gets or sets the default command timeout.
		/// </summary>
		/// <value>The command timeout.</value>
		public int CommandTimeout {
			get { return _commandTimeout; }
			set { _commandTimeout = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DBQuerier"/> class.
		/// </summary>
		public DBQuerier ()
		{
			_connectionTimeout = -1;
			_commandTimeout = -1;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DBQuerier"/> class.
		/// </summary>
		/// <param name="connectionString">Connection string.</param>
		public DBQuerier (string connectionString)
			: this ()
		{
			_connectionString = connectionString;
		}

		/// <summary>
		/// Gets the DataSet for the given SQL Command.
		/// </summary>
		/// <returns>The DataSet.</returns>
		/// <param name="sqlCommand">Sql command.</param>
		public System.Data.DataSet GetDs (string sqlCommand)
		{
			return GetDs (sqlCommand, _connectionString);
		}

		/// <summary>
		/// Gets the DataSet for the given SQL Command and Connection String
		/// </summary>
		/// <returns>The DataSet.</returns>
		/// <param name="sqlCommand">Sql command.</param>
		/// <param name="connectionString">Connection string.</param>
		public System.Data.DataSet GetDs (string sqlCommand, string connectionString)
		{
			return GetDs (sqlCommand, _commandTimeout, connectionString, _connectionTimeout);
		}

		/// <summary>
		/// Gets the DataSet for the given SQL Command and Connection String
		/// </summary>
		/// <returns>The DataSet.</returns>
		/// <param name="sqlCommand">Sql command.</param>
		/// <param name="commandTimeout">Command timeout.</param>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="connectionTimeout">Connection timeout.</param>
		public System.Data.DataSet GetDs (string sqlCommand, int commandTimeout, string connectionString, int connectionTimeout)
		{
			var ds = new System.Data.DataSet ();

			if (connectionTimeout > 0) {
				var builder = new SqlConnectionStringBuilder (connectionString);
				builder.ConnectTimeout = connectionTimeout;
				connectionString = builder.ToString ();
			}

			using (var conn = new System.Data.SqlClient.SqlConnection (connectionString)) {
				conn.Open ();

				using (var cmd = new System.Data.SqlClient.SqlCommand (sqlCommand, conn)) {
					if (commandTimeout > 0)
						cmd.CommandTimeout = commandTimeout;

					cmd.CommandType = CommandType.Text;

					var da = new System.Data.SqlClient.SqlDataAdapter (cmd);
					da.Fill (ds);
				}

				conn.Close ();
			}
			return ds;
		}

		public System.Data.DataSet GetDs (SqlCommand command)
		{
			return GetDs (command, _connectionString);
		}

		public System.Data.DataSet GetDs (SqlCommand command, string connectionString)
		{
			var ds = new DataSet ();

			using (var conn = new System.Data.SqlClient.SqlConnection (connectionString)) {
				conn.Open ();

				command.Connection = conn;
				var da = new System.Data.SqlClient.SqlDataAdapter (command);
				da.Fill (ds);

				conn.Close ();
			}
			return ds;
		}

		#region IDBQuerier implementation

		/// <summary>
		/// Gets the record count.
		/// </summary>
		/// <returns>The record count.</returns>
		/// <param name="field">Field.</param>
		public int GetRecordCount (IDBFieldSimple field)
		{
			return GetRecordCount (field.TableName, field.FieldName);
		}

		/// <summary>
		/// Gets the record count.
		/// </summary>
		/// <returns>The record count.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		public int GetRecordCount (string tableName, string fieldName)
		{
			string sql = String.Format ("SELECT COUNT(DISTINCT) FROM {1}", Database.SQLLanguage.SQLLang.Enbracket (fieldName), Database.SQLLanguage.SQLLang.Enbracket (tableName));

			return GetCount (sql);
		}

		#endregion

		/// <summary>
		/// Gets the number of records returned by the given SQL statement.
		/// </summary>
		/// <returns>The record count.</returns>
		/// <param name="sql">Sql.</param>
		public int GetRecordCount (string sql)
		{
			return GetDs (sql).Tables [0].Rows.Count;
		}

		/// <summary>
		/// Gets the number of records returned by the given SQL command.
		/// </summary>
		/// <returns>The record count.</returns>
		/// <param name="selectCommand">Select command.</param>
		public int GetRecordCount (SqlCommand selectCommand)
		{
			return GetDs (selectCommand).Tables [0].Rows.Count;
		}

		/// <summary>
		/// Gets the integer value of the first row/first column returned by the given SELECT COUNT... sql statement.
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="sqlCount">Sql count query.</param>
		public int GetCount (string sqlCount)
		{
			return GetCountValue (GetDs (sqlCount));
		}

		/// <summary>
		/// Gets the integer value of the first row/first column returned by the given SELECT COUNT... sql command.
		/// </summary>
		/// <returns>The count.</returns>
		/// <param name="selectCountCommand">Select count command.</param>
		public int GetCount (SqlCommand selectCountCommand)
		{
			return GetCountValue (GetDs (selectCountCommand));
		}

		static int GetCountValue (DataSet ds)
		{
			return (int)ds.Tables [0].Rows [0] [0];
		}


		/// <summary>
		/// Gets the unique values for each field (uses the same connection for better performance).
		/// </summary>
		/// <returns>The unique values as a dictionary of {DB field : unique values}.</returns>
		/// <param name="fields">The database fields.</param>
		public Dictionary<Dataelus.Database.IDBFieldSimple, List<string>> GetUniqueValues (IEnumerable<Dataelus.Database.IDBFieldSimple> fields)
		{
			return GetUniqueValues<string> (fields, x => x.GetValue (0).ToString ());
		}

		/// <summary>
		/// Gets the unique values for each field (uses the same connection for better performance).
		/// </summary>
		/// <returns>The unique values as a dictionary of {DB field : unique values}.</returns>
		/// <param name="fields">The database fields.</param>
		/// <param name="objectCreator">Function for creating an object of type T from a given SqlDataReader. (called each time a new record is read into the SqlDataReader)</param>
		public Dictionary<Dataelus.Database.IDBFieldSimple, List<T>> GetUniqueValues <T> (IEnumerable<Dataelus.Database.IDBFieldSimple> fields, Func<SqlDataReader, T> objectCreator)
		{
			var dict = new Dictionary<IDBFieldSimple, List<T>> (new Dataelus.Database.IDBFieldSimpleComparer ());
			using (var conn = new SqlConnection (_connectionString)) {
				conn.Open ();

				foreach (var field in fields) {
					string sqlCommand = GetSqlUniqueValues (field.SchemaName, field.TableName, field.FieldName);
					var lst = GetObjects<T> (sqlCommand, conn, objectCreator);
					if (!dict.ContainsKey (field))
						dict.Add (field, lst);
				}

				conn.Close ();
			}

			return dict;
		}

		/// <summary>
		/// Gets the unique values for the given Database Field.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="field">Field.</param>
		public List<string> GetUniqueValues (Dataelus.Database.IDBFieldSimple field)
		{
			return GetUniqueValues (field.SchemaName, field.TableName, field.FieldName);
		}

		/// <summary>
		/// Gets the unique values for the given Database Field.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="field">Field.</param>
		public List<object> GetUniqueValueObjects (Dataelus.Database.IDBFieldSimple field)
		{
			return GetUniqueValueObjects (field.SchemaName, field.TableName, field.FieldName);
		}

		/// <summary>
		/// Gets the unique values.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		public List<string> GetUniqueValues (string tableName, string fieldname)
		{
			return GetUniqueValues (null, tableName, fieldname);
		}

		/// <summary>
		/// Gets the SQL for Unique Values.
		/// </summary>
		/// <returns>The sql.</returns>
		/// <param name="schemaName">Schema name.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		protected virtual string GetSqlUniqueValues (string schemaName, string tableName, string fieldname)
		{
			string table = SQLLang.Enbracket (tableName);
			if (!String.IsNullOrWhiteSpace (schemaName))
				table = String.Format ("{0}.{1}", SQLLang.Enbracket (schemaName), table);
			string sql = String.Format ("SELECT DISTINCT {0} FROM {1}", SQLLang.Enbracket (fieldname), table);
			return sql;
		}

		/// <summary>
		/// Gets the SQL for Unique Values.
		/// </summary>
		/// <returns>The sql.</returns>
		/// <param name="schemaName">Schema name.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		protected virtual string GetSqlUniqueValuesCount (string schemaName, string tableName, string fieldname)
		{
			string table = SQLLang.Enbracket (tableName);
			if (!String.IsNullOrWhiteSpace (schemaName))
				table = String.Format ("{0}.{1}", SQLLang.Enbracket (schemaName), table);
			string sql = String.Format ("SELECT COUNT(DISTINCT {0}) FROM {1}", SQLLang.Enbracket (fieldname), table);
			return sql;
		}

		/// <summary>
		/// Gets the unique values for the given field.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="schemaName">Schema name.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		public List<string> GetUniqueValues (string schemaName, string tableName, string fieldname)
		{
			var sql = GetSqlUniqueValues (schemaName, tableName, fieldname);
			return GetObjects<string> (sql, x => GetString (x, 0));
		}

		public string GetString (SqlDataReader reader, int index)
		{
			if (reader.IsDBNull (index))
				return null;
			Type dataType = reader.GetFieldType (index);
			if (dataType.Equals (typeof(System.String))) {
				return reader.GetString (index);
			} else {
				return reader.GetValue (index).ToString ();
			}
		}

		/// <summary>
		/// Gets the unique values for the given field.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="schemaName">Schema name.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		public List<object> GetUniqueValueObjects (string schemaName, string tableName, string fieldname)
		{
			var sql = GetSqlUniqueValues (schemaName, tableName, fieldname);
			return GetObjects<object> (sql, x => x.GetValue (0));
		}

		/// <summary>
		/// Gets the count of unique values for the given field.
		/// </summary>
		/// <returns>The unique values count.</returns>
		/// <param name="field">An object which defines a database Field.</param>
		public int GetUniqueValuesCount (Dataelus.Database.IDBFieldSimple field)
		{
			return GetUniqueValuesCount (field.SchemaName, field.TableName, field.FieldName);
		}

		/// <summary>
		/// Gets the count of unique values for the given field.
		/// </summary>
		/// <returns>The unique values count.</returns>
		/// <param name="schemaName">Schema name.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		public int GetUniqueValuesCount (string schemaName, string tableName, string fieldname)
		{
			var sql = GetSqlUniqueValuesCount (schemaName, tableName, fieldname);
			return GetObjects<int> (sql, x => x.GetInt32 (0)) [0];
		}

		/// <summary>
		/// Gets the table at index zero for the given DataSet.
		/// </summary>
		/// <returns>The table zero.</returns>
		/// <param name="ds">The DataSet from which to find the table at index zero.</param>
		public static System.Data.DataTable GetTableZero (System.Data.DataSet ds)
		{
			if (ds.Tables.Count > 0)
				return ds.Tables [0];
			return null;
		}

		/// <summary>
		/// Gets the objects from the given command string.
		/// Read in via a SqlDataReader; function used to create each object.
		/// </summary>
		/// <returns>The objects.</returns>
		/// <param name="sqlCommand">Sql command.</param>
		/// <param name="objectCreator">A function which creates a single object of type T from a SqlDataReader (called after each record is read).</param>
		/// <typeparam name="T">The type of object to retrieve from each record.</typeparam>
		public List<T> GetObjects<T> (string sqlCommand, Func<SqlDataReader,T> objectCreator)
		{
			return GetObjects<T> (sqlCommand, _connectionString, objectCreator);
		}

		/// <summary>
		/// Gets the objects from the given command string.
		/// Read in via a SqlDataReader; function used to create each object.
		/// </summary>
		/// <returns>The objects.</returns>
		/// <param name="sqlCommand">Sql command.</param>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="objectCreator">A function which creates a single object of type T from a SqlDataReader (called after each record is read).</param>
		/// <typeparam name="T">The type of object to retrieve from each record.</typeparam>
		public static List<T> GetObjects<T> (string sqlCommand, string connectionString, Func<SqlDataReader,T> objectCreator)
		{
			List<T> lst = null;
			using (var conn = new SqlConnection (connectionString)) {
				conn.Open ();

				lst = GetObjects (sqlCommand, conn, objectCreator);

				conn.Close ();
			}

			return lst;
		}

		static List<T> GetObjectsDistinct<T> (string sqlCommand, SqlConnection conn, Func<SqlDataReader, T> objectCreator, IEqualityComparer<T> comparer)
		{
			return GetObjects<T> (sqlCommand, conn, objectCreator).Distinct (comparer).ToList ();
		}

		static List<T> GetObjects<T> (string sqlCommand, SqlConnection conn, Func<SqlDataReader, T> objectCreator)
		{
			var lst = new List<T> ();

			using (var cmd = new SqlCommand (sqlCommand, conn)) {
				var dreader = cmd.ExecuteReader ();
				while (dreader.Read ()) {
					lst.Add (objectCreator (dreader));
				}
				dreader.Close ();
			}
			return lst;
		}

		public void GetObjects (string sqlCommand, Action<SqlDataReader> readerAction)
		{
			GetObjects (sqlCommand, _connectionString, readerAction);
		}

		/// <summary>
		/// Gets the objects from the given command string.
		/// Read in via a SqlDataReader; function used to create each object.
		/// </summary>
		/// <returns>The objects.</returns>
		/// <param name="sqlCommand">Sql command.</param>
		/// <param name="connectionString">Connection string.</param>
		/// <param name="readerAction">A subroutine (or Action) called on a SqlDataReader (called after each record is read).</param>
		/// <typeparam name="T">The type of object to retrieve from each record.</typeparam>
		public static void GetObjects (string sqlCommand, string connectionString, Action<SqlDataReader> readerAction)
		{
			using (var conn = new SqlConnection (connectionString)) {
				using (var cmd = new SqlCommand (sqlCommand, conn)) {
					conn.Open ();

					var dreader = cmd.ExecuteReader ();

					while (dreader.Read ()) {
						readerAction (dreader);
					}

					dreader.Close ();

					conn.Close ();
				}
			}
		}

		/// <summary>
		/// Gets the nullable object value (if DBNull, returns null).
		/// </summary>
		/// <returns>The nullable.</returns>
		/// <param name="value">Value.</param>
		public static object GetNullable (object value)
		{
			return DBNull.Value.Equals (value) ? null : value;
		}

		/// <summary>
		/// Gets the schema identity columns for schema 'dbo'.
		/// </summary>
		/// <returns>The schema identity columns.</returns>
		public DataSet GetSchemaIdentityColumns ()
		{
			string dbSchemaName = "dbo";
			return GetSchemaIdentityColumns (dbSchemaName);
		}

		public DataSet GetSchemaIdentityColumns (string dbSchemaName)
		{
			// Only allow alpha-numerics and underscore
			dbSchemaName = System.Text.RegularExpressions.Regex.Replace (dbSchemaName, "[^a-zA-Z0-9_]+", "");

			// Query Source: http://stackoverflow.com/questions/87747/how-do-you-determine-what-sql-tables-have-an-identity-column-programatically

			string sql = "SELECT TABLE_SCHEMA, TABLE_NAME, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" + dbSchemaName + "'";
			sql += " AND COLUMNPROPERTY(object_id(TABLE_NAME), COLUMN_NAME, 'IsIdentity') = 1";
			sql += " ORDER BY TABLE_NAME, COLUMN_NAME";

			return GetDs (sql);
		}

		/// <summary>
		/// Gets the schema identity column list.
		/// </summary>
		/// <returns>The schema identity column list.</returns>
		/// <param name="dbSchemaName">Db schema name.</param>
		public List<DBFieldSimple> GetSchemaIdentityColumnList (string dbSchemaName)
		{
			var ds = GetSchemaIdentityColumns (dbSchemaName);

			return GetSchemaIdentityColumnList (ds);
		}

		/// <summary>
		/// Gets the schema identity column list, according to the DataSet created from GetSchemaIdentityColumns().
		/// </summary>
		/// <returns>The schema identity column list.</returns>
		/// <param name="dsIdentityColumns">DataSet of identity columns.</param>
		public List<DBFieldSimple> GetSchemaIdentityColumnList (DataSet dsIdentityColumns)
		{
			var collection = new List<DBFieldSimple> ();

			for (int r = 0; r < dsIdentityColumns.Tables [0].Rows.Count; r++) {
				var dr = dsIdentityColumns.Tables [0].Rows [r];
				var dbField = new DBFieldSimple (dr ["TABLE_SCHEMA"].ToNullableString (), dr ["TABLE_NAME"].ToNullableString (), dr ["COLUMN_NAME"].ToNullableString ());
				collection.Add (dbField);
			}

			return collection;
		}

		/// <summary>
		/// Gets the schema table.
		/// </summary>
		/// <returns>The schema table.</returns>
		/// <param name="tableName">Table name.</param>
		public DataTable GetSchemaTable (string tableName)
		{
			return GetSchemaTable (tableName, _connectionString);
		}

		/// <summary>
		/// Gets the schema table for Columns.
		/// </summary>
		/// <returns>The schema table columns.</returns>
		public DataTable GetSchemaTableColumns ()
		{
			return GetSchemaTable ("Columns");
		}

		/// <summary>
		/// Gets the schema table.
		/// </summary>
		/// <returns>The schema table.</returns>
		/// <param name="schemaType">Schema type.</param>
		public DataTable GetSchemaTable (SQLServer.DBSchemaType schemaType)
		{
			return GetSchemaTable (GetSchemaName (schemaType));
		}

		/// <summary>
		/// Gets the name of the schema.
		/// </summary>
		/// <returns>The schema name.</returns>
		/// <param name="schemaType">Schema type.</param>
		protected string GetSchemaName (SQLServer.DBSchemaType schemaType)
		{
			switch (schemaType) {
				case SQLServer.DBSchemaType.Columns:
					return "Columns";
				case SQLServer.DBSchemaType.Foreignkeys:
					return "Foreignkeys";
				case SQLServer.DBSchemaType.IndexColumns:
					return "IndexColumns";
				case SQLServer.DBSchemaType.StructuredTypeMembers:
					return "StructuredTypeMembers";
				case SQLServer.DBSchemaType.Tables:
					return "Tables";
				default:
					throw new ArgumentException (String.Format ("Invalid Schema Type: '{0}'", schemaType), "schemaType");
			}
		}

		/// <summary>
		/// Gets the schema table.
		/// </summary>
		/// <returns>The schema table.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="connectionString">Connection string.</param>
		public static DataTable GetSchemaTable (string tableName, string connectionString)
		{
			var comparer = new StringEqualityComparer ();

			if (!comparer.EqualsAny (tableName, "Columns", "Tables"))
				throw new ArgumentException (String.Format ("Schema table '{0}' not supported.", tableName), "tableName");

			DataTable dt = null;
			using (var conn = new SqlConnection (connectionString)) {
				conn.Open ();

				string sql = String.Format ("SELECT * FROM INFORMATION_SCHEMA.{0}", tableName);
				var cmd = conn.CreateCommand ();
				cmd.CommandText = sql;

				var da = new SqlDataAdapter ();
				da.SelectCommand = cmd;

				dt = new DataTable (tableName);
				da.Fill (dt);

//				dt = conn.GetSchema (tableName);

				conn.Close ();
			}
			return dt;
		}
	}
}

