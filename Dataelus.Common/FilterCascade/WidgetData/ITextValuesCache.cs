using System.Collections.Generic;

namespace Dataelus.FilterCascade.WidgetData
{
	/// <summary>
	/// Interface for a cache of selectable text values for a widget.
	/// </summary>
	public interface ITextValuesCache
	{
		/// <summary>
		/// Gets or sets the suggestion data.
		/// It is a dictionary of filter codes/suggestion data; the suggestion data is itself a dictionary of value/display.
		/// </summary>
		/// <value>The filter data.</value>
		Dictionary<string, Dictionary<string, string>> SuggestionData{ get; set; }

		/// <summary>
		/// Gets the widget data.
		/// Dictionary.Key = Widget item value, Dictionary.Value = Widget item display text
		/// </summary>
		/// <returns>The widget data.</returns>
		/// <param name="filterCode">Filter code.</param>
		Dictionary<string, string> GetWidgetData (string filterCode);

		/// <summary>
		/// Gets the widget data values.
		/// </summary>
		/// <returns>The widget data values.</returns>
		string[] GetWidgetDataValues (string filterCode);

		/// <summary>
		/// Gets the widget data display text.
		/// </summary>
		/// <returns>The widget data display text.</returns>
		string[] GetWidgetDataDisplayText (string filterCode);
	}
}

