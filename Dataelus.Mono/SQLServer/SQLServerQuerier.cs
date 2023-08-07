using System;

namespace Dataelus.Mono.SQLServer
{
	public class SQLServerQuerier : DBQuerier
	{
		public SQLServerQuerier (string connectionString)
			: base (connectionString)
		{
		}

		public SQLServerQuerier ()
			: base ()
		{
		}

		// Return the base extension methods

		public Database.DBConstraintCollection GetConstraints ()
		{
			return SQLServerDataServices.GetConstraints (this);
		}

		public Database.DBFieldCollection GetColumnSchema ()
		{
			return SQLServerDataServices.LoadColumnSchemaSqlServer (this);
		}

		public Database.DBPrimaryKeyCollection GetPrimaryKeys ()
		{
			return SQLServerDataServices.GetPrimaryKeys (this);
		}
	}
}

