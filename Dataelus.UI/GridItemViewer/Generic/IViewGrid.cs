using System;

namespace Dataelus.UI.GridItemViewer.Generic
{
	public interface IViewGrid<D, I>
	{
		/// <summary>
		/// Gets or sets the controller.
		/// </summary>
		/// <value>The controller.</value>
		IController<D, I> Controller{ get; set; }

		/// <summary>
		/// Hilights the grid item.
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		void HilightGridItem (int row, int column);

		/// <summary>
		/// Gets the index of the column with the given column name.
		/// </summary>
		/// <returns>The column index.</returns>
		/// <param name="columnName">Column name.</param>
		int GetColumnIndex (string columnName);

		/// <summary>
		/// Clears the hilighting.
		/// </summary>
		void ClearHilighting ();

		/// <summary>
		/// Loads the grid items.
		/// </summary>
		/// <param name="data">Data.</param>
		void LoadGridData (D data);
	}

	public interface IViewGridViewOnly
	{
		/// <summary>
		/// Hilights the grid item.
		/// </summary>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		void HilightGridItem (int row, int column);

		/// <summary>
		/// Clears the hilighting.
		/// </summary>
		void ClearHilighting ();
	}

	public interface IViewPagedGridViewOnly
	{
		/// <summary>
		/// Hilights the grid item.
		/// </summary>
		/// <param name="page"></param>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		void HilightGridItem (int page, int row, int column);

		/// <summary>
		/// Clears the hilighting.
		/// </summary>
		void ClearHilighting ();
	}
}

