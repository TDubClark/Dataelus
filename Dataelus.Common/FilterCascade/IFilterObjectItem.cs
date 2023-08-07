using System;

namespace Dataelus.FilterCascade
{
	public interface IFilterObjectItem : IFilterItem
	{
		/// <summary>
		/// Gets or sets the name of the column used for display to the user.
		/// </summary>
		/// <value>The display name of the column.</value>
		string DisplayColumnName{ get; set; }

		/// <summary>
		/// Gets or sets the method by which the display text is determined.
		/// </summary>
		/// <value>The display by.</value>
		ValueDisplay DisplayBy{ get; set; }

/*		/// <summary>
		/// Gets or sets the selected values.
		/// </summary>
		/// <value>The selected values.</value>
		List<string> SelectedValues { get; set; }
*/
		/// <summary>
		/// Gets or sets a value indicating whether the user has selected an option for "Include All".
		/// </summary>
		/// <value><c>true</c> if this instance is selected all; otherwise, <c>false</c>.</value>
		bool IsSelectedAll{ get; set; }

		/// <summary>
		/// Gets or sets whether the user is allowed to select an option for "Include All".
		/// </summary>
		/// <value><c>true</c> if allow select all; otherwise, <c>false</c>.</value>
		bool AllowSelectAll{ get; set; }

		/// <summary>
		/// Gets or sets the minimum selectable items.
		/// </summary>
		/// <value>The minimum select.</value>
		int MinSelect{ get; set; }

		/// <summary>
		/// Gets or sets the maximum selectable items (set to -1 for "unlimited").
		/// </summary>
		/// <value>The max select.</value>
		int MaxSelect{ get; set; }

/*		/// <summary>
		/// Copies from the given object.
		/// </summary>
		/// <param name="other">Other.</param>
		void CopyFrom (IFilterTextItem other);
*/	}


	/// <summary>
	/// How values are display to the user.
	/// </summary>
	public enum ValueDisplay
	{
		/// <summary>
		/// Values are displayed as text from the value column.
		/// </summary>
		ValueColumn,

		/// <summary>
		/// Values are displayed as text from another column.
		/// </summary>
		OtherColumn,

		/// <summary>
		/// Values are displayed as text build from other values in the same row.
		/// </summary>
		Function
	}
}

