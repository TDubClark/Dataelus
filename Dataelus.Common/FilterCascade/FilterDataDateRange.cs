using System;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// Date-range data filter.
	/// </summary>
	public class FilterDataDateRange : FilterDataBase
	{
		public DateTime SelectedStartDate{ get; set; }

		public DateTime SelectedEndDate{ get; set; }

		public DateTime MinDate{ get; set; }

		public DateTime MaxDate{ get; set; }

		public FilterDataDateRange ()
			: base ()
		{
		}
	}
}

