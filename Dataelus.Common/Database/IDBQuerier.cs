using System;

namespace Dataelus.Database
{
	/// <summary>
	/// Interface for a Database querier.
	/// </summary>
	public interface IDBQuerier
	{
		/// <summary>
		/// Gets the record count for the given Database field.
		/// </summary>
		/// <returns>The record count.</returns>
		/// <param name="field">Field.</param>
		int GetRecordCount (IDBFieldSimple field);

		/// <summary>
		/// Gets the record count for the given Field of the given Table.
		/// </summary>
		/// <returns>The record count.</returns>
		/// <param name="tableName">Table name.</param>
		/// <param name="fieldName">Field name.</param>
		int GetRecordCount (string tableName, string fieldName);
	}
}

