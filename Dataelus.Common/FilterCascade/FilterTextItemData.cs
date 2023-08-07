using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Filter text item, intended for serializing and storing data.
	/// </summary>
	public class FilterTextItemData : FilterObjectItemData, IFilterTextItem
	{
		/// <summary>
		/// Gets or sets the selected values.
		/// </summary>
		/// <value>The selected values.</value>
		public List<string> SelectedValues { get; set; }

		/// <summary>
		/// Copies from the given object.
		/// </summary>
		/// <param name="other">Other.</param>
		public void CopyFrom (IFilterTextItem other)
		{
			CopyTo (this, other);
		}

		/// <summary>
		/// Copies to the target from the source.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="source">Source.</param>
		public static void CopyTo (IFilterTextItem target, IFilterTextItem source)
		{
			FilterObjectItemData.CopyTo (target, source);
			target.SelectedValues = new List<string> (source.SelectedValues);
		}

		public FilterTextItemData ()
		{
		}
	}
}

