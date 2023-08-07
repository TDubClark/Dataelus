using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database
{
	using Extensions;

	/// <summary>
	/// Table constraint collection; manages and searches constraints between tables.
	/// </summary>
	public class DBTableConstraintCollection : ListBase<DBTableConstraint>, IComparer<string>
	{
		/// <summary>
		/// Gets or sets the table name equality comparer.
		/// </summary>
		/// <value>The table name equality comparer.</value>
		public IEqualityComparer<string> TableNameEqualityComparer {
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.TableConstraintCollection"/> class.
		/// </summary>
		/// <param name="tableNameEqualityComparer">Table name equality comparer.</param>
		public DBTableConstraintCollection (IEqualityComparer<string> tableNameEqualityComparer)
			: base ()
		{
			if (tableNameEqualityComparer == null)
				throw new ArgumentNullException ("tableNameEqualityComparer");
			this.TableNameEqualityComparer = tableNameEqualityComparer;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.TableConstraintCollection"/> class.
		/// </summary>
		public DBTableConstraintCollection ()
			: this (new StringEqualityComparer ())
		{

		}

		/// <summary>
		/// Sort this instance according to table name, then reference table name.
		/// </summary>
		public void SortByName ()
		{
			Sort (new DBTableConstraintSorter ());
		}

		/// <summary>
		/// Add the specified table and refTable pair.
		/// </summary>
		/// <param name="table">Table.</param>
		/// <param name="refTable">Reference table.</param>
		internal void Add (string table, string refTable)
		{
			if (!_items.Any (x => x.Equals (table, refTable, this.TableNameEqualityComparer))) {
				Add (new DBTableConstraint (table, refTable));
			}
		}

		/// <summary>
		/// Gets the immediate reference tables for the given table.
		/// </summary>
		/// <returns>The reference tables.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="tableNameComparer">Comparer.</param>
		public List<string> GetReferenceTables (string tableName, IEqualityComparer<string> tableNameComparer)
		{
			// Note: it is necessary to exclude "tableName" from the list of possible references, because there could be a self-referencing table
			//     ex: Parent Record ID
			// This was a problem
			return GetReferenceTables (tableName, new string[] { tableName }, tableNameComparer);
		}

		/// <summary>
		/// Gets the reference tables, except for those contained in [currentRecurseChain].
		/// </summary>
		/// <returns>The reference tables.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="currentRecurseChain">Current recurse chain.</param>
		/// <param name="tableNameComparer">Table name comparer.</param>
		public List<string> GetReferenceTables (string tableName, IEnumerable<string> currentRecurseChain, IEqualityComparer<string> tableNameComparer)
		{
			// Get all reference table names
			var tableNames = _items
				.Where (x => tableNameComparer.Equals (x.TableName, tableName))
				.Select (x => x.ReferenceTableName)
				.ToList ();

			// Filter out any 
			return tableNames
				.Where (x => !currentRecurseChain.Contains (x, tableNameComparer))
				.ToList ();
		}

		/// <summary>
		/// Determines whether [table] directly references [refTable].
		/// </summary>
		/// <returns><c>true</c> if this instance is reference direct the specified table refTable comparer; otherwise, <c>false</c>.</returns>
		/// <param name="table">Table.</param>
		/// <param name="refTable">Reference table.</param>
		/// <param name="tableNameComparer">Comparer.</param>
		public bool IsReferenceDirect (string table, string refTable, IEqualityComparer<string> tableNameComparer)
		{
			return _items.Any (x => x.Equals (table, refTable, tableNameComparer));
		}

		/// <summary>
		/// Gets the join chain between two tables.
		/// </summary>
		/// <returns>The join chain.</returns>
		/// <param name="table">Table.</param>
		/// <param name="refTable">Reference table.</param>
		public List<string> GetJoinChain (string table, string refTable)
		{
			return GetJoinChain (table, refTable, this.TableNameEqualityComparer);
		}

		/// <summary>
		/// Gets the join chain between the two tables.
		/// </summary>
		/// <returns>The join chain.</returns>
		/// <param name="table">Table.</param>
		/// <param name="refTable">Reference table.</param>
		/// <param name="tableNameComparer">Table name comparer.</param>
		public List<string> GetJoinChain (string table, string refTable, IEqualityComparer<string> tableNameComparer)
		{
			var lst = new List<string> ();
			if (BuildJoinChain (table, refTable, lst, tableNameComparer)) {
				lst.Add (table);
			}
			return lst;
		}

		/// <summary>
		/// Builds the join chain; adds refTable to lstReferenceChain if successful.
		/// </summary>
		/// <returns><c>true</c>, if join chain was built, <c>false</c> otherwise.</returns>
		/// <param name="table">Table.</param>
		/// <param name="refTable">Reference table.</param>
		/// <param name="lstReferenceChain">Lst reference chain.</param>
		/// <param name="tableNameComparer">Table name comparer.</param>
		public bool BuildJoinChain (string table, string refTable, List<string> lstReferenceChain, IEqualityComparer<string> tableNameComparer)
		{
			var directRefTables = GetReferenceTables (table, tableNameComparer);

			if (directRefTables.Contains (refTable, tableNameComparer)) {
				lstReferenceChain.Add (refTable);
				return true;
			} else {
				foreach (var directRefTable in directRefTables) {
					if (BuildJoinChain (directRefTable, refTable, lstReferenceChain, tableNameComparer)) {
						lstReferenceChain.Add (directRefTable);
						return true;
					}
				}
			}

			return false;
		}

		/// <summary>
		/// Determines whether [table] directly or indirectly references [refTable].
		/// </summary>
		/// <returns><c>true</c> if this [table] directly or indirectly references [refTable]; otherwise, <c>false</c>.</returns>
		/// <param name="table">Table.</param>
		/// <param name="refTable">Reference table.</param>
		public bool IsReferenceIndirect (string table, string refTable)
		{
			return IsReferenceIndirect (table, refTable, this.TableNameEqualityComparer);
		}

		/// <summary>
		/// Determines whether [table] directly or indirectly references [refTable].
		/// </summary>
		/// <returns><c>true</c> if this [table] directly or indirectly references [refTable]; otherwise, <c>false</c>.</returns>
		/// <param name="table">Table.</param>
		/// <param name="refTable">Reference table.</param>
		/// <param name="tableNameComparer">Comparer.</param>
		public bool IsReferenceIndirect (string table, string refTable, IEqualityComparer<string> tableNameComparer)
		{
			var recursiveList = new List<string> ();
			return IsReferenceIndirect (table, refTable, tableNameComparer, recursiveList.AddToNew (table));
		}

		/// <summary>
		/// Determines whether this instance is reference indirect the specified table refTable tableNameComparer recursiveList.
		/// </summary>
		/// <returns><c>true</c> if this instance is reference indirect the specified table refTable tableNameComparer recursiveList;
		/// otherwise, <c>false</c>.</returns>
		/// <param name="table">Table.</param>
		/// <param name="refTable">Reference table.</param>
		/// <param name="tableNameComparer">Table name comparer.</param>
		/// <param name="recursiveList">Recursive list.</param>
		protected bool IsReferenceIndirect (string table, string refTable, IEqualityComparer<string> tableNameComparer, List<string> recursiveList)
		{
			// Get the list of tables directly referenced by [table]
			var directRefTables = GetReferenceTables (table, recursiveList, tableNameComparer);

			if (directRefTables.Contains (refTable, tableNameComparer)) {
				return true;
			} else {
				// Check whether any of the direct reference tables point to [refTable]
				foreach (var directRefTable in directRefTables) {
					if (IsReferenceIndirect (directRefTable, refTable, tableNameComparer, recursiveList.AddToNew (table))) {
						return true;
					}
				}
			}
			return false;
		}

		#region IComparer implementation

		/// <summary>
		/// Compare the specified tables 1 and 2, sorts according to dependancy; the more-dependant table will be last.
		/// </summary>
		/// <param name="table1">The first table.</param>
		/// <param name="table2">The second table.</param>
		public int Compare (string table1, string table2)
		{
			if (IsReferenceIndirect (table1, table2, this.TableNameEqualityComparer)) {
				// Put table1 after table2 (table2 is the reference table, so it goes first in the list)
				return 1;
			} else if (IsReferenceIndirect (table2, table1, this.TableNameEqualityComparer)) {
				// Put table1 before table2 (table1 is the reference table, so it goes first in the list)
				return -1;
			} else {
				return 0;
			}
		}

		#endregion
	}
}

