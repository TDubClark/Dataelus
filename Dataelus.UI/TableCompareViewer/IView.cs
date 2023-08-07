using System;

namespace Dataelus.UI.TableCompareViewer
{
	public interface IView<T, R>
	{
		/// <summary>
		/// Gets or sets the controller.
		/// </summary>
		/// <value>The controller.</value>
		IController<T, R> Controller{ get; set; }

		/// <summary>
		/// Hilights the items in the grid.
		/// </summary>
		/// <param name="gridNumber">The grid number (1 or 2).</param>
		/// <param name="rows">Row.</param>
		/// <param name="columns">Column.</param>
		void HilightGridItems (int gridNumber, int[] rows, int[] columns);

		/// <summary>
		/// Gets the index of the column with the given column name in the numbered grid.
		/// </summary>
		/// <returns>The column index.</returns>
		/// <param name="gridNumber">The grid number (1 or 2).</param>
		/// <param name="columnName">Column name.</param>
		int GetGridColumnIndex (int gridNumber, string columnName);

		/// <summary>
		/// Clears the hilighting.
		/// </summary>
		void ClearHilighting ();

		/// <summary>
		/// Loads the grid items.
		/// </summary>
		/// <param name="gridNumber">The grid number (1 or 2).</param>
		/// <param name="data">Data.</param>
		void LoadGridData (int gridNumber, T data);

		/// <summary>
		/// Loads the items.
		/// </summary>
		/// <param name="data">Data.</param>
		void LoadItems (System.Collections.Generic.IEnumerable<Dataelus.Table.Comparison.IssueItem> data);
	}
}

