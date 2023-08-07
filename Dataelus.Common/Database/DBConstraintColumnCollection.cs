using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database
{
	public class DBConstraintColumnCollection : ListBase<DBConstraintColumn>
	{
		public bool AssignUniqueIDs { get; set; }

		public UniqueIdentifierManager IDManager { get; set; }

		public DBConstraintColumnCollection ()
			: base ()
		{
			this.AssignUniqueIDs = false;
			this.IDManager = new UniqueIdentifierManager (0);
		}

		/// <summary>
		/// Add a new constraint, using the specified field and referencedField.
		/// </summary>
		/// <param name="field">Field.</param>
		/// <param name="referencedField">Referenced field.</param>
		public DBConstraintColumn Add (DBFieldSimple field, DBFieldSimple referencedField)
		{
			var item = new DBConstraintColumn (field, referencedField);
			Add (item);
			return item;
		}

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public override void Add (DBConstraintColumn item)
		{
			if (this.AssignUniqueIDs) {
				item.UniqueID = (int)this.IDManager.GetUniqueID ();
			}
			base.Add (item);
		}

		/// <summary>
		/// Finds a constraint where the specified field references another column.
		/// </summary>
		/// <param name="field">Field.</param>
		/// <param name="constraint">Constraint.</param>
		/// <param name="fieldNameComparer">Field name comparer.</param>
		public bool FindReference (IDBFieldSimple field, out DBConstraintColumn constraint, IEqualityComparer<string> fieldNameComparer)
		{
			constraint = null;

			int index = _items.FindIndex (x => field.Equals (x.Column, fieldNameComparer));
			if (index < 0)
				return false;
			
			constraint = _items [index];
			return true;
		}

		/// <summary>
		/// Finds any constraints where the specified field is referenced by another column
		/// </summary>
		/// <param name="field">Field.</param>
		/// <param name="fieldNameComparer">Field name comparer.</param>
		public DBConstraintColumn[] FindReferenced (IDBFieldSimple field, IEqualityComparer<string> fieldNameComparer)
		{
			return _items.FindAll (x => field.Equals (x.ReferencedColumn, fieldNameComparer)).ToArray ();
		}


		/// <summary>
		/// Removes the item specified by the given ID.
		/// </summary>
		/// <returns><c>true</c>, if by ID was removed, <c>false</c> otherwise.</returns>
		/// <param name="uniqueID">Unique I.</param>
		public bool RemoveByID (int uniqueID)
		{
			if (GetCountByID (uniqueID) != 1) {
				// Must match exactly one record; otherwise, do nothing
				return false;
			}
			int index = FindIndexByID (uniqueID);
			if (index < 0)
				return false;

			_items.RemoveAt (index);
			return true;
		}

		/// <summary>
		/// Finds the index of the item specified by the given ID.
		/// </summary>
		/// <returns>The index by I.</returns>
		/// <param name="uniqueID">Unique I.</param>
		public int FindIndexByID (int uniqueID)
		{
			return _items.FindIndex (x => x.UniqueID == uniqueID);
		}

		/// <summary>
		/// Gets the count of items which match the given ID.
		/// </summary>
		/// <returns>The count by I.</returns>
		/// <param name="uniqueID">Unique I.</param>
		int GetCountByID (int uniqueID)
		{
			return _items.Count (x => x.UniqueID == uniqueID);
		}

		/// <summary>
		/// Update the item specified by the given ID; sets the fields to the given fields.
		/// Returns True if item was found; else returns false.
		/// </summary>
		/// <param name="uniqueID">Unique ID.</param>
		/// <param name="field">Field.</param>
		/// <param name="referencedField">Referenced field.</param>
		/// <param name="constraint">The updated Constraint object.</param>
		public bool Update (int uniqueID, DBFieldSimple field, DBFieldSimple referencedField, out DBConstraintColumn constraint)
		{
			constraint = null;

			int index = FindIndexByID (uniqueID);
			if (index < 0)
				return false;

			constraint = _items [index];
			constraint.Column = field;
			constraint.ReferencedColumn = referencedField;
			return true;
		}

		/// <summary>
		/// Prioritizes the by record count.
		/// </summary>
		/// <param name="querier">Querier.</param>
		/// <param name="assignOrder">If set to <c>true</c> assign order.</param>
		public void PrioritizeByRecordCount (IDBQuerier querier, bool assignOrder)
		{
			Prioritize (new DBConstraintColumnComparer (querier), assignOrder);
		}

		/// <summary>
		/// Prioritize the specified comparer and assignOrder.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <param name="assignOrder">If set to <c>true</c> assign order.</param>
		public void Prioritize (IComparer<DBConstraintColumn> comparer, bool assignOrder)
		{
			_items.Sort (comparer);

			if (assignOrder)
				AssignOrderNumber ();
		}

		/// <summary>
		/// Gets the name of the reference table.
		/// </summary>
		/// <returns>The reference table name.</returns>
		string GetReferenceTableName ()
		{
			return _items.Select (x => x.ReferencedColumn.TableName).ToList ().First ();
		}

		/// <summary>
		/// Gets the reference fields, enbracketted per MS SQL Server keywords.
		/// </summary>
		/// <returns>The reference fields enbracket.</returns>
		List<string> GetReferenceFieldsEnbracket ()
		{
			return GetReferenceFields ().Select (x => SQLLanguage.SQLLang.Enbracket (x)).ToList ();
		}

		/// <summary>
		/// Gets the list of reference fields.
		/// </summary>
		/// <returns>The reference fields.</returns>
		public List<string> GetReferenceFields ()
		{
			// Make sure this is done in order
			var lst = new List<string> ();
			for (int i = 0; i < _items.Count; i++) {
				lst.Add (_items [i].ReferencedColumn.FieldName);
			}
			return lst;
		}

		public List<string> GetColumnFields ()
		{
			// Make sure this is done in order
			var lst = new List<string> ();
			for (int i = 0; i < _items.Count; i++) {
				lst.Add (_items [i].Column.FieldName);
			}
			return lst;
		}

		/// <summary>
		/// Gets the sql query for the reference table.
		/// </summary>
		/// <returns>The sql reference table.</returns>
		public string GetSqlReferenceTable ()
		{
			return String.Format ("SELECT DISTINCT {0} FROM {1} ORDER BY {0}", String.Join (", ", GetReferenceFieldsEnbracket ()), SQLLanguage.SQLLang.Enbracket (GetReferenceTableName ()));
		}

		/// <summary>
		/// Assigns the order number to each item, according to it's index in the list; should be called after sorting.
		/// </summary>
		public void AssignOrderNumber ()
		{
			for (int i = 0; i < _items.Count; i++) {
				_items [i].FilterOrder = i;
			}
		}

		public override string ToString ()
		{
			if (_items.Count > 0)
				return string.Format ("[tables: {1} => {2}; field count={0:d}]", _items.Count, _items.First ().Column.TableName, _items.First ().ReferencedColumn.TableName);
			return string.Format ("[field count={0:d}]", _items.Count);
		}
	}
}

