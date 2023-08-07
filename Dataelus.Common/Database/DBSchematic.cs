using System;
using System.Collections.Generic;

namespace Dataelus.Database
{
	/// <summary>
	/// Stores the schematic for a database; including column definitions, primary keys, constraints, and identity fields.
	/// </summary>
	public class DBSchematic
	{
		/// <summary>
		/// Gets or sets the column schema.
		/// </summary>
		/// <value>The column schema.</value>
		public DBFieldCollection ColumnSchema { get; set; }

		/// <summary>
		/// Gets or sets the primary keys.
		/// </summary>
		/// <value>The primary keys.</value>
		public DBPrimaryKeyCollection PrimaryKeys { get; set; }

		/// <summary>
		/// Gets or sets the table constraints.
		/// </summary>
		/// <value>The table constraints.</value>
		public DBConstraintCollection TableConstraints { get; set; }

		/// <summary>
		/// Gets or sets the identity fields.
		/// </summary>
		/// <value>The identity fields.</value>
		public List<DBFieldSimple> IdentityFields { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBSchematic"/> class.
		/// </summary>
		public DBSchematic ()
		{
		}
	}
}

