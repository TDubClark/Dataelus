using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Interface for a text-based filter item.
	/// </summary>
	public interface IFilterTextItem : IFilterObjectItem
	{
		/// <summary>
		/// Gets or sets the selected values.
		/// </summary>
		/// <value>The selected values.</value>
		List<string> SelectedValues { get; set; }

		/// <summary>
		/// Copies from the given object.
		/// </summary>
		/// <param name="other">Other.</param>
		void CopyFrom (IFilterTextItem other);
	}
}

