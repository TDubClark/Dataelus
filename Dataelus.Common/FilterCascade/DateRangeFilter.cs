using System;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// A Date range filter.
	/// </summary>
	public class DateRangeFilter : NonTextFilter
	{
		/// <summary>
		/// Gets or sets the start date.
		/// </summary>
		/// <value>The start date.</value>
		public DateTime StartDate{ get; set; }

		/// <summary>
		/// Gets or sets the end date.
		/// </summary>
		/// <value>The end date.</value>
		public DateTime EndDate{ get; set; }

		public DateRangeFilter ()
			: base (Dataelus.Table.TypeClass.DateTime)
		{
		}
	}
}

