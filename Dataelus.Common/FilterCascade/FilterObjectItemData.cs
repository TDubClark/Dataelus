using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade
{
	public class FilterObjectItemData : IFilterObjectItem
	{
		/// <summary>
		/// Gets or sets the filter code.
		/// </summary>
		/// <value>The filter code.</value>
		public string FilterCode{ get; set; }

		/// <summary>
		/// Gets or sets the filter codes for parent filters.
		/// </summary>
		/// <value>The parent filter codes.</value>
		public string[] ParentFilterCodes{ get; set; }

		/// <summary>
		/// Gets or sets the name of the column.
		/// </summary>
		/// <value>The name of the column.</value>
		public string ColumnName{ get; set; }

		/// <summary>
		/// Gets or sets the name of the column used for display to the user.
		/// </summary>
		/// <value>The display name of the column.</value>
		public string DisplayColumnName{ get; set; }

		/// <summary>
		/// Gets or sets the method by which the display text is determined.
		/// </summary>
		/// <value>The display by.</value>
		public ValueDisplay DisplayBy { get; set; }

		/// <summary>
		/// Gets or sets a value indicating whether the user has selected an option for "Include All".
		/// </summary>
		/// <value><c>true</c> if this instance is selected all; otherwise, <c>false</c>.</value>
		public bool IsSelectedAll{ get; set; }

		/// <summary>
		/// Gets or sets whether the user is allowed to select an option for "Include All".
		/// </summary>
		/// <value><c>true</c> if allow select all; otherwise, <c>false</c>.</value>
		public bool AllowSelectAll{ get; set; }

		/// <summary>
		/// Gets or sets the minimum selectable items.
		/// </summary>
		/// <value>The minimum select.</value>
		public int MinSelect{ get; set; }

		/// <summary>
		/// Gets or sets the maximum selectable items (set to -1 for "unlimited").
		/// </summary>
		/// <value>The max select.</value>
		public int MaxSelect{ get; set; }

		/// <summary>
		/// Copies from the given object.
		/// </summary>
		/// <param name="other">Other.</param>
		public void CopyFrom (IFilterObjectItem other)
		{
			CopyTo (this, other);
		}

		/// <summary>
		/// Copies to the target from the source.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="source">Source.</param>
		public static void CopyTo (IFilterObjectItem target, IFilterObjectItem source)
		{
			target.ColumnName = source.ColumnName;
			target.DisplayColumnName = source.DisplayColumnName;
			target.FilterCode = source.FilterCode;
			target.ParentFilterCodes = new List<string> (source.ParentFilterCodes).ToArray ();
			target.IsSelectedAll = source.IsSelectedAll;
			target.MinSelect = source.MinSelect;
			target.MaxSelect = source.MaxSelect;
			target.AllowSelectAll = source.AllowSelectAll;
		}

		public FilterObjectItemData ()
		{
		}
	}
}

