using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Mono.Extensions;
using Dataelus.Extensions;

namespace Dataelus.Mono
{
	/// <summary>
	/// Data services which use the Mono/.NET Framework.
	/// </summary>
	public class DataServices : IDataServices2
	{
		/// <summary>
		/// The querier.
		/// </summary>
		protected DBQuerier _querier;

		/// <summary>
		/// Gets or sets the database querier.
		/// </summary>
		/// <value>The querier.</value>
		public DBQuerier Querier {
			get { return _querier; }
			set { _querier = value; }
		}

		protected IFieldValueQuerier _fieldValueQuerier;

		/// <summary>
		/// Gets or sets the field value querier.
		/// </summary>
		/// <value>The field value querier.</value>
		public IFieldValueQuerier FieldValueQuerier {
			get { return _fieldValueQuerier; }
			set { _fieldValueQuerier = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataServices"/> class.
		/// </summary>
		public DataServices ()
		{
			
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Mono.DataServices"/> class.
		/// </summary>
		/// <param name="querier">Querier.</param>
		public DataServices (DBQuerier querier)
		{
			_querier = querier;
			_fieldValueQuerier = new FieldValueQuerierDefault (querier);
		}

		public DataServices (DBQuerier querier, IFieldValueQuerier fieldValueQuerier)
		{
			_querier = querier;
			_fieldValueQuerier = fieldValueQuerier;
		}

		#region IDataServices implementation

		/// <summary>
		/// Gets the text table.
		/// </summary>
		/// <returns>The text table.</returns>
		/// <param name="sql">Sql.</param>
		public Dataelus.Table.TextTable GetTextTable (string sql)
		{
			return GetTextTable (_querier.GetDs (sql));
		}

		///<summary>
		///Gets the object table.
		///</summary>
		///<returns>The object table.</returns>
		///<param name="sql">Sql.</param>
		public Dataelus.Table.ObjectTable GetObjectTable (string sql)
		{
			return GetObjectTable (_querier.GetDs (sql));
		}

		/// <summary>
		/// Gets the value/display dictionary.
		/// </summary>
		/// <returns>The value display dictionary.</returns>
		/// <param name="databaseField">Database field.</param>
		public Dictionary<string, string> GetValueDisplayDictionary (Dataelus.Database.IDBFieldSimple databaseField)
		{
			return CollectionServices.GetValueDict (_querier.GetUniqueValues (databaseField));
		}

		/// <summary>
		/// Gets the value/display dictionary for each of the given database fields.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="databaseFields">Database fields.</param>
		public Dictionary<Dataelus.Database.IDBFieldSimple, Dictionary<string, string>> GetUniqueValues (IEnumerable<Dataelus.Database.IDBFieldSimple> databaseFields)
		{
			return GetUniqueValues (databaseFields, new StringEqualityComparer ());
		}

		/// <summary>
		/// Gets the value/display dictionary for each of the given database fields.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="databaseFields">Database fields.</param>
		/// <param name="valueKeyComparer">The Equality Comparer for the value key - the key field of the unique values dictionary</param>
		public Dictionary<Dataelus.Database.IDBFieldSimple, Dictionary<string, string>> GetUniqueValues (IEnumerable<Dataelus.Database.IDBFieldSimple> databaseFields, IEqualityComparer<string> valueKeyComparer)
		{
			var dict = new Dictionary<Dataelus.Database.IDBFieldSimple, Dictionary<string, string>> (new Dataelus.Database.IDBFieldSimpleComparer ());
			var values = _querier.GetUniqueValues (databaseFields);
			foreach (var item in values) {
				dict.Add (item.Key, CollectionServices.GetValueDict (item.Value, valueKeyComparer));
			}
			return dict;
		}

		/// <summary>
		/// Gets the value/display dictionary for each of the given database fields.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="databaseFields">Database fields.</param>
		/// <param name="converter">Converter.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public Dictionary<Dataelus.Database.IDBFieldSimple, Dictionary<T, string>> GetUniqueValues <T> (IEnumerable<Dataelus.Database.IDBFieldSimple> databaseFields, Func<System.Data.SqlClient.SqlDataReader, T> converter, IEqualityComparer<T> valueKeyComparer)
		{
			var dict = new Dictionary<Dataelus.Database.IDBFieldSimple, Dictionary<T, string>> (new Dataelus.Database.IDBFieldSimpleComparer ());
			var values = _querier.GetUniqueValues<T> (databaseFields, converter);
			foreach (var item in values) {
				dict.Add (item.Key, CollectionServices.GetValueDict<T> (item.Value, valueKeyComparer));
			}
			return dict;
		}

		/// <summary>
		/// Gets the value/display dictionary.
		/// </summary>
		/// <returns>The value display dictionary.</returns>
		/// <param name="databaseField">Database field.</param>
		public Dictionary<object, string> GetValueObjectDisplayDictionary (Dataelus.Database.IDBFieldSimple databaseField, IEqualityComparer<object> objectComparer)
		{
			return CollectionServices.GetValueDict<object> (_querier.GetUniqueValues (databaseField), objectComparer);
		}

		public Dictionary<object, string> GetValueObjectDisplayDictionary (Dataelus.Database.IDBFieldSimple databaseField)
		{
			return GetValueObjectDisplayDictionary (databaseField, EqualityComparer<object>.Default);
		}

		/// <summary>
		/// Gets the value/display dictionary for each of the given database fields.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="databaseFields">Database fields.</param>
		public Dictionary<Dataelus.Database.IDBFieldSimple, Dictionary<object, string>> GetUniqueValueObjects (IEnumerable<Dataelus.Database.IDBFieldSimple> databaseFields, IEqualityComparer<object> objectComparer)
		{
			return GetUniqueValues<object> (databaseFields, x => x.GetValue (0), objectComparer);
		}

		#endregion

		#region IDataServices2 implementation

		public virtual int GetUniqueValueCount (Dataelus.Database.IDBFieldSimple databaseField)
		{
			return _querier.GetUniqueValuesCount (databaseField);
		}

		public virtual Dictionary<Dataelus.Database.IDBFieldSimple, int> GetUniqueValueCounts (IEnumerable<Dataelus.Database.IDBFieldSimple> databaseFields)
		{
			var dict = new Dictionary<Dataelus.Database.IDBFieldSimple, int> (new Dataelus.Database.IDBFieldSimpleComparer ());
			foreach (var item in databaseFields) {
				dict.Add (item, _querier.GetUniqueValuesCount (item));
			}
			return dict;
		}

		public virtual DBFieldValueListCollection GetUniqueValues (IEnumerable<Dataelus.Database.IDBFieldSimple> databaseFields, int valueCountLimit, IEqualityComparer<string> valueKeyComparer)
		{
			var collection = new DBFieldValueListCollection ();

			foreach (var item in databaseFields) {
				var obj = new DBFieldValueList ();
				obj.FieldDef = item;
				obj.UniqueValueCount = _fieldValueQuerier.GetUniqueValueCount (item); //_querier.GetUniqueValuesCount (item);
				if (obj.UniqueValueCount <= valueCountLimit) {
					obj.FieldValues = _fieldValueQuerier.GetUniqueFieldValues (item, EqualityComparer<object>.Default); //CollectionServices.GetValueDict (_querier.GetUniqueValueObjects (item), EqualityComparer<object>.Default);
				}
				collection.Add (obj);
			}

			return collection;
		}

		public virtual DBFieldTextValueListCollection GetUniqueTextValues (IEnumerable<Dataelus.Database.IDBFieldSimple> databaseFields, int valueCountLimit, IEqualityComparer<string> valueKeyComparer)
		{
			var collection = new DBFieldTextValueListCollection ();

			// For each database field, get the unique value count (SELECT COUNT(DISTINCT [fieldname]) FROM [tablename])
			// If that count is less than the valueCountLimit, then query the unique values from the database
			foreach (var item in databaseFields) {
				var obj = new DBFieldTextValueList ();
				obj.FieldDef = item;
				obj.UniqueValueCount = _fieldValueQuerier.GetUniqueValueCount (item); //_querier.GetUniqueValuesCount (item);
				if ((valueCountLimit <= 0) || (obj.UniqueValueCount <= valueCountLimit)) {
					obj.FieldValues = _fieldValueQuerier.GetUniqueTextFieldValues (item, new StringEqualityComparer ()); //CollectionServices.GetValueDict (_querier.GetUniqueValues (item), new StringEqualityComparer ());
				}
				collection.Add (obj);
			}

			return collection;
		}

		#endregion

		#region Dataelus Table Creators

		/// <summary>
		/// Gets the text table from Table[0] of the given DataSet.
		/// </summary>
		/// <returns>The text table.</returns>
		/// <param name="ds">The DataSet.</param>
		public static Dataelus.Table.TextTable GetTextTable (System.Data.DataSet ds)
		{
			return GetTextTable (DBQuerier.GetTableZero (ds));
		}

		/// <summary>
		/// Gets the text table from the given DataTable.
		/// </summary>
		/// <returns>The text table.</returns>
		/// <param name="dt">The DataTable.</param>
		public static Dataelus.Table.TextTable GetTextTable (System.Data.DataTable dt)
		{
			var txtTbl = new Dataelus.Table.TextTable ();

			foreach (System.Data.DataColumn col in dt.Columns) {
				txtTbl.AddColumn (col.ColumnName);
			}
			foreach (System.Data.DataRow dr in dt.Rows) {
				var row = txtTbl.CreateRow ();
				foreach (var col in txtTbl.Columns) {
					row [col.ColumnName] = dr [col.ColumnName].ToString ();
				}
				txtTbl.AddRow (row);
			}
			return txtTbl;
		}

		/// <summary>
		/// Gets the object table from Table[0] of the given DataSet.
		/// </summary>
		/// <returns>The object table.</returns>
		/// <param name="ds">The DataSet.</param>
		public static Dataelus.Table.ObjectTable GetObjectTable (System.Data.DataSet ds)
		{
			return GetObjectTable (DBQuerier.GetTableZero (ds));
		}

		/// <summary>
		/// Gets the object table from the given DataTable.
		/// </summary>
		/// <returns>The object table.</returns>
		/// <param name="dt">The DataTable.</param>
		public static Dataelus.Table.ObjectTable GetObjectTable (System.Data.DataTable dt)
		{
			var objTbl = new Dataelus.Table.ObjectTable ();

			foreach (System.Data.DataColumn col in dt.Columns) {
				objTbl.AddColumn (col.ColumnName, col.DataType);
			}
			foreach (System.Data.DataRow dr in dt.Rows) {
				var row = objTbl.CreateRow ();
				foreach (var col in objTbl.Columns) {
					var obj = dr [col.ColumnName];
					row [col.ColumnName] = obj.ToNullable ();
				}
				objTbl.AddRow (row);
			}
			return objTbl;
		}

		#endregion

		#region Mono/.NET DataTable Creators

		/// <summary>
		/// Gets the Mono/.NET DataTable from the given ObjectTable.
		/// </summary>
		/// <returns>The mono data table.</returns>
		/// <param name="table">Table.</param>
		public static System.Data.DataTable GetMonoDataTable (Dataelus.Table.ObjectTable table)
		{
			var dt = new System.Data.DataTable ();

			foreach (var col in table.Columns) {
				// Check for types which are generics of System.Nullable(T)
				Type genericType;
				if (col.DataType.IsNullableType (out genericType)) {
					dt.Columns.Add (col.ColumnName, genericType);
				} else {
					dt.Columns.Add (col.ColumnName, col.DataType);
				}
			}

			for (int r = 0; r < table.RowCount; r++) {
				var dr = dt.NewRow ();

				foreach (var col in table.Columns) {
					dr [col.ColumnName] = table [r, col.ColumnName].ToDBNullable ();
				}

				//dr.RowState = System.Data.DataRowState.
				//dr.d

				//dt.Rows

				dt.Rows.Add (dr);
			}

			return dt;
		}

		#endregion


		/// <summary>
		/// Gets the constraints, using the default column indexes.
		/// Assumes columns in this order: (Constraint, Source Table, Source Field, Table, Field).
		/// </summary>
		/// <returns>The constraints.</returns>
		/// <param name="querier">Querier.</param>
		/// <param name="constraintsQuery">Constraints query.</param>
		public static Dataelus.Database.DBConstraintCollection GetConstraints (DBQuerier querier, string constraintsQuery)
		{
			return GetConstraints (querier, constraintsQuery, 0, 1, 2, 3, 4);
		}

		/// <summary>
		/// Gets the constraints (Downloads from the database using the given querier and query string).
		/// </summary>
		/// <returns>The constraints.</returns>
		/// <param name="querier">Querier.</param>
		/// <param name="constraintsQuery">Constraints query.</param>
		/// <param name="colConstraintName">Column index for the constraint name.</param>
		/// <param name="colSourceTable">Column index for the Source table name.</param>
		/// <param name="colSourceField">Column index for the Source field name.</param>
		/// <param name="colTable">Column index for the table name.</param>
		/// <param name="colField">Column index for the field name.</param>
		public static Dataelus.Database.DBConstraintCollection GetConstraints (DBQuerier querier, string constraintsQuery
			, int colConstraintName, int colSourceTable, int colSourceField, int colTable, int colField)
		{
			var obj = new Dataelus.Database.DBConstraintCollection ();
			querier.GetObjects (constraintsQuery, x => {
					obj.AddConstraintField (
						x.GetString (colConstraintName)
					, x.GetString (colSourceTable)
					, x.GetString (colSourceField)
					, x.GetString (colTable)
					, x.GetString (colField)
					);
				});
			return obj;
		}

		/// <summary>
		/// Gets the column schema from the given Table and column names.
		/// </summary>
		/// <returns>The column schema.</returns>
		/// <param name="columnsSchema">Columns schema.</param>
		/// <param name="colTable">Col table.</param>
		/// <param name="colField">Col field.</param>
		/// <param name="colDatType">Col dat type.</param>
		/// <param name="colMaxLen">Col max length.</param>
		/// <param name="colNullable">Col nullable.</param>
		/// <param name="colOrder"></param>
		public static Dataelus.Database.DBFieldCollection GetColumnSchema (System.Data.DataTable columnsSchema
			, string colTable, string colField, string colDatType, string colMaxLen, string colNullable, string colOrder)
		{
			var obj = new Dataelus.Database.DBFieldCollection ();

			try {
				for (int r = 0; r < columnsSchema.Rows.Count; r++) {
					obj.Add (new Dataelus.Database.DBField (
							columnsSchema.Rows [r] [colTable].ToString ()
						, columnsSchema.Rows [r] [colField].ToString ()
						, columnsSchema.Rows [r] [colDatType].ToString ()
						, columnsSchema.Rows [r] [colMaxLen].ToNullable<int> (-1)
						, (columnsSchema.Rows [r] [colNullable].ToNullable<string> ("NO").ToUpper ().Equals ("YES"))
						, columnsSchema.Rows [r] [colOrder].ToNullable<int> (0)
						)
					);
				}
			} catch (Exception ex) {
				/*var lstColumnNames = new System.Collections.Generic.List<string> ();
				for (int c = 0; c < columnsSchema.Columns.Count; c++) {
					lstColumnNames.Add (columnsSchema.Columns [c].ColumnName);
				}*/
				throw ex;
				//throw new Exception (String.Format ("Table Columns: [{0}]", String.Join (",", lstColumnNames)), ex);
			}

			return obj;
		}

		/*
		/// <summary>
		/// Gets the nullable value (if DBNull, returns Null; else returns [value]).
		/// </summary>
		/// <returns>The nullable value.</returns>
		/// <param name="value">Value.</param>
		public static object GetNullable (object value)
		{
			if (System.DBNull.Value.Equals (value))
				return null;
			return value;
		}

		/// <summary>
		/// Gets the nullable string (returns NULL if value is Null or DBNull).
		/// </summary>
		/// <returns>The nullable string.</returns>
		/// <param name="value">Value.</param>
		public static string GetNullableString (object value)
		{
			if (System.DBNull.Value.Equals (value))
				return null;
			if (value == null)
				return null;
			return (string)value;
		}

		/// <summary>
		/// Gets the nullable value of the given type.
		/// </summary>
		/// <returns>The nullable.</returns>
		/// <param name="value">Value.</param>
		/// <param name="defaultValue">Default value.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static T GetNullable<T> (object value, T defaultValue)
		{
			if (GetNullable (value) == null)
				return defaultValue;
			return (T)value;
		}
		*/
	}
}

