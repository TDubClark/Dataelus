using System;

namespace Dataelus.Mono
{
	public interface IDBInteractor
	{
		int ExecuteCommand (System.Data.IDbCommand command);

		/// <summary>
		/// Gets the connection for reading the database.
		/// </summary>
		/// <returns>The read connection.</returns>
		System.Data.IDbConnection GetReadConnection ();

		/// <summary>
		/// Gets the connection for writing to the database.
		/// </summary>
		/// <returns>The write connection.</returns>
		System.Data.IDbConnection GetWriteConnection ();
	}
}

