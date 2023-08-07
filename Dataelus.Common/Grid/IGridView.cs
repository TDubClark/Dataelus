using System;

namespace Dataelus.Grid
{
	/// <summary>
	/// Interface for a grid View.
	/// </summary>
	public interface IGridView
	{
		/// <summary>
		/// Gets the row count.
		/// </summary>
		/// <returns>The row count.</returns>
		int getRowCount ();

		/// <summary>
		/// Gets the column count.
		/// </summary>
		/// <returns>The column count.</returns>
		int getColumnCount ();

		/// <summary>
		/// Gets the index of the column.
		/// </summary>
		/// <returns>The column index.</returns>
		/// <param name="columnName">Column name.</param>
		int getColumnIndex (string columnName);

		/// <summary>
		/// Gets the name of the column.
		/// </summary>
		/// <returns>The column name.</returns>
		/// <param name="columnIndex">Column index.</param>
		string getColumnName (int columnIndex);
	}
}

