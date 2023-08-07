using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade
{
	public class FilterValueItemData : FilterObjectItemData, IFilterValueItem
	{
		/// <summary>
		/// Gets or sets the selected values.
		/// </summary>
		/// <value>The selected values.</value>
		public List<object> SelectedValues { get; set; }

		/// <summary>
		/// Copies from the given object.
		/// </summary>
		/// <param name="other">Other.</param>
		public void CopyFrom (IFilterValueItem other)
		{
			CopyTo (this, other);
		}

		/// <summary>
		/// Copies to the target from the source.
		/// </summary>
		/// <param name="target">Target.</param>
		/// <param name="source">Source.</param>
		public static void CopyTo (IFilterValueItem target, IFilterValueItem source)
		{
			FilterObjectItemData.CopyTo (target, source);
			target.SelectedValues = new List<object> (source.SelectedValues);
		}

		public FilterValueItemData ()
		{
		}
	}
}

