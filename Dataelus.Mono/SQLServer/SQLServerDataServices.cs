using System;

namespace Dataelus.Mono.SQLServer
{
	/// <summary>
	/// MS SQL Server data services.
	/// </summary>
	public static class SQLServerDataServices
	{
		public static System.Data.DataTable GetColumnSchemaTable (Mono.DBQuerier querier)
		{
			return querier.GetSchemaTableColumns ();
		}

		#region Extension Methods

		/// <summary>
		/// Gets the constraints.
		/// </summary>
		/// <returns>The constraints.</returns>
		/// <param name="querier">Querier.</param>
		public static Database.DBConstraintCollection GetConstraints (this Mono.DBQuerier querier)
		{
			return LoadConstraintsSqlServer (querier);
		}

		/// <summary>
		/// Gets the column schema object.
		/// </summary>
		/// <returns>The column schema.</returns>
		/// <param name="querier">Querier.</param>
		public static Database.DBFieldCollection GetColumnSchema (this Mono.DBQuerier querier)
		{
			return LoadColumnSchemaSqlServer (querier);
		}

		#endregion

		public static Database.DBConstraintCollection LoadConstraintsSqlServer (Mono.DBQuerier querier)
		{
			int colConstraintName;
			int colTable;
			int colField;
			int colRefTable;
			int colRefField;
			string constraintsQuery = SQLLib.GetConstraintsSql (out colConstraintName, out colTable, out colField, out colRefTable, out colRefField);

			return Mono.DataServices.GetConstraints (
				querier
				, constraintsQuery
				, colConstraintName
				, colRefTable, colRefField
				, colTable, colField
			);
		}

		public static Dataelus.Database.DBFieldCollection LoadColumnSchemaSqlServer (DBQuerier querier)
		{
			return LoadColumnSchemaSqlServer (GetColumnSchemaTable (querier));
		}

		/// <summary>
		/// Loads the column schema for Microsoft SQL Server.
		/// </summary>
		/// <returns>The column schema sql server.</returns>
		/// <param name="dtColumnsSchema">Dt columns schema.</param>
		public static Database.DBFieldCollection LoadColumnSchemaSqlServer (System.Data.DataTable dtColumnsSchema)
		{
			return Mono.DataServices.GetColumnSchema (
				dtColumnsSchema
				, "TABLE_NAME"
				, "COLUMN_NAME"
				, "DATA_TYPE"
				, "CHARACTER_MAXIMUM_LENGTH"
				, "IS_NULLABLE"
				, "ORDINAL_POSITION"
			);
		}

		/// <summary>
		/// Gets the primary keys, using the given SQL Server Querier object.
		/// </summary>
		/// <returns>The primary keys.</returns>
		/// <param name="querier">Querier.</param>
		public static Database.DBPrimaryKeyCollection GetPrimaryKeys (this DBQuerier querier)
		{
			string colTable;
			string colField;
			int colTableIndex;
			int colFieldIndex;

			string sql = SQLLib.GetPrimaryKeysOrderedSql (out colTable, out colField, out colTableIndex, out colFieldIndex);

			var obj = new Dataelus.Database.DBPrimaryKeyCollection ();

			// Get the whole thing as a list of 2-item arrays: { Table Name, Field Name }
			var lst = new System.Collections.Generic.List<string[]> ();
			querier.GetObjects (sql, x => {
					lst.Add (new string[] { x.GetString (colTableIndex), x.GetString (colFieldIndex) });
				});


			var comparer = new StringEqualityComparer ();

			// This is optimized (pre-maturely) - assuming 
			var lstFields = new System.Collections.Generic.List<string> ();
			string tableNameCurrent = null;
			bool isReset = true;
			foreach (var item in lst) {
				isReset = false;

				if (tableNameCurrent == null) {
					isReset = true;
				} else if (!comparer.Equals (tableNameCurrent, item [0])) {
					// New Table Name!

					// Store everything saved
					obj.AddNewTableKey (tableNameCurrent, lstFields);

					// Update cursors
					isReset = true;
				}
				if (isReset) {
					tableNameCurrent = item [0];
					lstFields = new System.Collections.Generic.List<string> ();
				}
				lstFields.Add (item [1]);
			}

			// If there were any items, then we will need to save the last item
			if (lst.Count > 0)
				obj.AddNewTableKey (tableNameCurrent, lstFields);

			return obj;
			
		}
	}
}

