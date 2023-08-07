using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	public class IdentifiedOrderedList : IdentifiedList<OrderedListItem>
	{
		public void Add (int order, object item)
		{
			base.Add (new OrderedListItem (order, item));
		}

		public void SwapOrder (long itemID1, long itemID2)
		{
			OrderedListItem item1;
			OrderedListItem item2;

			if (TryGetValue (itemID1, out item1)) {
				if (TryGetValue (itemID2, out item2)) {
					var tempOrder = item1.Order;
					item1.Order = item2.Order;
					item2.Order = tempOrder;
				}
			}
		}

		/// <summary>
		/// Gets the items, ordered.
		/// </summary>
		/// <returns>The items ordered.</returns>
		public List<object> GetItemsOrdered ()
		{
			// Sort
			var list = _identifiedValues.Select (x => x.Value).ToList ();
			list.Sort (new OrderedListItemComparer ());
			return list.Select (x => x.Item).ToList ();
		}

		public IdentifiedOrderedList ()
			: base ()
		{
		}
	}

	/// <summary>
	/// Ordered list item.
	/// </summary>
	public class OrderedListItem
	{
		public int Order { get; set; }

		public object Item { get; set; }

		public OrderedListItem ()
		{
			
		}

		public OrderedListItem (int order, object item)
		{
			this.Order = order;
			this.Item = item;
		}
	}

	class OrderedListItemComparer : IComparer<OrderedListItem>
	{
		#region IComparer implementation

		public int Compare (OrderedListItem x, OrderedListItem y)
		{
			return x.Order.CompareTo (y.Order);
		}

		#endregion
	}
}

