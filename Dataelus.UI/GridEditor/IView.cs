using System;
using System.Collections.Generic;

namespace Dataelus.UI.GridEditor
{
	/// <summary>
	/// Interface for a view class.
	/// </summary>
	public interface IView<D>
	{
		/// <summary>
		/// Loads the form.
		/// </summary>
		/// <param name="fields">Fields.</param>
		/// <param name="filterValues">Values.</param>
		/// <param name="gridData">The Grid Data object</param>
		void LoadForm (Dataelus.EDD.EDDFieldCollection fields, FilterCascade.WidgetData.FilterSelectableTextValuesCache filterValues, D gridData);

		/// <summary>
		/// Loads the form.
		/// </summary>
		/// <param name="columns">Columns.</param>
		/// <param name="columnDisplay">Column display.</param>
		/// <param name="filterValues">Filter values.</param>
		/// <param name="gridData">The Grid Data object</param>
		void LoadForm (Dataelus.Table.ObjectColumnCollection columns
			, Dictionary<string, string> columnDisplay
			, Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache filterValues
			, D gridData);

		/// <summary>
		/// Loads the form.
		/// </summary>
		/// <param name="columns">Columns.</param>
		/// <param name="columnDisplay">Column display.</param>
		/// <param name="filterValues">Filter values.</param>
		/// <param name="suggestionValues">Suggested Values for given fields.</param>
		/// <param name="gridData">The Grid Data object</param>
		void LoadForm (Dataelus.Table.ObjectColumnCollection columns
			, Dictionary<string, string> columnDisplay
			, Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache filterValues
			, Dataelus.FilterCascade.WidgetData.TextValuesCache suggestionValues
			, D gridData);

		/// <summary>
		/// Loads the form row.
		/// </summary>
		/// <param name="columns">Columns.</param>
		/// <param name="columnDisplay">Column display.</param>
		/// <param name="filterValues">Filter values.</param>
		/// <param name="gridData">Grid data.</param>
		/// <param name="rowIndex">Row index.</param>
		void LoadFormRow (Dataelus.Table.ObjectColumnCollection columns
			, Dictionary<string, string> columnDisplay
			, Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache filterValues
			, D gridData
			, int rowIndex);

		/// <summary>
		/// Reloads the filters for the given row, for the given column names.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnNames">Column names.</param>
		/// <param name="filterValues">Filter values.</param>
		void ReloadRowFilters (int rowIndex, string[] columnNames, FilterCascade.WidgetData.FilterSelectableTextValuesCache filterValues);

		/// <summary>
		/// Unloads the form data to the given grid data object.
		/// </summary>
		/// <param name="fields">Fields</param>
		/// <param name="gridData">Grid data.</param>
		void UnloadFormData (EDD.EDDFieldCollection fields, D gridData);

		/// <summary>
		/// Unloads the form data for the given row to the given grid data object.
		/// </summary>
		/// <param name="fields">Fields</param>
		/// <param name="gridData">Grid data.</param>
		/// <param name = "rowIndex">Row index.</param>
		void UnloadFormDataRow (EDD.EDDFieldCollection fields, D gridData, int rowIndex);

		/// <summary>
		/// Updates the form data for the given cell.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnName">Column name.</param>
		/// <param name="value">Value.</param>
		void UpdateFormDataCell (int rowIndex, string columnName, object value);

		/// <summary>
		/// Inserts a row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row Index.</param>
		void InsertRow (int rowIndex);

		/// <summary>
		/// Deletes the row at the given index.
		/// </summary>
		/// <param name="rowIndex">Row Index.</param>
		void DeleteRow (int rowIndex);


		/// <summary>
		/// Moves the row.
		/// </summary>
		/// <param name = "rowIndex"></param>
		/// <param name="direction">Direction.</param>
		/// <param name="magnitude">Magnitude.</param>
		void MoveRow (int rowIndex, VerticalDirection direction, int magnitude);

		/// <summary>
		/// Moves the column.
		/// </summary>
		/// <param name = "columnName"></param>
		/// <param name="direction">Direction.</param>
		/// <param name="magnitude">Magnitude.</param>
		void MoveColumn (string columnName, HorizontalDirection direction, int magnitude);

		/// <summary>
		/// Shows the validity of the given row.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="validation">Validation.</param>
		void ShowValidityRow (int rowIndex, ValidationResult validation);

		/// <summary>
		/// Shows the validity of the entire grid.
		/// </summary>
		/// <param name="validation">Validation.</param>
		void ShowValidityGrid (ValidationResult validation);
	}
}

