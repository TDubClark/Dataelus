using System;

namespace Dataelus.Wizard.Selection
{
	/// <summary>
	/// Interface for a View of a text grid
	/// </summary>
	public interface ITextGridView
	{
		/// <summary>
		/// Gets the cell text.
		/// </summary>
		/// <returns>The cell text.</returns>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnIndex">Column index.</param>
		string GetCellText (int rowIndex, int columnIndex);

		/// <summary>
		/// Gets the cell text.
		/// </summary>
		/// <returns>The cell text.</returns>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		string GetCellText (int rowIndex, string columnName);

		/// <summary>
		/// Gets the index of the row, which matches all the given values.
		/// </summary>
		/// <returns>The row index.</returns>
		/// <param name="values">Values.</param>
		int GetRowIndex(ICellText[] values);
	}
}

