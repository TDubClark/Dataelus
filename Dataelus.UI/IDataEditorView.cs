using System.Collections.Generic;

namespace Dataelus.UI
{
	public interface IDataEditorView : GridEditor.IView<Dataelus.Table.ObjectTable>
	{
		/// <summary>
		/// Gets or sets the controller object.
		/// </summary>
		/// <value>The controller object.</value>
		IDataEditorController ControllerObject { get; set; }

		/// <summary>
		/// Loads the form data.
		/// </summary>
		/// <param name="columns">Table Columns.</param>
		/// <param name="columnDisplay">Column display.</param>
		/// <param name="filterValues">Filter values.</param>
		/// <param name="table">Table.</param>
		void LoadFormData (Dataelus.Table.ObjectColumnCollection columns
			, Dictionary<string, string> columnDisplay
			, Dataelus.FilterCascade.WidgetData.FilterSelectableTextValuesCache filterValues
			, Dataelus.Table.ObjectTable table);
	}
}

