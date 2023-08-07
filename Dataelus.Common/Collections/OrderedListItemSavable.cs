using System;

namespace Dataelus.Collections
{
	/// <summary>
	/// Ordered list item, which is savable to XML or JSON.
	/// </summary>
	public class OrderedListItemSavable : OrderedListItem<KeyedValueCollection>
	{
		public OrderedListItemSavable ()
		{
		}

		public OrderedListItemSavable (KeyedValueCollection value, int orderIndex)
			: base (value, orderIndex)
		{
		}
	}
}

