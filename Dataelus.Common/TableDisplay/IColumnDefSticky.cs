using System;
using Dataelus.Collections;

namespace Dataelus.TableDisplay
{
	/// <summary>
	/// Interface for a column definition which supports Stickiness to a position or another column
	/// </summary>
	public interface IColumnDefSticky : IColumnDef, IOrderedListItemSticky
	{
		/// <summary>
		/// Gets or sets the name of the target column to which this column prefers to adhere (if applicable).
		/// </summary>
		/// <value>The name of the target column.</value>
		string targetColumnName { get; set; }

		/// <summary>
		/// Copies values from the other object.
		/// </summary>
		/// <param name="other">Other.</param>
		void copyFrom (IColumnDefSticky other);
	}
}

