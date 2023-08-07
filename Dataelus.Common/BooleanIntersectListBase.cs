using System;
using System.Collections.Generic;

using Dataelus.Extensions;

namespace Dataelus.Generic
{
	/// <summary>
	/// Base Boolean intersect list for items of type TItem.
	/// </summary>
	public abstract class BooleanIntersectListBase<TItem, TRow, TColumn> : IBooleanIntersectList<TRow, TColumn>
	{
		protected IList<TItem> _itemCollection;

		/// <summary>
		/// Gets or sets the item collection.
		/// </summary>
		/// <value>The item collection.</value>
		public IList<TItem> ItemCollection {
			get { return _itemCollection; }
			set { _itemCollection = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Generic.BooleanIntersectListBase`3"/> class.
		/// </summary>
		protected BooleanIntersectListBase ()
			: this (new List<TItem> ())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Generic.BooleanIntersectListBase`3"/> class.
		/// </summary>
		/// <param name="itemCollection">Item collection.</param>
		protected BooleanIntersectListBase (IList<TItem> itemCollection)
		{
			if (itemCollection == null)
				throw new ArgumentNullException ("itemCollection");
			this._itemCollection = itemCollection;
		}

		#region IBooleanIntersectList implementation

		public void AddNew (TRow row, TColumn column)
		{
			_itemCollection.Add (CreateNewItem (row, column));
		}

		public void Remove (TRow row, TColumn column)
		{
			int index = FindIndex (row, column);
			if (index < 0) {
				// This may get called even for intersections that are not in the list.  Don't throw an exception.
				//throw new IndexOutOfRangeException (String.Format ("Intersection list index not found for row {0}, column {1}.", row, column));
				return;
			}
			_itemCollection.RemoveAt (index);
		}

		public bool Contains (TRow row, TColumn column)
		{
			return (FindIndex (row, column) >= 0);
		}

		#endregion

		/// <summary>
		/// Creates the new item.
		/// </summary>
		/// <returns>The new item.</returns>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		protected abstract TItem CreateNewItem (TRow row, TColumn column);

		/// <summary>
		/// Finds the index.
		/// </summary>
		/// <returns>The index.</returns>
		/// <param name="row">Row.</param>
		/// <param name="column">Column.</param>
		protected abstract int FindIndex (TRow row, TColumn column);
	}

	/// <summary>
	/// Boolean intersect list for items of type TItem.
	/// </summary>
	public class BooleanIntersectList<TItem, TRow, TColumn> : BooleanIntersectListBase<TItem, TRow, TColumn>
	{
		protected Func<TRow, TColumn, TItem> _creator;

		/// <summary>
		/// Function for creating new items.
		/// </summary>
		/// <value>The creator.</value>
		public Func<TRow, TColumn, TItem> Creator {
			get { return _creator; }
			set { _creator = value; }
		}

		protected Func<TItem, TRow, TColumn, bool> _finder;

		/// <summary>
		/// Function for finding items; Function returns whether the given TItem meets the criteria for a TRow and TColumn intersection.
		/// </summary>
		/// <value>The finder function.</value>
		public Func<TItem, TRow, TColumn, bool> Finder {
			get { return _finder; }
			set { _finder = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Generic.BooleanIntersectList`3"/> class.
		/// </summary>
		public BooleanIntersectList ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Generic.BooleanIntersectList`3"/> class.
		/// </summary>
		/// <param name="itemCollection">Item collection.</param>
		public BooleanIntersectList (IList<TItem> itemCollection) : base (itemCollection)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Generic.BooleanIntersectList`3"/> class.
		/// </summary>
		/// <param name="creator">Function for creating new items.</param>
		/// <param name="finder">Function for finding items; Function returns whether the given TItem meets the criteria for a TRow and TColumn intersection.</param>
		public BooleanIntersectList (IList<TItem> itemCollection, Func<TRow, TColumn, TItem> creator, Func<TItem, TRow, TColumn, bool> finder)
			: base (itemCollection)
		{
			if (creator == null)
				throw new ArgumentNullException ("creator");
			if (finder == null)
				throw new ArgumentNullException ("finder");
			
			this._creator = creator;
			this._finder = finder;
		}


		#region implemented abstract members of BooleanIntersectListBase

		protected override TItem CreateNewItem (TRow row, TColumn column)
		{
			return _creator (row, column);
		}

		protected override int FindIndex (TRow row, TColumn column)
		{
			return _itemCollection.FindIndex (x => _finder (x, row, column));
		}

		#endregion
	}
}

