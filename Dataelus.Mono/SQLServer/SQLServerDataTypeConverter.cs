using System;
using System.Collections.Generic;
using System.Data;

namespace Dataelus.Mono.SQLServer
{
	/// <summary>
	/// Microsoft SQL Server data type converter.
	/// </summary>
	public class SQLServerDataTypeConverter : DataTypeConverter
	{
		protected Dictionary<string, SqlDbType> _sqlserverDictionary;

		public SQLServerDataTypeConverter ()
		{
			_sqlserverDictionary = GetMSSQLServerDataType ();
		}

		/// <summary>
		/// Gets the Microsoft SQL Server Data Type for the given type.
		/// </summary>
		/// <returns>The SQL server data type.</returns>
		public Dictionary<string, SqlDbType> GetMSSQLServerDataType ()
		{
			var dict = new Dictionary<string, SqlDbType> (new Dataelus.StringEqualityComparer ());

			dict.Add ("bigint", SqlDbType.BigInt);
			dict.Add ("binary", SqlDbType.VarBinary);
			dict.Add ("bit", SqlDbType.Bit);
			dict.Add ("char", SqlDbType.Char);
			dict.Add ("date", SqlDbType.Date);
			dict.Add ("datetime", SqlDbType.DateTime);
			dict.Add ("datetime2", SqlDbType.DateTime2);
			dict.Add ("datetimeoffset", SqlDbType.DateTimeOffset);
			dict.Add ("decimal", SqlDbType.Decimal);
			dict.Add ("varbinary(max)", SqlDbType.VarBinary);
			dict.Add ("float", SqlDbType.Float);
			dict.Add ("image", SqlDbType.Binary);
			dict.Add ("int", SqlDbType.Int);
			dict.Add ("money", SqlDbType.Money);
			dict.Add ("nchar", SqlDbType.NChar);
			dict.Add ("ntext", SqlDbType.NText);
			dict.Add ("numeric", SqlDbType.Decimal);
			dict.Add ("nvarchar", SqlDbType.NVarChar);
			dict.Add ("real", SqlDbType.Real);
			dict.Add ("rowversion", SqlDbType.Timestamp);
			dict.Add ("smalldatetime", SqlDbType.DateTime);
			dict.Add ("smallint", SqlDbType.SmallInt);
			dict.Add ("smallmoney", SqlDbType.SmallMoney);
			dict.Add ("sql_variant", SqlDbType.Variant);
			dict.Add ("text", SqlDbType.Text);
			dict.Add ("time", SqlDbType.Time);
			dict.Add ("timestamp", SqlDbType.Timestamp);
			dict.Add ("tinyint", SqlDbType.TinyInt);
			dict.Add ("uniqueidentifier", SqlDbType.UniqueIdentifier);
			dict.Add ("varbinary", SqlDbType.VarBinary);
			dict.Add ("varchar", SqlDbType.VarChar);
			dict.Add ("xml", SqlDbType.Xml);

			return dict;
		}

		/// <summary>
		/// Tries to get the type of the DB data.
		/// </summary>
		/// <returns><c>true</c>, if DB data type was gotten, <c>false</c> otherwise.</returns>
		/// <param name="dbDataType">Db data type.</param>
		/// <param name="sqlType">Sql type.</param>
		public bool TryGetDBDataType (string dbDataType, out SqlDbType sqlType)
		{
			return _sqlserverDictionary.TryGetValue (dbDataType, out sqlType);
		}

		public override Type GetDataType (string dbDataType)
		{
			SqlDbType sqlType;
			if (_sqlserverDictionary.TryGetValue (dbDataType, out sqlType)) {
				return base.GetTypeMono (sqlType);
			} else {
				throw new ArgumentException (String.Format ("Could not identify type '{0}'.", dbDataType), "dbDataType");
			}
		}
	}
}

