using System;

namespace Dataelus.Mono.SQLServer
{
	/// <summary>
	/// SQL library, for Microsoft SQL Server.
	/// </summary>
	public class SQLLib
	{
		public SQLLib ()
		{
		}

		public static string GetConstraintsSql (out int colConstraintName
			, out int colTable, out int colField
			, out int colRefTable, out int colRefField)
		{
			colConstraintName = 0;
			colTable = 2;
			colField = 3;
			colRefTable = 4;
			colRefField = 5;

			return GetConstraintsSql ();
		}

		public static string GetConstraintsSql (out string colConstraintName
			, out string colTable, out string colField
			, out string colRefTable, out string colRefField)
		{
			colConstraintName = "FK_NAME";
			colTable = "table";
			colField = "column";
			colRefTable = "referenced_table";
			colRefField = "referenced_column";

			return GetConstraintsSql ();
		}

		public static string GetConstraintsSql ()
		{
			string sql = "SELECT  obj.name AS FK_NAME,"
			             + " sch.name AS [schema_name],"
			             + " tab1.name AS [table],"
			             + " col1.name AS [column],"
			             + " tab2.name AS [referenced_table],"
			             + " col2.name AS [referenced_column]"
			             + " FROM sys.foreign_key_columns AS fkc"
			             + " INNER JOIN sys.objects AS obj"
			             + " ON obj.object_id = fkc.constraint_object_id"
			             + " INNER JOIN sys.tables AS tab1"
			             + " ON tab1.object_id = fkc.parent_object_id"
			             + " INNER JOIN sys.schemas AS sch"
			             + " ON tab1.schema_id = sch.schema_id"
			             + " INNER JOIN sys.columns AS col1"
			             + " ON col1.column_id = parent_column_id AND col1.object_id = tab1.object_id"
			             + " INNER JOIN sys.tables AS tab2"
			             + " ON tab2.object_id = fkc.referenced_object_id"
			             + " INNER JOIN sys.columns AS col2"
			             + " ON col2.column_id = referenced_column_id AND col2.object_id = tab2.object_id";
			return sql;
		}

		/// <summary>
		/// Gets the SQL for finding the list of primary keys (ordered).  fields are: 'tablename' and 'primarykeycolumn'
		/// </summary>
		/// <returns>The SQL for getting the primary keys (ordered).</returns>
		/// <param name="colTable">Col table.</param>
		/// <param name="colField">Col field.</param>
		public static string GetPrimaryKeysOrderedSql (out string colTable, out string colField, out int colTableIndex, out int colFieldIndex)
		{
			colTable = "tablename";
			colField = "primarykeycolumn";

			colTableIndex = 0;
			colFieldIndex = 1;
			// Copied from source: http://stackoverflow.com/questions/3930338/sql-server-get-table-primary-key-using-sql-query/3942921#3942921

			string sql = "SELECT KU.table_name AS tablename, column_name AS primarykeycolumn"
			             + " FROM INFORMATION_SCHEMA.TABLE_CONSTRAINTS AS TC"
			             + " INNER JOIN INFORMATION_SCHEMA.KEY_COLUMN_USAGE AS KU"
			             + "    ON TC.CONSTRAINT_TYPE = 'PRIMARY KEY'"
			             + "   AND TC.CONSTRAINT_NAME = KU.CONSTRAINT_NAME"
			             + " ORDER BY KU.TABLE_NAME, KU.ORDINAL_POSITION";
			return sql;
		}
	}
}

