using System;

namespace Dataelus.Collections
{
	/// <summary>
	/// Interface for an ordered list item.
	/// </summary>
	public interface IOrderedListItem
	{
		/// <summary>
		/// Gets or sets the order index.
		/// </summary>
		/// <value>The order index.</value>
		int OrderIndex { get; set; }
	}
}

