using System;

namespace Dataelus.FilterCascade
{
	public interface IFilterItem
	{
		/// <summary>
		/// Gets or sets the filter code.
		/// This is the unique Identifier of this filter.
		/// </summary>
		/// <value>The filter code.</value>
		string FilterCode{ get; set; }

		/// <summary>
		/// Gets or sets the filter codes for parent filters.
		/// </summary>
		/// <value>The parent filter codes.</value>
		string[] ParentFilterCodes{ get; set; }

		/// <summary>
		/// Gets or sets the name of the column.
		/// </summary>
		/// <value>The name of the column.</value>
		string ColumnName{ get; set; }
	}
}

