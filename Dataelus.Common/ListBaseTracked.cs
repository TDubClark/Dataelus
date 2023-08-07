using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// A base List class, where Add/Insert/Remove changes are tracked.
	/// </summary>
	public class ListBaseTracked<T> : ListBase<T>, ICollectionTracked<T>, ICollectionTrackable
	{
		protected bool _isTracking;

		/// <summary>
		/// Gets or sets a value indicating whether this instance is tracking changes.
		/// </summary>
		/// <value><c>true</c> if this instance is tracking; otherwise, <c>false</c>.</value>
		public bool IsTracking {
			get { return _isTracking; }
			set { _isTracking = value; }
		}

		public virtual void ResetTracking ()
		{
			_tracker.ResetTracking ();
			_isTracking = false;
		}

		public virtual void StartTracking ()
		{
			if (!_isTracking) {
				_tracker.ResetTracking ();
				_isTracking = true;
			}
		}

		public virtual void StopTracking ()
		{
			ResetTracking ();
		}

		protected IEqualityComparer<T> _comparer;

		/// <summary>
		/// Gets or sets the equality comparer.
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<T> Comparer {
			get { return _comparer; }
			set { _comparer = value; }
		}

		protected ListItemTracker<T> _tracker;

		/// <summary>
		/// Gets or sets the tracker for items added and removed.
		/// </summary>
		/// <value>The tracker.</value>
		public ListItemTracker<T> Tracker {
			get { return _tracker; }
			set { _tracker = value; }
		}

		public ListBaseTracked (IEqualityComparer<T> comparer)
		{
			if (comparer == null)
				throw new ArgumentNullException ("comparer");
			
			_comparer = comparer;
			_tracker = new ListItemTracker<T> (comparer);
			_isTracking = false;
		}

		/// <summary>
		/// Removes the given items from the list.
		/// </summary>
		/// <param name="items">The items which will be removed.</param>
		public void RemoveItems (IEnumerable<T> items)
		{
			RemoveItems (items, _comparer);
		}

		#region Overrides

		public override void Add (T item)
		{
			if (_isTracking)
				_tracker.TrackItemAdded (item);
			base.Add (item);
		}

		public override void Insert (int index, T item)
		{
			if (_isTracking)
				_tracker.TrackItemAdded (item);
			base.Insert (index, item);
		}

		public override bool Remove (T item)
		{
			if (base.Remove (item)) {
				if (_isTracking)
					_tracker.TrackItemRemoved (item);
				return true;
			}
			return false;
		}

		public override void RemoveAt (int index)
		{
			if (_isTracking)
				_tracker.TrackItemRemoved (_items [index]);
			base.RemoveAt (index);
		}

		public override void Clear ()
		{
			if (_isTracking)
				_tracker.TrackItemsRemoved (_items);
			base.Clear ();
		}

		#endregion

		#region Implementation for ICollectionTracked<T>

		/// <summary>
		/// Updates this collection to match the given list.
		/// </summary>
		/// <param name="newCollection">The new collection of items.</param>
		public void Update (IEnumerable<T> newCollection)
		{
			// Remove any items NOT in the new collection
			RemoveItems (_items.Except (newCollection, _comparer).ToArray ());

			// Add any items from the new collection, except those which are already in this list
			AddItems (newCollection.Except (_items, _comparer).ToArray ());
		}

		#endregion
	}

	/// <summary>
	/// State of a Tracking flag.
	/// </summary>
	public enum TrackingFlagState
	{
		/// <summary>
		/// The flag is not set
		/// </summary>
		NotSet,

		/// <summary>
		/// The list has changed since the flag was set
		/// </summary>
		Changed,

		/// <summary>
		/// The list has not changed since the flag was set
		/// </summary>
		NotChanged,

		/// <summary>
		/// Change tracking was reset since the flag was set
		/// </summary>
		Reset
	}

	/// <summary>
	/// The status on whether an object has changed.
	/// </summary>
	public enum ChangedStatus
	{
		/// <summary>
		/// The object has changed.
		/// </summary>
		Changed,

		/// <summary>
		/// The object has not changed.
		/// </summary>
		Unchanged,

		/// <summary>
		/// The state of the object is unknown, or cannot be determined.
		/// </summary>
		Unknown
	}

	/// <summary>
	/// List item tracker.
	/// </summary>
	public class ListItemTracker<T>
	{
		protected List<T> _addedItems;
		protected List<T> _removedItems;

		/// <summary>
		/// Gets or sets the added items.
		/// </summary>
		/// <value>The added items.</value>
		public List<T> AddedItems {
			get { return _addedItems; }
			set { _addedItems = value; }
		}

		/// <summary>
		/// Gets or sets the removed items.
		/// </summary>
		/// <value>The removed items.</value>
		public List<T> RemovedItems {
			get { return _removedItems; }
			set { _removedItems = value; }
		}

		protected IEqualityComparer<T> _comparer;

		/// <summary>
		/// Gets or sets the equality comparer.
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<T> Comparer {
			get { return _comparer; }
			set { _comparer = value; }
		}

		protected TrackingFlagState _flagState;

		/// <summary>
		/// Sets the flag (to unchanged); flag gets updated when a change occurs.
		/// </summary>
		public void SetFlag ()
		{
			_flagState = TrackingFlagState.NotChanged;
		}

		/// <summary>
		/// Clears the flag.
		/// </summary>
		public void ClearFlag ()
		{
			_flagState = TrackingFlagState.NotSet;
		}

		/// <summary>
		/// Determines the flag status; returns true if the flag status can be determined.
		/// </summary>
		/// <returns><c>true</c>, if flag status was determined, <c>false</c> otherwise.</returns>
		/// <param name="isChanged">Outputs whether the flag is marked changed (true) or unchanged (false).</param>
		public bool DetermineFlagStatus (out bool isChanged)
		{
			switch (_flagState) {
				case TrackingFlagState.Changed:
					isChanged = true;
					return true;
				case TrackingFlagState.NotChanged:
					isChanged = false;
					return true;
			}

			isChanged = false;
			return false;
		}

		/// <summary>
		/// Determines the flag status.
		/// </summary>
		/// <returns>The flag status.</returns>
		public ChangedStatus DetermineFlagStatus ()
		{
			bool isChanged;
			if (DetermineFlagStatus (out isChanged)) {
				if (isChanged)
					return ChangedStatus.Changed;
				return ChangedStatus.Unchanged;
			}
			return ChangedStatus.Unknown;
		}

		/// <summary>
		/// Flags the change.
		/// </summary>
		protected void FlagChange ()
		{
			if (_flagState == TrackingFlagState.NotChanged)
				_flagState = TrackingFlagState.Changed;
		}

		/// <summary>
		/// Resets the tracking.
		/// </summary>
		public virtual void ResetTracking ()
		{
			if (_addedItems == null)
				_addedItems = new List<T> ();
			if (_removedItems == null)
				_removedItems = new List<T> ();

			_addedItems.Clear ();
			_removedItems.Clear ();
			_flagState = TrackingFlagState.Reset;
		}

		/// <summary>
		/// Whether the Removed list contains the given item.
		/// </summary>
		/// <returns><c>true</c>, if Removed list contains the given item, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		public virtual bool RemovedContains (T item, out int index)
		{
			index = _removedItems.FindIndex (x => _comparer.Equals (x, item));
			return (index >= 0);
		}

		/// <summary>
		/// Whether the Added list contains the given item.
		/// </summary>
		/// <returns><c>true</c>, if Added list contains the given item, <c>false</c> otherwise.</returns>
		/// <param name="item">Item.</param>
		public virtual bool AddedContains (T item, out int index)
		{
			index = _addedItems.FindIndex (x => _comparer.Equals (x, item));
			return (index >= 0);
		}

		/// <summary>
		/// Tracks the given item added to the list.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void TrackItemAdded (T item)
		{
			FlagChange ();

			int index;
			if (RemovedContains (item, out index)) {
				_removedItems.RemoveAt (index);
			} else {
				_addedItems.Add (item);
			}
		}

		/// <summary>
		/// Tracks the given item removed from the list.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void TrackItemRemoved (T item)
		{
			FlagChange ();

			int index;
			if (AddedContains (item, out index)) {
				_addedItems.RemoveAt (index);
			} else {
				_removedItems.Add (item);
			}
		}

		public void TrackItemsRemoved (IEnumerable<T> items)
		{
			foreach (var item in items) {
				TrackItemRemoved (item);
			}
		}

		public ListItemTracker (IEqualityComparer<T> comparer)
		{
			ClearFlag ();

			_comparer = comparer;
			_addedItems = new List<T> ();
			_removedItems = new List<T> ();
		}
	}
}

