using System.Collections.Generic;
using System.Data.SqlClient;

using Dataelus.Database.SQL;

namespace Dataelus.Mono.SQLServer
{
	public interface ISQLServerCommandBuilder
	{
		SqlCommand GetCommandInsert (string tableName, SQLCommandParameterized cmdParam, Dictionary<string, object> insertValues);

		SqlCommand GetCommandUpdate (string tableName, SQLCommandParameterized cmdParamValue, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions, Dictionary<string, object> updateValues);

		SqlCommand GetCommandDelete (string tableName, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions);

		SqlCommand GetCommandSelect (string tableName, IEnumerable<string> fieldNames, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions);

		SqlCommand GetCommandSelectCount (string tableName, SQLCommandParameterized cmdParamCondition, Dictionary<string, object> conditions);
	}
}
