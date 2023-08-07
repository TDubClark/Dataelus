using System;
using System.Collections.Generic;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// Interface for a parameterized sql command.
	/// </summary>
	public interface ISQLCommandParameterized
	{
		/// <summary>
		/// Gets or sets the sql where clauses.
		/// </summary>
		/// <value>The sql where clauses.</value>
		List<SQLWhereClause> SqlWhereClauses{ get; set; }

		/// <summary>
		/// Gets or sets the command parameters.
		/// </summary>
		/// <value>The command parameters.</value>
		List<SQLCommandParameter> CommandParameters{ get; set; }
	}
}

