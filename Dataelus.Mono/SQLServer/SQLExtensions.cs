using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Database;
using Dataelus.Database.SQL;

using Dataelus.Mono;

namespace Dataelus.Mono.SQLServer
{
	/// <summary>
	/// Extensions for SQL Server querying objects.
	/// </summary>
	public static class SQLExtensions
	{
		/// <summary>
		/// Gets the array of parameters, for each value in this Command Parameter.
		/// </summary>
		/// <returns>The parameters.</returns>
		/// <param name="item">Item.</param>
		/// <param name="typeConverter">Type converter.</param>
		public static SqlParameter[] GetParameters (this SQLCommandParameter item, SQLServerDataTypeConverter typeConverter)
		{
			return item.Parameters.Select (prm => prm.GetParameter (item.Field, typeConverter)).ToArray ();
		}

		/// <summary>
		/// Gets the parameter for this parameter value.
		/// </summary>
		/// <returns>The parameter.</returns>
		/// <param name="parameterValue">Parameter value.</param>
		/// <param name="dbField">Db field.</param>
		/// <param name="typeConverter">Type converter.</param>
		public static SqlParameter GetParameter (this ParamValue parameterValue, IDBField dbField, SQLServerDataTypeConverter typeConverter)
		{
			System.Data.SqlDbType sqlType;
			if (!typeConverter.TryGetDBDataType (dbField.DataType, out sqlType))
				throw new ArgumentOutOfRangeException ("dbField.DataType", dbField.DataType, String.Format ("The data SQL data type was not recognized '{0}'.", dbField.DataType));

			SqlParameter obj = null;
			if (dbField.MaxLength > 0) {
				obj = new SqlParameter (parameterValue.ParameterName, sqlType, dbField.MaxLength);
			} else {
				obj = new SqlParameter (parameterValue.ParameterName, sqlType);
			}
			obj.Value = parameterValue.Value ?? DBNull.Value;

			// Per this fix (not released yet on the Stable channel), reduce the Size to zero for Null values
			// https://github.com/mono/mono/commit/18fe8197084960179d6844416ffe94c5e79f1f93

			if (sqlType.isInt () && DBNull.Value.Equals (obj.Value)) {
				obj.Size = 0;
			}

			return obj;
		}

		/// <summary>
		/// Gets the parameter.
		/// </summary>
		/// <returns>The parameter.</returns>
		/// <param name="fieldValue">Field value.</param>
		/// <param name="typeConverter">Type converter.</param>
		public static SqlParameter GetParameter (this SQLFieldValue fieldValue, SQLServerDataTypeConverter typeConverter)
		{
			return fieldValue.Parameter.GetParameter (fieldValue.Field, typeConverter);
		}

		/// <summary>
		/// Adds the parameters to this command.
		/// </summary>
		/// <param name="command">Command.</param>
		/// <param name="commandParameters">Command parameters.</param>
		public static void AddParameters (this SqlCommand command, SQLCommandParameterized commandParameters)
		{
			var typeConverter = new SQLServerDataTypeConverter ();

			// For each command parameter object
			foreach (var item in commandParameters.CommandParameters) {
				// For each value in the command paramter object
				foreach (var prm in item.GetParameters (typeConverter)) {
					command.Parameters.Add (prm);
				}
			}
		}

		/// <summary>
		/// Adds the parameters to this command
		/// </summary>
		/// <param name="command">Command.</param>
		/// <param name="parameterValues">Parameter values.</param>
		public static void AddParameters (this SqlCommand command, IEnumerable<SQLFieldValue> parameterValues)
		{
			var typeConverter = new SQLServerDataTypeConverter ();

			// For each command parameter object
			foreach (var item in parameterValues) {
				command.Parameters.Add (item.GetParameter (typeConverter));
			}
		}

		/// <summary>
		/// Adds the parameter to this command.
		/// </summary>
		/// <returns>The parameter.</returns>
		/// <param name="command">Command.</param>
		/// <param name="paramDef">Parameter def.</param>
		/// <param name="columnSchema">Column schema.</param>
		/// <param name="typeConverter">Type converter.</param>
		public static SqlParameter AddParameter (this SqlCommand command, AdHocSqlParameter paramDef, DBFieldCollection columnSchema, SQLServerDataTypeConverter typeConverter)
		{
			string tableName = paramDef.TableName;
			string fieldName = paramDef.FieldName;
			string parameterName = paramDef.ParamName;
			object parameterValue = paramDef.ParamValue;
			return command.AddParameter (tableName, fieldName, parameterName, parameterValue, columnSchema, typeConverter);
		}

		/// <summary>
		/// Adds the parameter to this command
		/// </summary>
		/// <returns>The parameter.</returns>
		/// <param name="command">Command.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		/// <param name="parameterName">Parameter name.</param>
		/// <param name="parameterValue">Parameter value.</param>
		/// <param name="columnSchema">Column schema.</param>
		/// <param name="typeConverter">Type converter.</param>
		public static SqlParameter AddParameter (this SqlCommand command, string tableName, string fieldName, string parameterName, object parameterValue, DBFieldCollection columnSchema, SQLServerDataTypeConverter typeConverter)
		{
			var oField = columnSchema.Find (new DBFieldSimple (tableName, fieldName));
			var oParam = new ParamValue (parameterName, parameterValue);

			return command.Parameters.Add (oParam.GetParameter (oField, typeConverter));
		}

		/// <summary>
		/// Gets a Formatted, readable string.
		/// </summary>
		/// <returns>The string formatted.</returns>
		/// <param name="command">Command.</param>
		public static string ToStringFormatted (this SqlCommand command)
		{
			var sb = new System.Text.StringBuilder ();
			sb.AppendFormat ("Command Text: {0}", command.CommandText);
			sb.AppendLine ();
			sb.AppendLine ();
			sb.AppendLine ("Command Parameters:");

			sb.AppendLine ("  " + String.Join (Environment.NewLine + "  ", command.Parameters.GetParamDesc ()));

			return sb.ToString ();
		}

		public static string[] GetParamDesc (this SqlParameterCollection parameters)
		{
			var lst = new List<string> ();

			foreach (SqlParameter prm in parameters) {
				lst.Add (prm.ToStringFormatted ());
			}

			return lst.ToArray ();
		}

		public static string ToStringFormatted (this SqlParameter parameter)
		{
			string typeString = parameter.SqlDbType.ToString ();
			if (parameter.Size > 0) {
				typeString = String.Format ("{0}({1:d})", typeString, parameter.Size);
			}
			return String.Format ("{0} [{2}] = '{1}'", parameter.ParameterName, parameter.Value, typeString);
		}
	}
}

