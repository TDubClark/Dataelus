using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// Interface for data services.
	/// </summary>
	public interface IDataServices
	{
		/// <summary>
		/// Gets the text table.
		/// </summary>
		/// <returns>The text table.</returns>
		/// <param name="sql">Sql.</param>
		Table.TextTable GetTextTable (string sql);

		/// <summary>
		/// Gets the object table.
		/// </summary>
		/// <returns>The object table.</returns>
		/// <param name="sql">Sql.</param>
		Table.ObjectTable GetObjectTable (string sql);

		/// <summary>
		/// Gets the value/display dictionary.
		/// </summary>
		/// <returns>The value display dictionary.</returns>
		/// <param name="databaseField">Database field.</param>
		Dictionary<string, string> GetValueDisplayDictionary (Database.IDBFieldSimple databaseField);

		/// <summary>
		/// Gets the value/display dictionary.
		/// </summary>
		/// <returns>The value display dictionary.</returns>
		/// <param name="databaseField">Database field.</param>
		Dictionary<object, string> GetValueObjectDisplayDictionary (Database.IDBFieldSimple databaseField);

		/// <summary>
		/// Gets the value/display dictionary for each of the given database fields.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="databaseFields">Database fields.</param>
		/// <param name="valueKeyComparer">The Equality Comparer for the value key - the key field of the unique values dictionary</param>
		Dictionary<Dataelus.Database.IDBFieldSimple, Dictionary<string, string>> GetUniqueValues (IEnumerable<Database.IDBFieldSimple> databaseFields, IEqualityComparer<string> valueKeyComparer);

		/// <summary>
		/// Gets the value/display dictionary for each of the given database fields.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="databaseFields">Database fields.</param>
		/// <param name="valueKeyComparer">The Equality Comparer for the value key - the key field of the unique values dictionary</param>
		Dictionary<Dataelus.Database.IDBFieldSimple, Dictionary<object, string>> GetUniqueValueObjects (IEnumerable<Database.IDBFieldSimple> databaseFields, IEqualityComparer<object> valueKeyComparer);
	}

	/// <summary>
	/// Interface for data services, version 2.
	/// </summary>
	public interface IDataServices2 : IDataServices
	{
		/// <summary>
		/// Gets the value count for the given field.
		/// </summary>
		/// <returns>The value count.</returns>
		/// <param name="databaseField">Database field.</param>
		int GetUniqueValueCount (Database.IDBFieldSimple databaseField);

		/// <summary>
		/// Gets the unique value count for each of the given fields.
		/// </summary>
		/// <returns>The unique value counts.</returns>
		/// <param name="databaseFields">Database fields.</param>
		Dictionary<Dataelus.Database.IDBFieldSimple, int> GetUniqueValueCounts (IEnumerable<Database.IDBFieldSimple> databaseFields);

		/// <summary>
		/// Gets the value/display dictionary for each of the given database fields, except where it exceeds the given Count limit.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="databaseFields">Database fields.</param>
		/// <param name="valueCountLimit">Count Limit for any value list returned.</param>
		/// <param name="valueKeyComparer">The Equality Comparer for the value key - the key field of the unique values dictionary</param>
		DBFieldValueListCollection GetUniqueValues (IEnumerable<Database.IDBFieldSimple> databaseFields, int valueCountLimit, IEqualityComparer<string> valueKeyComparer);

		/// <summary>
		/// Gets the string value/display dictionary for each of the given database fields, except where it exceeds the given Count limit.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="databaseFields">Database fields.</param>
		/// <param name="valueCountLimit">Count Limit for any value list returned.</param>
		/// <param name="valueKeyComparer">The Equality Comparer for the value key - the key field of the unique values dictionary</param>
		DBFieldTextValueListCollection GetUniqueTextValues (IEnumerable<Database.IDBFieldSimple> databaseFields, int valueCountLimit, IEqualityComparer<string> valueKeyComparer);
	}

	/// <summary>
	/// Base class for a DB field value list.
	/// </summary>
	public class DBFieldValueListBase
	{
		/// <summary>
		/// Gets or sets the Database field definition.
		/// </summary>
		/// <value>The field definition.</value>
		public Dataelus.Database.IDBFieldSimple FieldDef { get; set; }

		/// <summary>
		/// Gets or sets the count of unique values in the given list.
		/// </summary>
		/// <value>The value count.</value>
		public int UniqueValueCount { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether a custom value is allowed.
		/// </summary>
		/// <value><c>true</c> if custom value allowed; otherwise, <c>false</c>.</value>
		public bool CustomValueAllowed { get; set; }
	}

	public class DBFieldTextValueList : DBFieldValueListBase
	{
		/// <summary>
		/// Gets or sets the field value/display pairs.
		/// </summary>
		/// <value>The field values.</value>
		public Dictionary<string, string> FieldValues { get; set; }

		public DBFieldTextValueList ()
			: base ()
		{
			this.FieldValues = new Dictionary<string, string> ();
		}
	}

	public class DBFieldTextValueListCollection : ListBase<DBFieldTextValueList>
	{
		public DBFieldTextValueListCollection ()
			: base ()
		{
		}

		public bool TryFind (Dataelus.Database.IDBField field, out DBFieldTextValueList value)
		{
			int index = _items.FindIndex (x => x.FieldDef.Equals (field));
			if (index < 0) {
				value = null;
				return false;
			}
			value = _items [index];
			return true;
		}
	}

	public class DBFieldValueList : DBFieldValueListBase
	{
		/// <summary>
		/// Gets or sets the field values.
		/// </summary>
		/// <value>The field values.</value>
		public Dictionary<object, string> FieldValues { get; set; }

		public DBFieldValueList ()
			: base ()
		{
			this.FieldValues = new Dictionary<object, string> ();
		}
	}

	public class DBFieldValueListCollection : ListBase<DBFieldValueList>
	{
		public DBFieldValueListCollection ()
			: base ()
		{
		}

		/// <summary>
		/// Tries to find the item.
		/// </summary>
		/// <returns><c>true</c>, if find was tryed, <c>false</c> otherwise.</returns>
		/// <param name="field">Field.</param>
		/// <param name="value">Value.</param>
		public bool TryFind (Database.IDBFieldSimple field, out DBFieldValueList value)
		{
			int index = _items.FindIndex (x => x.FieldDef.Equals (field));
			if (index < 0) {
				value = null;
				return false;
			}
			value = _items [index];
			return true;
		}
	}
}

