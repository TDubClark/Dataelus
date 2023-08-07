using System;

// This is the Wizard namespace, for all wizard-type classes
// The Selection Sub-namespace is for the part of the wizard that deals with user selection
namespace Dataelus.Wizard.Selection
{
	/// <summary>
	/// Interface for a View of a grid, which supports selectable rows.
	/// </summary>
	public interface IGridSelectableRowView
	{
		/// <summary>
		/// Gets the indexes of the rows selected.
		/// </summary>
		/// <returns>The rows selected.</returns>
		int[] GetRowsSelected ();

		/// <summary>
		/// Sets the rows selected.
		/// </summary>
		/// <returns><c>true</c>, if rows selected was set, <c>false</c> otherwise.</returns>
		/// <param name="rowIndexes">Row indexes.</param>
		bool SetRowsSelected (int[] rowIndexes);

		/// <summary>
		/// Gets the index of the column.
		/// </summary>
		/// <returns>The column index.</returns>
		/// <param name="columnName">Column name.</param>
		int GetColumnIndex(string columnName);
	}
}

