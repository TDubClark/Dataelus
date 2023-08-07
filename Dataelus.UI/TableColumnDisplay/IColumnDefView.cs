using System;

namespace Dataelus.UI.TableColumnDisplay
{
	using Dataelus.TableDisplay;

	public interface IColumnDefView
	{
		/// <summary>
		/// Gets or sets the Controller object.
		/// </summary>
		/// <value>The controller object.</value>
		IController ControllerObject { get; set; }

		/// <summary>
		/// Initializes the widgets.
		/// </summary>
		void InitializeWidgets (bool includeVisibleCheckbox);

		/// <summary>
		/// Loads the data into the form.
		/// </summary>
		/// <param name="items">Items.</param>
		void LoadData (System.Collections.Generic.IEnumerable<IColumnDef> items);

		/// <summary>
		/// Loads the data item.
		/// </summary>
		/// <param name="item">Item.</param>
		void LoadDataItem (IColumnDef item);

		/// <summary>
		/// Moves the column the given number of positions (over other columns) in the given direction.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		/// <param name="direction">Direction.</param>
		void MoveColumn (string columnName, MoveDirection direction, int positionMoveCount);

		/// <summary>
		/// Selects the column in the View.
		/// </summary>
		/// <param name="columnName">Column name.</param>
		void SelectColumn (string columnName);
	}
}

