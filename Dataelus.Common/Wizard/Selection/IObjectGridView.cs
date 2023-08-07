using System;

namespace Dataelus.Wizard.Selection
{
	/// <summary>
	/// Interface for a View of an object grid
	/// </summary>
	public interface IObjectGridView
	{
		/// <summary>
		/// Gets the cell value.
		/// </summary>
		/// <returns>The cell value.</returns>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnIndex">Column index.</param>
		object GetCellValue (int rowIndex, int columnIndex);

		/// <summary>
		/// Gets the cell value.
		/// </summary>
		/// <returns>The cell value.</returns>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		object GetCellValue (int rowIndex, string columnName);

		/// <summary>
		/// Gets the index of the row, which matches all the given values.
		/// </summary>
		/// <returns>The row index.</returns>
		/// <param name="values">Values.</param>
		int GetRowIndex(ICellObject[] values);
	}
}

