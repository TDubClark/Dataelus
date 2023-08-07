using System;

namespace Dataelus.Collections
{
	/// <summary>
	/// Ordered list, savable to XML or JSON.
	/// </summary>
	public class OrderedListSavable : ListBase<OrderedListItemSavable>, System.Collections.IEnumerable
	{
		public OrderedListSavable ()
		{
		}

		public OrderedListSavable (System.Collections.Generic.IEnumerable<OrderedListItemSavable> collection) : base (collection)
		{
		}
	}
}

