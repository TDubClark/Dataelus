using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.Database
{
	/// <summary>
	/// A collection of database fields.
	/// </summary>
	public class DBFieldCollection : CollectionBase<DBField>, System.Collections.IEnumerable
	{
		public DBFieldCollection ()
			: base ()
		{
		}

		public DBField Find (IDBFieldSimple fieldSimple)
		{
			return _items.Find (x => x.Equals (fieldSimple));
		}

		public DBField Find (string tableName, string fieldName)
		{
			return Find (tableName, fieldName, new StringEqualityComparer ());
		}

		public DBField Find (string tableName, string fieldName, IEqualityComparer<string> comparer)
		{
			return _items.Find (x => comparer.Equals (x.TableName, tableName) && comparer.Equals (x.FieldName, fieldName));
		}

		/// <summary>
		/// Gets the array of table names.
		/// </summary>
		/// <returns>The table names.</returns>
		public string[] GetTableNames ()
		{
			var list = _items
				.Select (x => x.TableName)
				.Distinct (new StringEqualityComparer ())
				.ToList ();
			
			list.Sort ();

			return list.ToArray ();
		}

		/// <summary>
		/// Gets the field names for the given table.
		/// </summary>
		/// <returns>The field names.</returns>
		/// <param name="tableName">Table name.</param>
		public string[] GetFieldNames (string tableName)
		{
			return GetFieldNames (tableName, new StringEqualityComparer ());
		}

		/// <summary>
		/// Gets the field names for the given table.
		/// </summary>
		/// <returns>The field names.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="comparer">Comparer.</param>
		public string[] GetFieldNames (string tableName, IEqualityComparer<string> comparer)
		{
			var list = _items
				.Where (x => comparer.Equals (x.TableName, tableName))
				.Select (x => x.FieldName)
				.Distinct (new StringEqualityComparer ())
				.ToList ();

			list.Sort ();

			return list.ToArray ();
		}

		/// <summary>
		/// Gets the fields for the given table.
		/// </summary>
		/// <returns>The fields.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="comparer">Comparer.</param>
		public DBField[] GetFields (string tableName, IEqualityComparer<string> comparer)
		{
			return _items
				.Where (x => comparer.Equals (x.TableName, tableName))
				.ToArray ();
		}

		#region System.Collections.IEnumerable implementation (explicit)

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return base.GetEnumerator ();
		}

		#endregion
	}
}

