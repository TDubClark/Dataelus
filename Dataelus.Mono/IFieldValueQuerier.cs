using System;
using System.Collections.Generic;

namespace Dataelus.Mono
{
	/// <summary>
	/// Interface for querying the values of fields.
	/// </summary>
	public interface IFieldValueQuerier
	{
		int GetUniqueValueCount (Database.IDBFieldSimple field);

		/// <summary>
		/// Gets the text field value/display pairs.
		/// </summary>
		/// <returns>The text field values.</returns>
		/// <param name="field">Field.</param>
		/// <param name="valueComparer">The comparer for key values</param>
		Dictionary<string, string> GetUniqueTextFieldValues (Database.IDBFieldSimple field, IEqualityComparer<string> valueComparer);

		/// <summary>
		/// Gets the field value/display pairs.
		/// </summary>
		/// <returns>The field values.</returns>
		/// <param name="field">Field.</param>
		/// <param name="valueComparer">The comparer for key values</param>
		Dictionary<object, string> GetUniqueFieldValues (Database.IDBFieldSimple field, IEqualityComparer<object> valueComparer);
	}

	public class FieldValueQuerierDefault : IFieldValueQuerier
	{
		DBQuerier _querier;

		public DBQuerier Querier {
			get { return _querier; }
			set { _querier = value; }
		}

		public FieldValueQuerierDefault ()
		{
			
		}

		public FieldValueQuerierDefault (DBQuerier querier)
		{
			if (querier == null)
				throw new ArgumentNullException ("querier");
			_querier = querier;
		}

		#region IFieldValueQuerier implementation

		public int GetUniqueValueCount (Dataelus.Database.IDBFieldSimple field)
		{
			return _querier.GetUniqueValuesCount (field);
		}

		public Dictionary<string, string> GetUniqueTextFieldValues (Dataelus.Database.IDBFieldSimple field, IEqualityComparer<string> valueComparer)
		{
			return CollectionServices.GetValueDict (_querier.GetUniqueValues (field), valueComparer);
		}

		public Dictionary<object, string> GetUniqueFieldValues (Dataelus.Database.IDBFieldSimple field, IEqualityComparer<object> valueComparer)
		{
			return CollectionServices.GetValueDict (_querier.GetUniqueValueObjects (field), valueComparer);
		}

		#endregion
	}
}

