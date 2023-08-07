using System;
using System.Collections.Generic;
using System.Data;

namespace Dataelus.Mono
{
	/// <summary>
	/// Data type converter, between System.Data.SqlDbType types and .NET Types.
	/// </summary>
	public class DataTypeConverter : ITypeConverter
	{
		protected Dictionary<SqlDbType, string> _typeDictionary;

		public DataTypeConverter ()
		{
			_typeDictionary = GetSqlDbTypeDictionary ();
		}


		#region Static Methods

		/// <summary>
		/// Gets the System.Data.SqlDbType dictionary to .NET Type Names.
		/// </summary>
		/// <returns>The sql db type dictionary.</returns>
		public static Dictionary<SqlDbType, string> GetSqlDbTypeDictionary ()
		{
			var dict = new Dictionary<SqlDbType, string> ();

			/*
			 * Real  => System.Single
			 * Float => System.Double
			 * 
			 * Decimal/Money => System.Decimal
			*/

			dict.Add (SqlDbType.BigInt, "System.Int64");
			dict.Add (SqlDbType.Binary, "System.Byte[]");
			dict.Add (SqlDbType.Bit, "System.Boolean");
			dict.Add (SqlDbType.Char, "System.String");
			dict.Add (SqlDbType.Date, "System.DateTime");
			dict.Add (SqlDbType.DateTime, "System.DateTime");
			dict.Add (SqlDbType.DateTime2, "System.DateTime");
			dict.Add (SqlDbType.DateTimeOffset, "System.DateTimeOffset");
			dict.Add (SqlDbType.Decimal, "System.Decimal");
			dict.Add (SqlDbType.Float, "System.Double");
			dict.Add (SqlDbType.Int, "System.Int32");
			dict.Add (SqlDbType.Money, "System.Decimal");
			dict.Add (SqlDbType.NChar, "System.String");
			dict.Add (SqlDbType.NText, "System.String");
			dict.Add (SqlDbType.NVarChar, "System.String");
			dict.Add (SqlDbType.Real, "System.Single");
			dict.Add (SqlDbType.SmallInt, "System.Int16");
			dict.Add (SqlDbType.SmallMoney, "System.Decimal");
			dict.Add (SqlDbType.Text, "System.String");
			dict.Add (SqlDbType.Time, "System.TimeSpan");
			dict.Add (SqlDbType.Timestamp, "System.Byte[]");
			dict.Add (SqlDbType.TinyInt, "System.Byte");
			dict.Add (SqlDbType.UniqueIdentifier, "System.Guid");
			dict.Add (SqlDbType.VarBinary, "System.Byte[]");
			dict.Add (SqlDbType.VarChar, "System.String");
			dict.Add (SqlDbType.Variant, "System.Object");
			//dict.Add (SqlDbType.Xml, "System.Xml"); // XML is not a real type

			return dict;
		}

		#endregion

		/// <summary>
		/// Gets the Mono/.NET TypeName string.
		/// </summary>
		/// <returns>The type string.</returns>
		/// <param name="sqlType">Sql type.</param>
		protected virtual Type GetTypeMono (SqlDbType sqlType)
		{
			string typeName;
			if (_typeDictionary.TryGetValue (sqlType, out typeName)) {
				return System.Type.GetType (typeName);
			} else {
				throw new Exception (String.Format ("Could not find System.Data.SqlDbType '{0}' in the dictionary of .NET Type names.", sqlType));
			}
		}

		#region ITypeConverter implementation

		/// <summary>
		/// Gets the type of the data for the given SQL Database Type.
		/// </summary>
		/// <returns>The data type.</returns>
		/// <param name="dbDataType">Db data type (should be a string representation of System.Data.SqlDbType).</param>
		public virtual Type GetDataType (string dbDataType)
		{
			SqlDbType sqlType;
			if (Enum.TryParse<SqlDbType> (dbDataType, true, out sqlType)) {
				return GetTypeMono (sqlType);
			} else {
				throw new ArgumentException (String.Format ("Could not parse '{0}' as a System.Data.SqlDbType.", dbDataType), "dbDataType");
			}
		}

		#endregion
	}

	/// <summary>
	/// Extensions for the SqlDbType Enumeration.
	/// </summary>
	public static class SqlDbTypeExtensions
	{
		/// <summary>
		/// Gets whether the given SQL DataType is an Integer.
		/// </summary>
		/// <returns><c>true</c>, if int was ised, <c>false</c> otherwise.</returns>
		/// <param name="sqlType">Sql type.</param>
		public static bool isInt (this System.Data.SqlDbType sqlType)
		{
			switch (sqlType) {
				case System.Data.SqlDbType.BigInt:
					break;
				case System.Data.SqlDbType.Int:
					break;
				case System.Data.SqlDbType.SmallInt:
					break;
				case System.Data.SqlDbType.TinyInt:
					break;
				default:
					return false;
			}
			return true;
		}

		/// <summary>
		/// Gets whether the given SQL DataType is a Long Integer (64-bit integer).
		/// </summary>
		/// <returns><c>true</c>, if long int was ised, <c>false</c> otherwise.</returns>
		/// <param name="sqlType">Sql type.</param>
		public static bool isLongInt (this SqlDbType sqlType)
		{
			return (sqlType == SqlDbType.BigInt);
		}

		/// <summary>
		/// Gets whether the given SQL DataType is a Mathematical Real number (any number with a decimal point).
		/// Excludes Integer types.
		/// </summary>
		/// <returns><c>true</c>, if int was ised, <c>false</c> otherwise.</returns>
		/// <param name="sqlType">Sql type.</param>
		public static bool isDecimal (this System.Data.SqlDbType sqlType)
		{
			switch (sqlType) {
				case SqlDbType.Decimal:
					break;
				case SqlDbType.Float:
					break;
				case SqlDbType.Money:
					break;
				case SqlDbType.Real:
					break;
				case SqlDbType.SmallMoney:
					break;
				default:
					return false;
			}
			return true;
		}

		/// <summary>
		/// Gets whether the given SQL DataType is a Text/String type.
		/// </summary>
		/// <returns><c>true</c>, if string was ised, <c>false</c> otherwise.</returns>
		/// <param name="sqlType">Sql type.</param>
		public static bool isString (this SqlDbType sqlType)
		{
			switch (sqlType) {
				case SqlDbType.Char:
					break;
				case SqlDbType.NChar:
					break;
				case SqlDbType.NText:
					break;
				case SqlDbType.NVarChar:
					break;
				case SqlDbType.Text:
					break;
				case SqlDbType.VarChar:
					break;
				default:
					return false;
			}
			return true;
		}

		/// <summary>
		/// Gets whether the given SQL DataType is a DateTime type.
		/// </summary>
		/// <returns><c>true</c>, if date time was ised, <c>false</c> otherwise.</returns>
		/// <param name="sqlType">Sql type.</param>
		public static bool isDateTime (SqlDbType sqlType)
		{
			var dict = DataTypeConverter.GetSqlDbTypeDictionary ();

			string value;
			if (dict.TryGetValue (sqlType, out value)) {
				return value.Equals ("System.DateTime");
			}

			throw new ArgumentException (String.Format ("Could not find the SQL Type '{0}' in the type dictionary.", sqlType), "sqlType");
		}
	}
}

