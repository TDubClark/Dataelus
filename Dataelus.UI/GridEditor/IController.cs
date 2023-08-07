using System;
using System.Collections.Generic;

namespace Dataelus.UI.GridEditor
{
	/// <summary>
	/// Interface for a controller, for editing a grid data object of the given type.
	/// </summary>
	public interface IController<D>
	{
		/// <summary>
		/// Gets or sets the saver for the grid data object.
		/// </summary>
		/// <value>The saver object.</value>
		IDataSaver<D> Saver { get; set; }

		/// <summary>
		/// Gets or sets the validator for the grid data object.
		/// </summary>
		/// <value>The validator object.</value>
		IValidator<D> Validator { get; set; }

		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		IView<D> ViewObject { get; set; }

		/// <summary>
		/// Gets or sets the grid data object.
		/// </summary>
		/// <value>The grid data object.</value>
		D GridDataObject { get; set; }

		/// <summary>
		/// Gets or sets the view object for the Grid Issue Viewer.
		/// </summary>
		/// <value><c>true</c> if this instance issue view object; otherwise, <c>false</c>.</value>
		GridItemViewer.Generic.IView<D> IssueViewer { get; set; }

		/// <summary>
		/// Loads the view.
		/// </summary>
		void LoadView ();

		/// <summary>
		/// To be run the after deserialization.
		/// </summary>
		void RunAfterDeserialization ();


		/// <summary>
		/// Moves the row.
		/// </summary>
		/// <param name="rowIndex">The index of the row to be moved.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="magnitude">Magnitude.</param>
		void MoveRow (int rowIndex, VerticalDirection direction, int magnitude);

		/// <summary>
		/// Moves the column.
		/// </summary>
		/// <param name="columnName">The name of the column to be moved.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="magnitude">Magnitude.</param>
		void MoveColumn (string columnName, HorizontalDirection direction, int magnitude);

		/// <summary>
		/// Moves the column.
		/// </summary>
		/// <param name="columnIndex">The index of the column to be moved.</param>
		/// <param name="direction">Direction.</param>
		/// <param name="magnitude">Magnitude.</param>
		void MoveColumn (int columnIndex, HorizontalDirection direction, int magnitude);

		/// <summary>
		/// Called when the value of the given row index is changed, for the given column name.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="newValue">New value of the given cell.</param>
		void ValueChanged (int rowIndex, string columnName, string newValue);

		/// <summary>
		/// Called when the value of the given row index is changed, for the given column name.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="newValue">New value of the given cell.</param>
		void ValueChanged (int rowIndex, string columnName, object newValue);

		/// <summary>
		/// Saves the data.
		/// </summary>
		void SaveData ();

		/// <summary>
		/// Append a row to the end.
		/// </summary>
		void AppendRow ();

		/// <summary>
		/// Insert a row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		void InsertRow (int rowIndex);

		/// <summary>
		/// Delete the row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		void DeleteRow (int rowIndex);


		/// <summary>
		/// Validates the row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		void ValidateRow (int rowIndex);

		/// <summary>
		/// Validates the grid.
		/// </summary>
		void ValidateGrid ();

		/// <summary>
		/// Validates the grid, loading the issues into the given Issue view
		/// </summary>
		void ValidateGrid (GridItemViewer.Generic.IView<D> gridItemViewer);
	}
}

