using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Extensions;

namespace Dataelus.Database
{
	/// <summary>
	/// A collection of Database primary keys.
	/// </summary>
	public class DBPrimaryKeyCollection : ListBase<DBPrimaryKey>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Database.DBPrimaryKeyCollection"/> class.
		/// </summary>
		public DBPrimaryKeyCollection ()
			: base ()
		{
		}

		public List<DBPrimaryKey> GetPrimaryKeys (string tableName)
		{
			var comparer = new StringEqualityComparer ();
			return _items.Where (x => comparer.Equals (tableName, x.TableName)).ToList ();
		}

		public List<string> GetPrimaryKeyFields (string tableName)
		{
			return GetPrimaryKeys (tableName)
				.Select (x => x.Columns.Select (y => y.FieldName).ToList ())
				.ToList ()
				.ToCombinedList ()
				.Distinct (new StringEqualityComparer ())
				.ToList ();
		}

		/// <summary>
		/// Adds the new table key.
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldNames">Field names.</param>
		public void AddNewTableKey (string tableName, System.Collections.Generic.IEnumerable<string> fieldNames)
		{
			_items.Add (new DBPrimaryKey (tableName, fieldNames));
		}

		/// <summary>
		/// Adds the new table key column; Appends the column to an existing key for the given table (if not found, adds a new key).
		/// </summary>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		/// <param name="tableNameComparer">The comparer for Table Names</param>
		public void AddNewTableKeyColumn (string tableName, string fieldName, System.Collections.Generic.IEqualityComparer<string> tableNameComparer)
		{
			int index = _items.FindIndex (x => tableNameComparer.Equals (x.TableName, tableName));
			if (index < 0) {
				AddNewTableKey (tableName, new string[] { fieldName });
			} else {
				_items [index].AddField (fieldName);
			}
		}
	}
}

