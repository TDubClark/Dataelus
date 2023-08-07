using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// Base Implementation of ICollection(T)
	/// </summary>
	public class CollectionBase<T> : ICollectionSearchable<T>, IClonable<CollectionBase<T>>
	{
		/// <summary>
		/// The items.
		/// </summary>
		protected List<T> _items;

		/// <summary>
		/// Gets or sets the items.
		/// </summary>
		/// <value>The items.</value>
		public List<T> Items { get { return _items; } set { _items = value; } }

		/// <summary>
		/// Occurs when an item in the list is set to a value.
		/// </summary>
		public event ListItemSetEventHandler<T> ItemSet;

		/// <summary>
		/// Occurs when an item is added to the list.
		/// </summary>
		public event ListItemAddedEventHandler<T> ItemAdded;

		/// <summary>
		/// Occurs when an item is removed from the list.
		/// </summary>
		public event ListItemRemovedEventHandler<T> ItemRemoved;

		/// <summary>
		/// Occurs when the list is cleared.
		/// </summary>
		public event ListClearedEventHandler<T> ListCleared;

		/// <summary>
		/// Raises the item set event.
		/// </summary>
		/// <param name="oldValue">Old value.</param>
		/// <param name="index">Index.</param>
		/// <param name="newValue">New value.</param>
		protected virtual void OnItemSet (T oldValue, int index, T newValue)
		{
			if (ItemSet != null) {
				ItemSet (this, new ListItemSetEventArgs<T> (newValue, oldValue, index));
			}
		}

		/// <summary>
		/// Raises the item added event.
		/// </summary>
		/// <param name="newValue">New value.</param>
		/// <param name="index">Index.</param>
		protected virtual void OnItemAdded (T newValue, int index)
		{
			if (ItemAdded != null) {
				ItemAdded (this, new ListItemAddedEventArgs<T> (newValue, index));
			}
		}

		/// <summary>
		/// Raises the item removed event.
		/// </summary>
		/// <param name="oldValue">Old value.</param>
		/// <param name="index">Index.</param>
		protected virtual void OnItemRemoved (T oldValue, int index)
		{
			if (ItemRemoved != null) {
				ItemRemoved (this, new ListItemRemovedEventArgs<T> (oldValue, index));
			}
		}

		/// <summary>
		/// Raises the list cleared event.
		/// </summary>
		protected virtual void OnListCleared ()
		{
			if (ListCleared != null) {
				ListCleared (this, new ListClearedEventArgs<T> (_items));
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="Dataelus.CollectionBase`1"/> at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		public virtual T this [int index] {
			get { return _items [index]; }
			set {
				OnItemSet (_items [index], index, value);
				_items [index] = value;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.CollectionBase`1"/> class.
		/// </summary>
		public CollectionBase ()
		{
			_items = new List<T> ();
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.CollectionBase`1"/> class.
		/// </summary>
		/// <param name="items">The initial collection of items.</param>
		public CollectionBase (IEnumerable<T> items)
			: this ()
		{
			AddItems (items);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.CollectionBase`1"/> class.
		/// </summary>
		/// <param name="items">The initial items.</param>
		public CollectionBase (params T[] items)
			: this (items.ToList ())
		{
		}

		/// <summary>
		/// Contains the specified item and comparer.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="comparer">Comparer.</param>
		public virtual bool Contains (T item, IEqualityComparer<T> comparer)
		{
			return (_items.FindIndex (x => comparer.Equals (x, item)) >= 0);
		}

		/// <summary>
		/// Finds the index all.
		/// </summary>
		/// <returns>The index all.</returns>
		/// <param name="predicate">Predicate.</param>
		public virtual int[] FindIndexAll (Predicate<T> predicate)
		{
			var indexes = new System.Collections.Generic.List<int> ();
			for (int i = 0; i < _items.Count; i++) {
				if (predicate (_items [i]))
					indexes.Add (i);
			}
			return indexes.ToArray ();
		}

		/// <summary>
		/// Tries to get the item which matches the given predicate.
		/// </summary>
		/// <returns><c>true</c>, if get item was found, <c>false</c> otherwise.</returns>
		/// <param name="match">Match.</param>
		/// <param name="item">Item.</param>
		public virtual bool TryGetItem (Predicate<T> match, out T item)
		{
			return TryGetItem (match, out item, default(T));
		}

		/// <summary>
		/// Tries to get the item which matches the given predicate.
		/// </summary>
		/// <returns><c>true</c>, if get item was found, <c>false</c> otherwise.</returns>
		/// <param name="match">The item match criteria.</param>
		/// <param name="item">Item.</param>
		/// <param name="defaultValue">Default value.</param>
		public virtual bool TryGetItem (Predicate<T> match, out T item, T defaultValue)
		{
			int index = _items.FindIndex (match);
			if (index < 0) {
				item = defaultValue;
				return false;
			}
			item = _items [index];
			return true;
		}

		/// <summary>
		/// Tries to get the value of the item which matches the given predicate.
		/// </summary>
		/// <returns><c>true</c>, if get item was found, <c>false</c> otherwise.</returns>
		/// <param name="match">The item match criteria.</param>
		/// <param name="valueGetter">The function for getting the appropriate value</param>
		/// <param name="value">The value.</param>
		/// <param name="defaultValue">Default value.</param>
		public virtual bool TryGetItemValue<V> (Predicate<T> match, Func<T, V> valueGetter, out V value, V defaultValue)
		{
			int index = _items.FindIndex (match);
			if (index < 0) {
				value = defaultValue;
				return false;
			}
			value = valueGetter (_items [index]);
			return true;
		}

		/// <summary>
		/// Finds the index of the given item.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="item">Item.</param>
		/// <param name="comparer">Comparer.</param>
		public virtual int FindIndex (T item, IEqualityComparer<T> comparer)
		{
			return _items.FindIndex (x => comparer.Equals (x, item));
		}

		/// <summary>
		/// Finds the index.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="match">Match.</param>
		public virtual int FindIndex (Predicate<T> match)
		{
			return _items.FindIndex (match);
		}

		/// <summary>
		/// Find the specified match.
		/// </summary>
		/// <param name="match">Match.</param>
		public virtual T Find (Predicate<T> match)
		{
			return _items.Find (match);
		}

		/// <summary>
		/// Applies the given action on each item
		/// </summary>
		/// <param name="action">Action.</param>
		public virtual void ForEach (Action<T> action)
		{
			foreach (var item in _items) {
				action (item);
			}
		}

		/// <summary>
		/// Applies the given action on each item which matches the given predicate
		/// </summary>
		/// <param name="action">Action.</param>
		/// <param name="match">The filter; determines which items on which to perform the Action.</param>
		public virtual void ForEach (Action<T> action, Predicate<T> match)
		{
			foreach (var item in _items) {
				if (match (item))
					action (item);
			}
		}

		/// <summary>
		/// Adds the items.
		/// </summary>
		/// <param name="items">Items.</param>
		public virtual void AddItems (IEnumerable<T> items)
		{
			foreach (var item in items) {
				Add (item);
			}
		}

		/// <summary>
		/// Removes each of the given items in the given list (does not check for duplicates).
		/// </summary>
		/// <param name="items">The list of items to remove.</param>
		/// <param name="comparer">The Equality Comparer for the items.</param>
		public virtual void RemoveItems (IEnumerable<T> items, IEqualityComparer<T> comparer)
		{
			foreach (var item in items) {
				int index = FindIndex (item, comparer);
				if (index < 0)
					continue;
				RemoveAt (index);
			}
		}

		/// <summary>
		/// Clones the list, using the specified object cloner.
		/// </summary>
		/// <param name="cloner">An object which clones individual items.</param>
		public virtual CollectionBase<T> Clone (IObjectCloner<T> cloner)
		{
			return Clone (cloner.GetClone);
		}

		/// <summary>
		/// Clones the list, using the specified object cloner.
		/// </summary>
		/// <param name="cloner">Function which clones individual items.</param>
		public virtual CollectionBase<T> Clone (Func<T, T> cloner)
		{
			var items = new List<T> ();
			foreach (var item in _items) {
				items.Add (cloner (item));
			}
			return new CollectionBase<T> (items);
		}

		/// <summary>
		/// Sort the specified comparer.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		public void Sort (IComparer<T> comparer)
		{
			_items.Sort (comparer);
		}

		/// <summary>
		/// Removes at index.
		/// </summary>
		/// <param name="index">Index.</param>
		public virtual void RemoveAt (int index)
		{
			OnItemRemoved (_items [index], index);
			_items.RemoveAt (index);
		}

		/// <summary>
		/// Insert the specified index and item.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		public virtual void Insert (int index, T item)
		{
			OnItemAdded (item, index);
			_items.Insert (index, item);
		}

		/// <summary>
		/// Moves the item from the given index to the given index.
		/// </summary>
		/// <returns><c>true</c>, if item was moved, <c>false</c> otherwise.</returns>
		/// <param name="fromIndex">From index.</param>
		/// <param name="toIndex">To index.</param>
		public virtual void MoveItem (int fromIndex, int toIndex)
		{
			var item = _items [fromIndex];
			RemoveAt (fromIndex);
			Insert (toIndex, item);
		}

		public override string ToString ()
		{
			return string.Format ("Count = {0}", Count);
		}

		#region IClonable implementation

		/// <summary>
		/// Clone this instance.
		/// </summary>
		public CollectionBase<T> Clone ()
		{
			return Clone (new ObjectClonerDefault<T> ());
		}

		#endregion

		#region ICollection implementation

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void Add (T item)
		{
			OnItemAdded (item, _items.Count);
			_items.Add (item);
		}

		/// <summary>
		/// Clear this instance.
		/// </summary>
		public virtual void Clear ()
		{
			OnListCleared ();
			_items.Clear ();
		}

		/// <Docs>The object to locate in the current collection.</Docs>
		/// <para>Determines whether the current collection contains a specific value.</para>
		/// <summary>
		/// Contains the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual bool Contains (T item)
		{
			return _items.Contains (item);
		}

		/// <summary>
		/// Copies to the given array.
		/// </summary>
		/// <param name="array">Array.</param>
		/// <param name="arrayIndex">Array index.</param>
		public virtual void CopyTo (T[] array, int arrayIndex)
		{
			_items.CopyTo (array, arrayIndex);
		}

		/// <Docs>The item to remove from the current collection.</Docs>
		/// <para>Removes the first occurrence of an item from the current collection.</para>
		/// <summary>
		/// Remove the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual bool Remove (T item)
		{
			OnItemRemoved (item, _items.FindIndex (x => x.Equals (item)));
			return _items.Remove (item);
		}

		/// <summary>
		/// Gets the count.
		/// </summary>
		/// <value>The count.</value>
		public virtual int Count {
			get { return _items.Count; }
		}

		/// <summary>
		/// Gets a value indicating whether this instance is read only.
		/// </summary>
		/// <value><c>true</c> if this instance is read only; otherwise, <c>false</c>.</value>
		public virtual bool IsReadOnly {
			get { return false; }
		}

		#endregion

		#region IEnumerable implementation

		/// <summary>
		/// Gets the enumerator.
		/// </summary>
		/// <returns>The enumerator.</returns>
		public virtual IEnumerator<T> GetEnumerator ()
		{
			return _items.GetEnumerator ();
		}

		#endregion

		#region IEnumerable implementation

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator ()
		{
			return GetEnumerator ();
		}

		#endregion
	}

	/// <summary>
	/// List item set event arguments.
	/// </summary>
	public class ListItemSetEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets or sets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public T NewValue { get; set; }

		/// <summary>
		/// Gets or sets the prior value.
		/// </summary>
		/// <value>The prior value.</value>
		public T PriorValue { get; set; }

		/// <summary>
		/// Gets or sets the index of the item set.
		/// </summary>
		/// <value>The index set.</value>
		public int IndexSet { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListItemSetEventArgs`1"/> class.
		/// </summary>
		public ListItemSetEventArgs ()
			: base ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListItemSetEventArgs`1"/> class.
		/// </summary>
		/// <param name="newValue">New value.</param>
		/// <param name="oldValue">Old value.</param>
		/// <param name="index">Index.</param>
		public ListItemSetEventArgs (T newValue, T oldValue, int index)
			: this ()
		{
			this.NewValue = newValue;
			this.PriorValue = oldValue;
			this.IndexSet = index;
		}
	}

	/// <summary>
	/// List item set event handler.
	/// </summary>
	public delegate void ListItemSetEventHandler<T> (object sender, ListItemSetEventArgs<T> args);

	/// <summary>
	/// List item removed event arguments.
	/// </summary>
	public class ListItemRemovedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets or sets the prior value.
		/// </summary>
		/// <value>The prior value.</value>
		public T PriorValue { get; set; }

		/// <summary>
		/// Gets or sets the index of the item removed.
		/// </summary>
		/// <value>The index set.</value>
		public int Index { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListItemRemovedEventArgs`1"/> class.
		/// </summary>
		public ListItemRemovedEventArgs ()
			: base ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListItemRemovedEventArgs`1"/> class.
		/// </summary>
		/// <param name="oldValue">Old value.</param>
		/// <param name="index">Index.</param>
		public ListItemRemovedEventArgs (T oldValue, int index)
			: this ()
		{
			this.PriorValue = oldValue;
			this.Index = index;
		}
	}

	/// <summary>
	/// List item removed event handler.
	/// </summary>
	public delegate void ListItemRemovedEventHandler<T> (object sender, ListItemRemovedEventArgs<T> args);

	/// <summary>
	/// List item added event arguments.
	/// </summary>
	public class ListItemAddedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets or sets the new value.
		/// </summary>
		/// <value>The new value.</value>
		public T NewValue { get; set; }

		/// <summary>
		/// Gets or sets the index of the item added.
		/// </summary>
		/// <value>The index set.</value>
		public int Index { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListItemAddedEventArgs`1"/> class.
		/// </summary>
		public ListItemAddedEventArgs ()
			: base ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.ListItemAddedEventArgs`1"/> class.
		/// </summary>
		/// <param name="newValue">New value.</param>
		/// <param name="index">Index.</param>
		public ListItemAddedEventArgs (T newValue, int index)
			: this ()
		{
			this.NewValue = newValue;
			this.Index = index;
		}
	}

	/// <summary>
	/// List item added event handler.
	/// </summary>
	public delegate void ListItemAddedEventHandler<T> (object sender, ListItemAddedEventArgs<T> args);

	/// <summary>
	/// List cleared event arguments.
	/// </summary>
	public class ListClearedEventArgs<T> : EventArgs
	{
		/// <summary>
		/// Gets or sets the list items.
		/// </summary>
		/// <value>The list items.</value>
		public List<T> ListItems { get; set; }

		public ListClearedEventArgs ()
			: base ()
		{
		}

		public ListClearedEventArgs (List<T> listItems)
		{
			this.ListItems = listItems;
		}
	}

	/// <summary>
	/// List cleared event handler.
	/// </summary>
	public delegate void ListClearedEventHandler<T> (object sender, ListClearedEventArgs<T> args);
}

