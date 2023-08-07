using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade
{
	public interface IFilterValueItem : IFilterObjectItem
	{
		/// <summary>
		/// Gets or sets the selected values.
		/// </summary>
		/// <value>The selected values.</value>
		List<object> SelectedValues { get; set; }

		/// <summary>
		/// Copies from the given object.
		/// </summary>
		/// <param name="other">Other.</param>
		void CopyFrom (IFilterValueItem other);
	}
}

