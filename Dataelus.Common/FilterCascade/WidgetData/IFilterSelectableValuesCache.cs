using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade.WidgetData
{
	/* This is in progress:
	 *  - need to replace the FilterTextItemCollection and the FilterTextTable with object-based classes
	 */

	/// <summary>
	/// Interface for a selectable values cache for a filter widget (such as a drop-down box).
	/// </summary>
	public interface IFilterSelectableValuesCache
	{
		/// <summary>
		/// Gets or sets the filter data.
		/// It is a dictionary of filter codes/filter data; the filter data is itself a dictionary of value/display.
		/// </summary>
		/// <value>The filter data.</value>
		Dictionary<string, Dictionary<object, string>> FilterData{ get; set; }

		/// <summary>
		/// Load the cache using the specified filters and table.
		/// </summary>
		/// <param name="filters">Filters.</param>
		/// <param name="table">Table.</param>
		void Load (FilterValueItemCollection filters, FilterObjectTable table);

		/// <summary>
		/// Updates the cache for all child-filters of the specified parentFilterCode.
		/// This should be called when the user changes the selection on filter widget: parentFilterCode = filter code for changed widget.
		/// </summary>
		/// <param name="parentFilterCode">Filter code of the parent filter.</param>
		/// <param name="filters">Filters.</param>
		/// <param name="table">Table.</param>
		void UpdateCache (string parentFilterCode, FilterValueItemCollection filters, FilterObjectTable table);

		/// <summary>
		/// Gets the widget data.
		/// Dictionary.Key = Widget item value, Dictionary.Value = Widget item display text
		/// </summary>
		/// <returns>The widget data.</returns>
		/// <param name="filterCode">Filter code.</param>
		Dictionary<object, string> GetWidgetData (string filterCode);

		/// <summary>
		/// Gets the widget data values.
		/// </summary>
		/// <returns>The widget data values.</returns>
		object[] GetWidgetDataValues (string filterCode);

		/// <summary>
		/// Gets the widget data display text.
		/// </summary>
		/// <returns>The widget data display text.</returns>
		string[] GetWidgetDataDisplayText (string filterCode);
	}
}

