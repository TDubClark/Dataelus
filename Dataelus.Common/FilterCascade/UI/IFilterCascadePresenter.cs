using System;
using System.Collections.Generic;

namespace Dataelus.FilterCascade.UI
{
	public interface IFilterCascadePresenter
	{
		/// <summary>
		/// Occurs when the user has changed the selection of the given filter.
		/// </summary>
		/// <param name="filterCode">Filter code.</param>
		void FilterSelectionChanged (string filterCode, List<string> selectedValues);

		/// <summary>
		/// The user has changed a non-text filter and applied it (by clicking "Apply").
		/// </summary>
		/// <param name="filterCode">Filter code.</param>
		void NonTextFilterApplied (string filterCode, INonTextFilter filter);
	}
}

