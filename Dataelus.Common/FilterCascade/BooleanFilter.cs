using System;

namespace Dataelus.FilterCascade
{
	/// <summary>
	/// A Boolean filter.
	/// </summary>
	public class BooleanFilter : NonTextFilter
	{
		/// <summary>
		/// Gets or sets whether the value "true" is selected to be within the filter.
		/// </summary>
		/// <value><c>true</c> if "true" is selected; otherwise, <c>false</c>.</value>
		public bool IsTrueSelected{ get; set; }

		/// <summary>
		/// Gets or sets whether the value "false" is selected to be within the filter.
		/// </summary>
		/// <value><c>true</c> if "false" is selected; otherwise, <c>false</c>.</value>
		public bool IsFalseSelected{ get; set; }

		public BooleanFilter ()
			: base (Dataelus.Table.TypeClass.Boolean)
		{
		}
	}
}

