using System;
using System.Linq;

namespace Dataelus.Table
{
	/// <summary>
	/// Object row collection.
	/// </summary>
	public class ObjectRowCollection : CollectionBase<ObjectRow>, System.Collections.IEnumerable, IEquatable<ObjectRowCollection>
	{
		/// <summary>
		/// The identifier mgr.
		/// </summary>
		protected UniqueIdentifierManager _idMgr;

		/// <summary>
		/// Occurs when this row is changed.
		/// </summary>
		public event RowChangedEventHandler RowChanged;

		/// <summary>
		/// Raises the row changed event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void OnRowChanged (ObjectRowChangedEventArgs args)
		{
			if (RowChanged != null) {
				RowChanged (this, args);
			}
		}

		/// <summary>
		/// Occurs when (immediately before) a row is added.
		/// </summary>
		public event RowAddedEventHandler RowAdded;

		/// <summary>
		/// Raises the row added event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void OnRowAdded (ObjectRowAddedEventArgs args)
		{
			if (RowAdded != null) {
				RowAdded (this, args);
			}
		}

		/// <summary>
		/// Occurs when (immediately before) a row is deleted.
		/// </summary>
		public event RowDeletedEventHandler RowDeleted;

		/// <summary>
		/// Raises the row deleted event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		public void OnRowDeleted (ObjectRowDeletedEventArgs args)
		{
			if (RowDeleted != null) {
				RowDeleted (this, args);
			}
		}

		protected override void OnItemAdded (ObjectRow newValue, int index)
		{
			base.OnItemAdded (newValue, index);
			OnRowAdded (new ObjectRowAddedEventArgs (newValue, index));
		}

		protected override void OnItemRemoved (ObjectRow oldValue, int index)
		{
			base.OnItemRemoved (oldValue, index);
			OnRowDeleted (new ObjectRowDeletedEventArgs (oldValue));
		}

		public ObjectRowCollection ()
			: base ()
		{
			// Construct the ID manager
			_idMgr = new UniqueIdentifierManager ();
		}

		public ObjectRowCollection (ObjectTable parent, System.Collections.Generic.IEnumerable<ObjectRow> other)
			: this ()
		{
			foreach (var item in other) {
				AddNewRow (new ObjectRow (parent, item));
			}
		}

		/// <summary>
		/// Finds the index of the given item by the unique object ID.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="objectUniqueID">The unique object ID.</param>
		public virtual int FindIndex (long objectUniqueID)
		{
			return _items.FindIndex (x => x.ObjectUniqueID == objectUniqueID);
		}

		/// <summary>
		/// Finds the index by tag.
		/// </summary>
		/// <returns>The index by tag.</returns>
		/// <param name="customTag">Custom tag.</param>
		/// <param name="tagComparer">Tag comparer.</param>
		public virtual int FindIndexByTag (object customTag, System.Collections.IEqualityComparer tagComparer)
		{
			return _items.FindIndex (x => tagComparer.Equals (x.CustomTag, customTag));
		}

		/// <summary>
		/// Finds the index by tag.
		/// </summary>
		/// <returns>The index by tag.</returns>
		public virtual int FindIndexByTag (Predicate<object> tagPredicate)
		{
			return _items.FindIndex (x => tagPredicate (x.CustomTag));
		}

		/// <summary>
		/// Adds the new row, assigning a unique ID.
		/// </summary>
		/// <param name="item">Item.</param>
		protected void AddNewRow (ObjectRow item)
		{
			item.ObjectUniqueID = _idMgr.GetUniqueID ();
			item.RowChanged += Row_RowChanged;
			base.Add (item);
		}

		/// <summary>
		/// Handles the row changed.
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="args">Arguments.</param>
		void Row_RowChanged (object sender, ObjectRowChangedEventArgs args)
		{
			this.OnRowChanged (args);
		}

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public override void Add (ObjectRow item)
		{
			// Override this, by assigning a Unique ID first
			AddNewRow (item);
		}

		/// <summary>
		/// Insert the specified item at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		public override void Insert (int index, ObjectRow item)
		{
			item.ObjectUniqueID = _idMgr.GetUniqueID ();
			item.RowChanged += Row_RowChanged;
			base.Insert (index, item);
		}

		/// <summary>
		/// Adds the new item and returns a pointer.
		/// </summary>
		/// <returns>The return.</returns>
		/// <param name="item">Item.</param>
		public ObjectRow AddReturn (ObjectRow item)
		{
			Add (item);
			return item;
		}

		#region IEquatable implementation

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectRowCollection"/> is equal to the current <see cref="Dataelus.Table.ObjectRowCollection"/>.
		/// </summary>
		/// <param name="other">The <see cref="Dataelus.Table.ObjectRowCollection"/> to compare with the current <see cref="Dataelus.Table.ObjectRowCollection"/>.</param>
		/// <returns><c>true</c> if the specified <see cref="Dataelus.Table.ObjectRowCollection"/> is equal to the current
		/// <see cref="Dataelus.Table.ObjectRowCollection"/>; otherwise, <c>false</c>.</returns>
		public bool Equals (ObjectRowCollection other)
		{
			return Equals (other, true);
		}

		#endregion

		/// <summary>
		/// Determines whether the specified <see cref="Dataelus.Table.ObjectRowCollection"/> is equal to the current <see cref="Dataelus.Table.ObjectRowCollection"/>.
		/// </summary>
		/// <param name="other">Other.</param>
		/// <param name="assumeSameOrder">If set to <c>true</c> assume same order.</param>
		/// <param name="rowComparer">Row comparer.</param>
		public bool Equals (ObjectRowCollection other, bool assumeSameOrder, System.Collections.Generic.IEqualityComparer<ObjectRow> rowComparer)
		{
			if (other == null)
				throw new ArgumentNullException ("other");
			if (_items.Count != other.Count)
				return false;
			
			if (assumeSameOrder) {
				// Compare only the items at the same index
				for (int r = 0; r < _items.Count; r++) {
					if (!rowComparer.Equals (_items [r], other [r]))
						return false;
				}
			} else {
				// Search for each item
				for (int r = 0; r < _items.Count; r++) {
					if (other.FindIndex (_items [r], rowComparer) < 0)
						return false;
				}
			}
			return true;
		}
	}
}

