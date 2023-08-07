using System;
using System.Collections.Generic;

namespace Dataelus.UI.RecordEditor
{
	/// <summary>
	/// Interface for a single-record editor view.
	/// </summary>
	public interface IRecordEditorView
	{
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
			, Dataelus.Table.ObjectTable gridData
			, int rowIndex);

		/// <summary>
		/// Reloads the filters for the given row, for the given column names.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="columnNames">Column names.</param>
		/// <param name="filterValues">Filter values.</param>
		void ReloadRowFilters (int rowIndex, string[] columnNames, FilterCascade.WidgetData.FilterSelectableTextValuesCache filterValues);

		/// <summary>
		/// Shows the validity of the given row.
		/// </summary>
		/// <param name="rowIndex">Row index.</param>
		/// <param name="validation">Validation.</param>
		void ShowValidityRow (int rowIndex, Dataelus.UI.GridEditor.ValidationResult validation);

		/// <summary>
		/// Unloads the form data for the given row to the given grid data object.
		/// </summary>
		/// <param name="fields">Fields</param>
		/// <param name="gridData">Grid data.</param>
		/// <param name = "rowIndex">Row index.</param>
		void UnloadFormDataRow (Dataelus.Table.ObjectTable gridData, int rowIndex);

		/// <summary>
		/// Sets the field visiblility.
		/// </summary>
		/// <param name="columnNames">Column names.</param>
		/// <param name="visible">Whether the given fields should be visible.</param>
		void SetFieldsVisible(string[] columnNames, bool visible);

		/// <summary>
		/// Gets the value of the given column.
		/// </summary>
		/// <returns>The value.</returns>
		/// <param name="columnName">Column name.</param>
		object GetValue(string columnName);

		/// <summary>
		/// Determines whether this instance has a value for the specified columnName.
		/// </summary>
		/// <returns><c>true</c> if this instance has a value for the specified columnName; otherwise, <c>false</c>.</returns>
		/// <param name="columnName">Column name.</param>
		bool HasValue (string columnName);
	}
}

