using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Extensions;

namespace Dataelus.Database
{
	/// <summary>
	/// A collection of Database constraints.
	/// </summary>
	public class DBConstraintCollection : CollectionBase<DBConstraint>, System.Collections.IEnumerable
	{
		protected IEqualityComparer<string> _fieldComparer;

		/// <summary>
		/// Gets or sets the comparer for database field names.
		/// </summary>
		/// <value>The field comparer.</value>
		public IEqualityComparer<string> FieldComparer {
			get { return _fieldComparer; }
			set { _fieldComparer = value; }
		}

		protected IEqualityComparer<string> _constraintNameComparer;

		/// <summary>
		/// Gets or sets the constraint name comparer.
		/// </summary>
		/// <value>The constraint name comparer.</value>
		public IEqualityComparer<string> ConstraintNameComparer {
			get { return _constraintNameComparer; }
			set { _constraintNameComparer = value; }
		}

		public DBConstraintCollection ()
			: this (new Dataelus.Database.SQL.SQLFieldStringEqualityComparer ()
				, new StringEqualityComparer ())
		{
		}

		public DBConstraintCollection (IEqualityComparer<string> fieldComparer, IEqualityComparer<string> constraintNameComparer)
			: base ()
		{
			_fieldComparer = fieldComparer;
			_constraintNameComparer = constraintNameComparer;
		}

		public bool IsConstrained (string table1, string table2)
		{
			return _items.Any (x => x.IsConstrainedBy (table1, table2) || x.IsConstrainedBy (table2, table1));
		}

		/// <summary>
		/// Gets the table constraints collection.
		/// </summary>
		/// <returns>The constraints.</returns>
		public DBTableConstraintCollection GetConstraints ()
		{
			var lst = new DBTableConstraintCollection ();

			foreach (var item in _items) {
				foreach (var col in item.ConstraintColumns) {
					lst.Add (col.Column.TableName, col.ReferencedColumn.TableName);
				}
			}

			return lst;
		}

		/// <summary>
		/// Builds the list of all tables involved in any constraint.
		/// </summary>
		/// <returns>The list tables.</returns>
		public string[] BuildListTables ()
		{
			var lst = new List<string> ();

			var srcTables = _items.Select (x => x.ConstraintColumns.Select (y => y.Column.TableName).ToList ()).ToList ().ToCombinedList ();
			lst.AddRange (srcTables);

			var refTables = _items.Select (x => x.ConstraintColumns.Select (y => y.ReferencedColumn.TableName).ToList ()).ToList ().ToCombinedList ();
			lst.AddRange (refTables);

			return lst.Distinct (new StringEqualityComparer ()).ToArray ();
		}

		/// <summary>
		/// Gets the tables list, sorted by dependency (most dependent last).
		/// </summary>
		/// <returns>The list tables sorted.</returns>
		public List<string> GetListTablesSorted ()
		{
			var lst = BuildListTables ().ToList ();

			lst.Sort (GetDependancyComparer ());

			return lst;
		}

		/// <summary>
		/// Gets the dependancy comparer.
		/// </summary>
		/// <returns>The dependancy comparer.</returns>
		public DBTableDependancyComparer GetDependancyComparer ()
		{
			return new DBTableDependancyComparer (GetConstraints ());
		}

		/// <summary>
		/// Adds the constraint field.
		/// </summary>
		/// <param name="constraintName">Constraint name.</param>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		public void AddConstraintField (string constraintName, string sourceTableName, string sourceFieldName, string tableName, string fieldname)
		{
			int index = _items.FindIndex (x => _constraintNameComparer.Equals (x.ConstraintName, constraintName));
			if (index < 0) {
				var newField = new DBConstraint (constraintName, _fieldComparer);
				newField.AddColumn (sourceTableName, sourceFieldName, tableName, fieldname);
				_items.Add (newField);
			} else {
				_items [index].AddColumn (sourceTableName, sourceFieldName, tableName, fieldname);
			}
		}

		/// <summary>
		/// Finds the indexes for all single-column constraints.
		/// </summary>
		/// <returns>The all index single.</returns>
		public int[] FindAllIndexSingle ()
		{
			return base.FindIndexAll (x => x.ConstraintColumns.Count == 1);
		}

		/// <summary>
		/// Finds all single-column constraints (where the constraint applies to only one column).
		/// </summary>
		/// <returns>The all single.</returns>
		public DBConstraint[] FindAllSingle ()
		{
			return _items.FindAll (x => x.ConstraintColumns.Count == 1).ToArray ();
		}

		public DBConstraint[] FindAllMultiple ()
		{
			return _items.FindAll (x => x.ConstraintColumns.Count > 1).ToArray ();
		}

		#region Retrievers and Searchers

		/// <summary>
		/// Gets the constraints for the given table and field.
		/// </summary>
		/// <returns><c>true</c>, if constraints was gotten, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="constraints">Constraints.</param>
		public bool GetConstraints (IDBFieldSimple field, out DBConstraint[] constraints)
		{
			constraints = _items
				.Where (x => x.IsMatchTarget (field))
				.ToArray ();
			return (constraints.Length > 0);
		}

		/// <summary>
		/// Gets the constraints for the given table and field.
		/// </summary>
		/// <returns><c>true</c>, if constraint was gotten, <c>false</c> otherwise.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		/// <param name="constraints">Constraints.</param>
		public bool GetConstraints (string tableName, string fieldname, out DBConstraint[] constraints)
		{
			constraints = _items
				.Where (x => x.IsMatchTarget (tableName, fieldname))
				.ToArray ();
			return (constraints.Length > 0);
		}

		/// <summary>
		/// Gets the constraints where there is a single field, for the given table and field.
		/// These constraints would apply only to the given table/field.
		/// </summary>
		/// <returns><c>true</c>, if constrains single field was gotten, <c>false</c> otherwise.</returns>
		/// <param name="field">The field.</param>
		/// <param name="constraints">Constraints.</param>
		public bool GetConstraintsSingleField (IDBFieldSimple field, out DBConstraint[] constraints)
		{
			constraints = FindAllSingle ()
				.Where (x => x.IsMatchTarget (field))
				.ToArray ();
			return (constraints.Length > 0);
		}

		/// <summary>
		/// Gets the constraints where there is a single field, for the given table and field.
		/// These constraints would apply only to the given table/field.
		/// </summary>
		/// <returns><c>true</c>, if constrains single field was gotten, <c>false</c> otherwise.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldname">Fieldname.</param>
		/// <param name="constraints">Constraints.</param>
		public bool GetConstraintsSingleField (string tableName, string fieldname, out DBConstraint[] constraints)
		{
			constraints = FindAllSingle ()
				.Where (x => x.IsMatchTarget (tableName, fieldname))
				.ToArray ();
			return (constraints.Length > 0);
		}

		/// <summary>
		/// Gets the constraints where there is are multiple fields, for the given table and field.
		/// These constraints would apply only to the given table/field.
		/// </summary>
		/// <returns><c>true</c>, if constrains single field was gotten, <c>false</c> otherwise.</returns>
		/// <param name="field">The field.</param>
		/// <param name="constraints">Constraints.</param>
		public bool GetConstraintsMultiField (IDBFieldSimple field, out DBConstraint[] constraints)
		{
			constraints = FindAllMultiple ()
				.Where (x => x.IsMatchTarget (field))
				.ToArray ();
			return (constraints.Length > 0);
		}

		public bool GetConstraintsMultiField (string tableName, string fieldname, out DBConstraint[] constraints)
		{
			constraints = FindAllMultiple ()
				.Where (x => x.IsMatchTarget (tableName, fieldname))
				.ToArray ();
			return (constraints.Length > 0);
		}

		public bool GetConstraintsMultiField (string tableName, out DBConstraint[] constraints)
		{
			var comparer = new StringEqualityComparer ();
			constraints = FindAllMultiple ()
				.Where (x => x.IsMatchTarget (tableName, comparer))
				.ToArray ();
			return (constraints.Length > 0);
		}

		#endregion

		#region System.Collections.IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return base.GetEnumerator ();
		}

		#endregion
	}
}

