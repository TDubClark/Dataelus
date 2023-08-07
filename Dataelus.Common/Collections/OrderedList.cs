using System;
using System.Linq;

namespace Dataelus.Collections
{
	/// <summary>
	/// Ordered list.
	/// </summary>
	public class OrderedList<T> : ListBase<OrderedListItem<T>>
	{
		public OrderedList ()
		{
		}

		public virtual void Sort ()
		{
			_items.Sort (new OrderedListItemComparer<OrderedListItem<T>> ());
		}

		/// <summary>
		/// Merges the given list order into this list.
		/// </summary>
		/// <param name="other">The other list, which will be merged into this list.</param>
		public virtual void MergeListOrder (OrderedList<T> other, OrderedListMergeOptions options)
		{
			PrepMerge ();

			throw new NotImplementedException ();
		}

		/// <summary>
		/// Preps for the merge by assigning everything as Double-orecion points.
		/// </summary>
		protected virtual void PrepMerge ()
		{
			for (int i = 0; i < _items.Count; i++) {
				var item = new OrderedListItemSticky<T> (_items [i]);
				item.isSticky = true;
				item.stickinessObject = StickyTo.OtherObject;
				// Set the target item
				if (i == 0)
					item.targetItem = null;
				else
					item.targetItem = _items [i - 1];
				item.OrderIndex = -1;
				
				_items [i] = item;
			}
		}

		public virtual void Add (T newValue, int order)
		{
			Add (new OrderedListItem<T> (newValue, order));
		}

		/// <summary>
		/// Gets this list as a serializable list.
		/// </summary>
		/// <returns>The serializable.</returns>
		/// <param name="itemSaver">Item saver.</param>
		public OrderedListSavable ToSerializable (IValueSaver<T> itemSaver)
		{
//			var saved = new OrderedListSavable ();
//			foreach (var item in _items) {
//				saved.Add (new OrderedListItemSavable (itemSaver.GetKeyValues (item.Value), item.OrderIndex));
//			}
			return new OrderedListSavable (_items.Select (x => new OrderedListItemSavable (itemSaver.GetKeyValues (x.Value), x.OrderIndex)).ToArray ());
		}

		/// <summary>
		/// Adds in all the items from a serializable list.
		/// </summary>
		/// <param name="saved">Saved list.</param>
		/// <param name="itemReader">Item reader.</param>
		public void FromSerializable (OrderedListSavable saved, IValueReader<T> itemReader)
		{
			foreach (var item in saved) {
				Add (new OrderedListItem<T> (itemReader.GetItem (item.Value), item.OrderIndex));
			}
		}
	}

	public class OrderedListMergeOptions
	{
		
	}

	/// <summary>
	/// Interface for a value saver, which takes an item and returns the appropriate key-value pairs.
	/// </summary>
	public interface IValueSaver<T>
	{
		KeyedValueCollection GetKeyValues (T item);
	}

	/// <summary>
	/// Interface for a value reader, which takes a set of saved key-value pairs and returns the item.
	/// </summary>
	public interface IValueReader<T>
	{
		T GetItem (KeyedValueCollection keyValues);
	}
}

