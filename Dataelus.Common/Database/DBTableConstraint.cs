using System;
using System.Collections.Generic;

namespace Dataelus.Database
{
	/// <summary>
	/// Database Table constraint.
	/// </summary>
	public class DBTableConstraint
	{
		/// <summary>
		/// Gets or sets the name of the table.
		/// </summary>
		/// <value>The name of the table.</value>
		public string TableName { get; set; }

		/// <summary>
		/// Gets or sets the name of the reference table (referenced by TableName).
		/// </summary>
		/// <value>The name of the reference table.</value>
		public string ReferenceTableName { get; set; }

		public bool Equals (string table, string refTable, IEqualityComparer<string> comparer)
		{
			return comparer.Equals (this.TableName, table) && comparer.Equals (this.ReferenceTableName, refTable);
		}

		public DBTableConstraint (string table, string refTable)
		{
			TableName = table;
			ReferenceTableName = refTable;
		}
	}

	/// <summary>
	/// Database Table constraint sorter.
	/// </summary>
	public class DBTableConstraintSorter : IComparer<DBTableConstraint>
	{
		public DBTableConstraintSorter ()
		{
		}

		#region IComparer implementation

		public int Compare (DBTableConstraint x, DBTableConstraint y)
		{
			int result = x.TableName.CompareTo (y.TableName);
			if (result == 0)
				result = x.ReferenceTableName.CompareTo (y.ReferenceTableName);
			return result;
		}

		#endregion
	}
}

