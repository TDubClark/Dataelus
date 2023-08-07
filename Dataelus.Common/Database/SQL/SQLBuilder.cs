using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Dataelus.Database.SQL
{
	using SQLLanguage;

	public static class SQLBuilderExtensions
	{
		/// <summary>
		/// Gets this database field as a SQL string ([table].[field]).
		/// </summary>
		/// <returns>The SQL field.</returns>
		/// <param name="field">Field.</param>
		public static string GetSQLField (this IDBField field)
		{
			return field.GetSQLField (SQLLanguageImplementation.GenericEnbracketAll);
		}

		/// <summary>
		/// Gets this database field as a SQL string ([table].[field]).
		/// </summary>
		/// <returns>The SQL field.</returns>
		/// <param name="field">Field.</param>
		/// <param name="implementation">Implementation.</param>
		public static string GetSQLField (this IDBField field, SQLLanguageImplementation implementation)
		{
			return field.GetSQLField (false, implementation);
		}

		/// <summary>
		/// Gets this database field as a SQL string ([table].[field]).
		/// </summary>
		/// <returns>The sql field.</returns>
		/// <param name="field">The Field object.</param>
		/// <param name="includeSchema">Whether to include the Schema</param>
		/// <param name="implementation">The SQL language implementation</param>
		public static string GetSQLField (this IDBField field, bool includeSchema, SQLLanguageImplementation implementation)
		{
			const bool removeBrackets = true;
			return field.GetSQLField (includeSchema, removeBrackets, implementation);
		}

		public static string GetSQLField (this IDBField field, bool includeSchema, bool removeBrackets, SQLLanguageImplementation implementation)
		{
			string fieldOut;
			/*fieldOut = String.Format ("[{0}].[{1}].[{2}]", new Object[] {
				field.SchemaName,
				field.TableName,
				field.FieldName
			});*/
			fieldOut = String.Format ("{0}.{1}", new Object[] {
					SQLLanguage.SQLLang.Enbracket (field.TableName, removeBrackets, implementation),
					SQLLanguage.SQLLang.Enbracket (field.FieldName, removeBrackets, implementation)
				});

			if (includeSchema) {
				// Prepend the Schema
				fieldOut = String.Format ("{0}.{1}", new Object[] {
						SQLLanguage.SQLLang.Enbracket (field.SchemaName, removeBrackets, implementation),
						fieldOut
					});
			}

			return fieldOut;
		}

		/// <summary>
		/// Gets the equality statements between a field and a 
		/// </summary>
		/// <returns>The list of SQL equality statements.</returns>
		/// <param name="cmdParams">The command parameter.</param>
		/// <param name="builder">The SQL Builder object</param>
		public static List<string> GetSQLEqualityStatements (this SQLCommandParameterized cmdParams, SQLBuilder builder)
		{
			return builder.GetSQLEqualityStatements (cmdParams);
		}
	}

	/// <summary>
	/// An object for building SQL statements.
	/// </summary>
	public class SQLBuilder
	{
		private SQLLanguageImplementation _sqlImplementation;

		/// <summary>
		/// Gets or sets the SQL implementation.
		/// </summary>
		/// <value>The SQL implementation.</value>
		public SQLLanguageImplementation SQLImplementation {
			get { return _sqlImplementation; }
			set { _sqlImplementation = value; }
		}

		private bool _removeAllBrackets;

		/// <summary>
		/// Gets or sets whether to remove all brackets.
		/// </summary>
		/// <value><c>true</c> if remove all brackets; otherwise, <c>false</c>.</value>
		public bool RemoveAllBrackets {
			get { return _removeAllBrackets; }
			set { _removeAllBrackets = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLBuilder"/> class.
		/// </summary>
		public SQLBuilder ()
			: this (SQLLanguageImplementation.GenericEnbracketAll, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLBuilder"/> class.
		/// </summary>
		/// <param name="sqlImplementation">SQL language implementation.</param>
		public SQLBuilder (SQLLanguageImplementation sqlImplementation)
			: this (sqlImplementation, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLBuilder"/> class.
		/// </summary>
		/// <param name="sqlImplementation">SQL language implementation.</param>
		/// <param name="removeAllBrackets">If set to <c>true</c> remove all brackets from SQL fields.</param>
		public SQLBuilder (SQLLanguageImplementation sqlImplementation, bool removeAllBrackets)
		{
			_sqlImplementation = sqlImplementation;
			_removeAllBrackets = removeAllBrackets;
		}


		/// <summary>
		/// Gets the SQL field.
		/// </summary>
		/// <returns>The SQL field.</returns>
		/// <param name="field">Field.</param>
		/// <param name="includeSchema">If set to <c>true</c> include schema.</param>
		public virtual string GetSQLField (IDBField field, bool includeSchema)
		{
			return field.GetSQLField (includeSchema, _removeAllBrackets, _sqlImplementation);
		}

		/// <summary>
		/// Gets the equality statements between a field and a 
		/// </summary>
		/// <returns>The list of SQL equality statements.</returns>
		/// <param name="cmdParams">The command parameter.</param>
		public virtual List<string> GetSQLEqualityStatements (SQLCommandParameterized cmdParams)
		{
			// Build the WHERE-clause parameters
			var whereFieldValues = cmdParams.GetFieldValuesAssignedOnly ();

			return GetSQLEqualityStatements (whereFieldValues);
		}

		/// <summary>
		/// Gets the SQL equality statements.
		/// </summary>
		/// <returns>The SQL equality statements.</returns>
		/// <param name="whereFieldValues">Where field values.</param>
		public virtual List<string> GetSQLEqualityStatements (SQLFieldValueCollection whereFieldValues)
		{
			List<string> whereValueList = whereFieldValues.Select (x => GetSQLFieldEqualsWhere (x.Field, x.Parameter.ParameterName, x.Parameter.Value)).ToList ();
			return whereValueList;
		}

		/// <summary>
		/// Enbracket the specified field, if needed.
		/// </summary>
		/// <param name="field">Field.</param>
		public virtual string Enbracket (string field)
		{
			return SQLLang.Enbracket (field, _removeAllBrackets, _sqlImplementation);
		}

		public virtual List<string> Enbracket (IEnumerable<string> fields)
		{
			return SQLLang.Enbracket (fields, _sqlImplementation);
		}

		/// <summary>
		/// Builds the select statement for a single table.
		/// </summary>
		/// <returns>The select single table.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldNames">The list of table field names.</param>
		public virtual string BuildSelectSingleTable (string tableName, IEnumerable<string> fieldNames)
		{
			return BuildSelectSingleTable (tableName, fieldNames, false);
		}

		public virtual string BuildSelectSingleTable (string tableName, IEnumerable<string> fieldNames, bool isDistinct)
		{
			string strDistinct = isDistinct ? " DISTINCT" : "";
			return String.Format ("SELECT{2} {0} FROM {1}", GetSQLFieldList (fieldNames), Enbracket (tableName), strDistinct);
		}

		public virtual string GetSQLFieldList (IEnumerable<string> fieldNames)
		{
			if (fieldNames == null || !fieldNames.Any ())
				return "*";
			return String.Join (", ", Enbracket (fieldNames));
		}

		/// <summary>
		/// Builds the select statement for a single table.
		/// </summary>
		/// <returns>The select single table.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldNamesAs">The dictionary of table field names, where key=field name, value="select as" name.</param>
		public virtual string BuildSelectSingleTable (string tableName, Dictionary<string, string> fieldNamesAs)
		{
			var sqlFieldList = GetSQLFieldAsList (fieldNamesAs);
			return String.Format ("SELECT {0} FROM {1}", sqlFieldList, Enbracket (tableName));
		}

		public virtual string GetSQLFieldAsList (Dictionary<string, string> fieldNamesAs)
		{
			if (fieldNamesAs == null || fieldNamesAs.Count == 0)
				return "*";
			
			var fieldsAs = GetSelectFieldsAs (fieldNamesAs);
			var sqlFieldList = String.Join (", ", fieldsAs);

			return sqlFieldList;
		}

		/// <summary>
		/// Gets the list of SELECT fields (with optional "Select As" names) properly formatted.
		/// </summary>
		/// <returns>The select fields as.</returns>
		/// <param name="fieldNamesAs">Field names as.</param>
		public virtual string[] GetSelectFieldsAs (Dictionary<string, string> fieldNamesAs)
		{
			return fieldNamesAs.Select (x => GetSQLFieldAs (x.Key, x.Value)).ToArray ();
		}

		/// <summary>
		/// Builds the select statement.
		/// </summary>
		/// <returns>The select.</returns>
		/// <param name="joinTable">Join table.</param>
		/// <param name="fields">Fields.</param>
		public virtual string BuildSelect (string joinTable, bool distinct, IEnumerable<IDBField> fields)
		{
			return String.Format ("SELECT{2} {1} FROM {0}", joinTable, GetSQLFieldList (fields, true, false), distinct ? " DISTINCT" : "");
		}


		/// <summary>
		/// Gets the SQL field list, each field name separated by a comma.
		/// </summary>
		/// <returns>The SQL field list.</returns>
		/// <param name="selectFields">Select fields.</param>
		/// <param name="includeTableName">If set to <c>true</c> include table name.</param>
		/// <param name="includeSchema">If set to <c>true</c> include schema.</param>
		public virtual string GetSQLFieldList (IEnumerable<IDBField> selectFields, bool includeTableName, bool includeSchema)
		{
			return GetSQLFieldList (GetDefaultFieldAs (selectFields), includeTableName, includeSchema);
		}

		/// <summary>
		/// Gets the default Field AS dictionary (assume no AS).
		/// </summary>
		/// <returns>The default field as.</returns>
		/// <param name="selectFields">Select fields.</param>
		public virtual Dictionary<IDBField, string> GetDefaultFieldAs (IEnumerable<IDBField> selectFields)
		{
			var dict = new Dictionary<IDBField, string> ();

			foreach (var item in selectFields) {
				dict.Add (item, null);
			}

			return dict;
		}

		/// <summary>
		/// Gets the SQL field equals statement.
		/// </summary>
		/// <returns>The SQL field equals.</returns>
		/// <param name="field">Field.</param>
		/// <param name="parameterName">Parameter name.</param>
		public virtual string GetSQLFieldEquals (IDBField field, string parameterName)
		{
			//return String.Format ("{0} = {1}", Enbracket (field.FieldName), parameterName);
			return String.Format ("{0} = {1}", field.GetSQLField (false, SQLLanguageImplementation.MSSQLServer), parameterName);
		}

		/// <summary>
		/// Gets the SQL field equals statement.
		/// </summary>
		/// <returns>The SQL field equals.</returns>
		/// <param name="field">Field.</param>
		/// <param name = "parameter">The Parameter/value object</param>
		public virtual string GetSQLFieldEqualsWhere (IDBField field, ParamValue parameter)
		{
			if (field == null)
				throw new ArgumentNullException ("field");
			if (parameter == null)
				throw new ArgumentNullException ("parameter");
			return GetSQLFieldEqualsWhere (field, parameter.ParameterName, parameter.Value);
		}

		/// <summary>
		/// Gets the SQL field equals statement.
		/// </summary>
		/// <returns>The SQL field equals.</returns>
		/// <param name="field">Field.</param>
		/// <param name="parameterName">Parameter name.</param>
		/// <param name = "parameterValue">The value of the Parameter</param>
		public virtual string GetSQLFieldEqualsWhere (IDBField field, string parameterName, object parameterValue)
		{
			if (field == null)
				throw new ArgumentNullException ("field");
			if (parameterName == null)
				throw new ArgumentNullException ("parameterName");
			if (String.IsNullOrWhiteSpace (parameterName))
				throw new ArgumentNullException ("parameterName", "Parameter is null or only white-space characters; must be a valid parameter name.");
			
			string fieldText = field.GetSQLField (false, SQLLanguageImplementation.MSSQLServer);

			if (parameterValue == null)
				return String.Format ("{0} IS NULL", fieldText);
			
			//return String.Format ("{0} = {1}", Enbracket (field.FieldName), parameterName);
			return String.Format ("{0} = {1}", fieldText, parameterName);
		}

		/// <summary>
		/// Gets the SQL field list.
		/// </summary>
		/// <returns>The SQL field list.</returns>
		/// <param name="selectFieldsAs">Select fields as.</param>
		/// <param name="includeTableName">If set to <c>true</c> include table name.</param>
		/// <param name="includeSchema">If set to <c>true</c> include schema.</param>
		public virtual string GetSQLFieldList (Dictionary<IDBField, string> selectFieldsAs, bool includeTableName, bool includeSchema)
		{
			SQLFieldBuilder fieldBuilder = new SQLFieldBuilder (GetSQLField);
			return GetSQLFieldList (selectFieldsAs, includeTableName, includeSchema, fieldBuilder);
		}

		/// <summary>
		/// Gets the SQL field list.
		/// </summary>
		/// <returns>The SQL field list.</returns>
		/// <param name="selectFieldsAs">Select fields as.</param>
		/// <param name="includeTableName">If set to <c>true</c> include table name.</param>
		/// <param name="includeSchema">If set to <c>true</c> include schema.</param>
		/// <param name="fieldBuilder">Field builder.</param>
		public virtual string GetSQLFieldList (Dictionary<IDBField, string> selectFieldsAs, bool includeTableName, bool includeSchema, SQLFieldBuilder fieldBuilder)
		{
			includeTableName |= includeSchema;

			if (includeTableName) {
				var lst = selectFieldsAs.Select (x => GetSQLFieldAs (x.Key, x.Value, fieldBuilder, includeSchema)).ToList ();

				return String.Join (", ", lst);
			} else {
				var lst = selectFieldsAs.Select (x => GetSQLFieldAs (x.Key.FieldName, x.Value)).ToList ();

				return String.Join (", ", lst);
			}
		}

		/// <summary>
		/// Gets the SQL field as a return name.
		/// </summary>
		/// <returns>The SQL field as.</returns>
		/// <param name="field">Field.</param>
		/// <param name="fieldAs">Field as.</param>
		/// <param name="fieldBuilder">Field builder.</param>
		/// <param name="includeSchema">If set to <c>true</c> include schema.</param>
		public virtual string GetSQLFieldAs (IDBField field, string fieldAs, SQLFieldBuilder fieldBuilder, bool includeSchema)
		{
			if (String.IsNullOrWhiteSpace (fieldAs)) {
				return fieldBuilder (field, includeSchema);
			}
			return String.Format ("{0} AS {1}", fieldBuilder (field, includeSchema), Enbracket (fieldAs));
		}

		/// <summary>
		/// Gets the SQL field as a return name.
		/// </summary>
		/// <returns>The SQL field as.</returns>
		/// <param name="field">Field name.</param>
		/// <param name="fieldAs">The "Select As" name.</param>
		public virtual string GetSQLFieldAs (string field, string fieldAs)
		{
			if (String.IsNullOrWhiteSpace (fieldAs)) {
				return Enbracket (field);
			}
			return String.Format ("{0} AS {1}", Enbracket (field), Enbracket (fieldAs));
		}



		public virtual string GetSQLInsert (string tableName, SQLCommandParameterized cmdParameterized, out SQLFieldValueCollection fieldValues)
		{
			fieldValues = cmdParameterized.GetFieldValuesAssignedOnly ();

			return GetSQLInsert (tableName, fieldValues);
		}

		public virtual string GetSQLInsert (string tableName, SQLFieldValueCollection fieldValues)
		{
			string tableName2 = Enbracket (tableName);
			string fieldList = GetSQLFieldList (fieldValues.Select (x => x.Field.FieldName).ToList ());
			string paramList = String.Join (", ", fieldValues.Select (x => x.Parameter.ParameterName).ToList ());

			string sql = String.Format ("INSERT INTO {0} ({1}) VALUES ({2})", tableName2, fieldList, paramList);
			return sql;
		}

		public virtual string GetSQLUpdateSet (string tableName, SQLFieldValueCollection fieldValuesSet, SQLFieldValueCollection fieldValuesWhere)
		{
			string tableName2 = Enbracket (tableName);

			List<string> setValueList = fieldValuesSet.Select (x => GetSQLFieldEquals (x.Field, x.Parameter.ParameterName)).ToList ();
			List<string> whereValueList = fieldValuesWhere.Select (x => GetSQLFieldEqualsWhere (x.Field, x.Parameter)).ToList ();

			string setList = String.Join (", ", setValueList);
			string whereList = String.Join (" AND ", whereValueList);

			string sql = String.Format ("UPDATE {0} SET {1} WHERE {2}", tableName2, setList, whereList);
			return sql;
		}

		public virtual string GetSQLSelect (string joinTables, Dictionary<string, string> selectFieldsAs)
		{
			string sql = String.Format ("SELECT {0} FROM {1}", GetSQLFieldAsList (selectFieldsAs), joinTables);
			return sql;
		}

		public virtual string GetSQLSelect (string joinTables, bool isDistinct, Dictionary<string, string> selectFieldsAs, SQLCommandParameterized cmdparam)
		{
			return GetSQLSelect (joinTables, isDistinct, GetSQLFieldAsList (selectFieldsAs), cmdparam);
		}

		public virtual string GetSQLSelect (string joinTables, bool isDistinct, IEnumerable<string> selectFields, SQLCommandParameterized cmdparam)
		{
			return GetSQLSelect (joinTables, isDistinct, GetSQLFieldList (selectFields), cmdparam);
		}

		/// <summary>
		/// Gets the SQL select statement.
		/// </summary>
		/// <returns>The SQL select.</returns>
		/// <param name="joinTables">Join tables.</param>
		/// <param name="isDistinct">If set to <c>true</c> is distinct.</param>
		/// <param name="sqlFieldList">The SQL field list, in string format, ready to be put into the SQL statement.</param>
		/// <param name="cmdparam">Cmdparam.</param>
		public virtual string GetSQLSelect (string joinTables, bool isDistinct, string sqlFieldList, SQLCommandParameterized cmdparam)
		{
			string strDistinct = isDistinct ? " DISTINCT" : "";
			string sqlSelectFrom = String.Format ("SELECT{2} {0} FROM {1}", sqlFieldList, joinTables, strDistinct);

			string sql = String.Format ("{0} WHERE {1}", sqlSelectFrom, cmdparam.GetSQLWhereClause ());
			return sql;
		}

//		public virtual string GetSQLSelect (string tableName, bool isDistinct, IEnumerable<string> selectFields, SQLCommandParameterized cmdparam)
//		{
//			string sqlSelectFrom = BuildSelectSingleTable (tableName, selectFields, isDistinct);
//
//			string sql = String.Format ("{0} WHERE {1}", sqlSelectFrom, cmdparam.GetSQLWhereClause ());
//			return sql;
//		}

		public virtual string GetSQLDeleteSingleTable (string tableName, SQLCommandParameterized cmdParamCondition, out SQLFieldValueCollection whereFieldValues)
		{
			whereFieldValues = cmdParamCondition.GetFieldValuesAssignedOnly ();

			return GetSQLDeleteSingleTable (tableName, whereFieldValues);
		}

		public virtual string GetSQLDeleteSingleTable (string tableName, SQLFieldValueCollection whereFieldValues)
		{
			string whereList = String.Join (" AND ", GetSQLEqualityStatements (whereFieldValues));

			var tableName2 = Enbracket (tableName);

			string sql = String.Format ("DELETE FROM {0} WHERE {1}", tableName2, whereList);
			return sql;
		}
	}
}

