using System;

namespace Dataelus.Database.SQL
{
	/// <summary>
	/// Sql where clause (a SQL statement, which is associated with a Database Field).
	/// </summary>
	public class SQLWhereClause
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLWhereClause"/> class.
		/// </summary>
		public SQLWhereClause ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.SQL.SQLWhereClause"/> class.
		/// </summary>
		/// <param name="field">The Database Field.</param>
		/// <param name="sql">The SQL string part.</param>
		public SQLWhereClause (IDBField field, string sql)
		{
			Field = field;
			Sql = sql;
		}

		/// <summary>
		/// Gets or sets the Database field.
		/// </summary>
		/// <value>The field.</value>
		public IDBField Field { get; set; }

		/// <summary>
		/// Gets or sets the SQL string part.
		/// </summary>
		/// <value>The sql.</value>
		public string Sql { get; set; }
	}
}

