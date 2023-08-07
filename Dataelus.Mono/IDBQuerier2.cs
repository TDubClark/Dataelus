using System;

namespace Dataelus.Mono
{
	/// <summary>
	/// Interface for a Database querier (version 2)
	/// </summary>
	public interface IDBQuerier2 : Dataelus.Database.IDBQuerier
	{
		/// <summary>
		/// Gets the DataSet for the given SQL Command.
		/// </summary>
		/// <returns>The DataSet.</returns>
		/// <param name="sqlCommand">Sql command.</param>
		System.Data.DataSet GetDs (string sqlCommand);

		/// <summary>
		/// Gets the unique values for the given Database Field.
		/// </summary>
		/// <returns>The unique values.</returns>
		/// <param name="field">The Database Field.</param>
		System.Collections.Generic.List<string> GetUniqueValues (Dataelus.Database.IDBFieldSimple field);
	}
}

