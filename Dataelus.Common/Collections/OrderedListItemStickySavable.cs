using System;

namespace Dataelus.Collections
{
	public class OrderedListItemStickySavable : OrderedListItemSticky<KeyedValueCollection>
	{
		public OrderedListItemStickySavable ()
		{
		}

		public OrderedListItemStickySavable (KeyedValueCollection value, int orderIndex)
			: base (value, orderIndex)
		{
		}
	}
}

