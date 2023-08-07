using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	public class IdentifiedSelectedOrderedList : IdentifiedList<SelectedOrderedListItem>
	{
		/// <summary>
		/// Add the specified item, with the given order and selection.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="order">Order.</param>
		/// <param name="selected">If set to <c>true</c> selected.</param>
		public void Add (object item, int order, bool selected)
		{
			base.Add (new SelectedOrderedListItem (item, order, selected));
		}

		/// <summary>
		/// Gets the list items.
		/// </summary>
		/// <returns>The list items.</returns>
		List<SelectedOrderedListItem> GetListItems ()
		{
			return _identifiedValues.Select (x => x.Value).ToList ();
		}

		/// <summary>
		/// Gets the selected items.
		/// </summary>
		/// <returns>The selected.</returns>
		public object[] GetSelected ()
		{
			return GetListItems ()
				.Where (x => x.Selected)
				.Select (x => x.Item)
				.ToArray ();
		}

		/// <summary>
		/// Sets whether the item (specified by the given ID) is selected.
		/// </summary>
		/// <param name="id">Identifier.</param>
		/// <param name="selected">If set to <c>true</c> selected.</param>
		public void Select (long id, bool selected)
		{
			SelectedOrderedListItem item;
			if (TryGetValue (id, out item)) {
				item.Selected = selected;
			}
		}

		/// <summary>
		/// Swaps the order of the two items, specified by the given IDs.
		/// </summary>
		/// <param name="itemID1">Item ID 1.</param>
		/// <param name="itemID2">Item ID 2.</param>
		public void SwapOrder (long itemID1, long itemID2)
		{
			SelectedOrderedListItem item1;
			SelectedOrderedListItem item2;

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
			var list = GetListItems ();

			// Sort
			list.Sort (new OrderedListItemComparer ());

			return list.Select (x => x.Item).ToList ();
		}

		/// <summary>
		/// Gets those items which are selected, ordered.
		/// </summary>
		/// <returns>The items ordered selected.</returns>
		public List<object> GetItemsOrderedSelected ()
		{
			var list = GetListItems ();

			// Sort
			list.Sort (new OrderedListItemComparer ());

			// Get the selected items
			return list
				.Where (x => x.Selected)
				.Select (x => x.Item)
				.ToList ();
		}

		public IdentifiedSelectedOrderedList ()
		{
		}
	}

	/// <summary>
	/// Selected ordered list item.
	/// </summary>
	public class SelectedOrderedListItem : OrderedListItem
	{
		public bool Selected { get; set; }

		public SelectedOrderedListItem (object item, int order, bool selected)
			: base (order, item)
		{
			this.Selected = selected;
		}
	}
}

