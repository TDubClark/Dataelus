using System;

namespace Dataelus.FilterCascade.UI
{
	public interface IFilterCascadeView
	{
		/// <summary>
		/// Loads the non-text filters.
		/// </summary>
		void LoadNonTextFilters (INonTextFilter[] filters);

		/// <summary>
		/// Loads the filters.
		/// </summary>
		/// <param name="filters">Filters.</param>
		/// <param name="tables">Tables.</param>
		void LoadFilters (FilterTextItemCollection filters, FilterTextTableSqlHierarchy tables);

		/// <summary>
		/// Updates the given filters.
		/// </summary>
		/// <param name="filters">Filters.</param>
		void UpdateFilters (FilterTextItemCollection filters);
	}
}

