using System;
using System.Collections.Generic;

namespace Dataelus
{
	/// <summary>
	/// List base.
	/// </summary>
	public class ListBase<T> : CollectionBase<T>, IList<T>, System.Collections.IEnumerable
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListBase`1"/> class.
		/// </summary>
		public ListBase ()
			: base ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListBase`1"/> class.
		/// </summary>
		/// <param name="collection">Collection.</param>
		public ListBase (IEnumerable<T> collection)
			: base (collection)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListBase`1"/> class.
		/// </summary>
		/// <param name="capacity">Capacity.</param>
		public ListBase (int capacity)
			: base ()
		{
			_items = new List<T> (capacity);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListBase`1"/> class.
		/// </summary>
		/// <param name="items">Items.</param>
		public ListBase (params T[] items)
			: base (items)
		{
		}

		/// <Docs>To be added.</Docs>
		/// <para>Determines the index of a specific item in the current instance.</para>
		/// <summary>
		/// Indexs the of.
		/// </summary>
		/// <returns>The of.</returns>
		/// <param name="item">Item.</param>
		public int IndexOf (T item)
		{
			return _items.IndexOf (item);
		}

		/// <summary>
		/// Gets or sets the <see cref="Dataelus.ListBase`1"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		T IList<T>.this [int index] {
			get {
				return _items [index];
			}
			set {
				OnItemSet (_items [index], index, value);
				_items [index] = value;
			}
		}

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return base.GetEnumerator ();
		}
	}
}

