using System;

namespace Dataelus.UI.TableColumnDisplay
{
	using Dataelus.TableDisplay;

	public interface IController
	{
		/// <summary>
		/// Gets or sets the View object.
		/// </summary>
		/// <value>The view object.</value>
		IColumnDefView ViewObject{ get; set; }

		/// <summary>
		/// Gets or sets the Data object.
		/// </summary>
		/// <value>The data object.</value>
		ColumnDefCollection DataObject{ get; set; }

		/// <summary>
		/// Moves the column the given number of positions (over other columns) in the given direction.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="positionMoveCount">Number of positions to move the column (in the given direction).</param>
		void MoveColumn (string columnName, MoveDirection direction, int positionMoveCount);

		/// <summary>
		/// Moves the column the given number of positions (over other columns) in the given direction.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="positionMoveCount">Number of positions to move the column (in the given direction).</param>
		/// <param name="respectStickyPosition">Whether to respect preference of a column for a given absolute position.</param>
		/// <param name="respectColumnAdhesion">Whether to respect adhesion of one column to another.</param>
		void MoveColumn (string columnName, MoveDirection direction, int positionMoveCount, bool respectStickyPosition, bool respectColumnAdhesion);

		/// <summary>
		/// Changes the column visible property.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="isVisible">If set to <c>true</c> is visible.</param>
		void ChangeColumnVisible (string columnName, bool isVisible);

		/// <summary>
		/// Finds the index of the column.
		/// </summary>
		/// <returns>The column index.</returns>
		/// <param name="columnName">Column name.</param>
		int FindColumnIndex (string columnName);
	}

	/// <summary>
	/// Move direction for a column in a table.
	/// </summary>
	public enum MoveDirection
	{
		LeftMost,
		Left,
		Right,
		RightMost
	}
}

