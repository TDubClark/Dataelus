using System;

namespace Dataelus.FilterCascade
{
	public class FilterDataNumeric : FilterDataBase
	{
		public double SelectedLowerBound{ get; set; }

		public double SelectedUpperBound{ get; set; }

		public double MinValue{ get; set; }

		public double MaxValue{ get; set; }

		public FilterDataNumeric ()
			: base ()
		{
		}
	}
}

