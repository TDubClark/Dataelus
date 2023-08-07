using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	public class UniqueListBase<T> : ListBase<T> where T : IUniqueIdentifier
	{
		protected UniqueIdentifierManager _idManager;

		/// <summary>
		/// Gets or sets the identifier manager.
		/// </summary>
		/// <value>The identifier manager.</value>
		public UniqueIdentifierManager IDManager {
			get { return _idManager; }
			set { _idManager = value; }
		}

		/// <summary>
		/// Initializes this instance.
		/// </summary>
		protected void Init ()
		{
			_idManager = new UniqueIdentifierManager ();
		}

		public UniqueListBase ()
		{
			Init ();
		}

		public UniqueListBase (IEnumerable<T> collection) : base (collection)
		{
			Init ();
		}

		public UniqueListBase (params T[] items) : base (items)
		{
			Init ();
		}

		public UniqueListBase (int capacity) : base (capacity)
		{
			Init ();
		}

		/// <summary>
		/// Adds the item and assigns a new Unique ID.
		/// </summary>
		/// <returns>The item with an assigned ID.</returns>
		/// <param name="item">The item, with no ID assigned.</param>
		public virtual T AddAssignID (T item)
		{
			Add (item);
			return item;
		}

		/// <Docs>The item to add to the current collection.</Docs>
		/// <para>Adds an item to the current collection.</para>
		/// <remarks>To be added.</remarks>
		/// <exception cref="System.NotSupportedException">The current collection is read-only.</exception>
		/// <summary>
		/// Add the specified item.
		/// </summary>
		/// <param name="item">Item.</param>
		public override void Add (T item)
		{
			item.ObjectUniqueID = GetNewUniqueObjectID ();
			base.Add (item);
		}

		/// <summary>
		/// Insert the specified item at the specified index.
		/// </summary>
		/// <param name="index">Index.</param>
		/// <param name="item">Item.</param>
		public override void Insert (int index, T item)
		{
			item.ObjectUniqueID = GetNewUniqueObjectID ();
			base.Insert (index, item);
		}

		/// <summary>
		/// Finds the index by the unique ID.
		/// </summary>
		/// <returns>The index by unique ID.</returns>
		/// <param name="objectUniqueID">The unique object ID.</param>
		public virtual int FindIndexByUniqueID (long objectUniqueID)
		{
			return _items.FindIndex (x => x.ObjectUniqueID == objectUniqueID);
		}

		/// <summary>
		/// Gets whether the given object unique ID is in use.
		/// </summary>
		/// <returns><c>true</c>, if in use, <c>false</c> otherwise.</returns>
		/// <param name="objectUniqueID">Object unique ID.</param>
		protected virtual bool InUseUniqueID (long objectUniqueID)
		{
			return _items.Any (x => x.ObjectUniqueID == objectUniqueID);
		}

		/// <summary>
		/// Gets a new unique object ID.
		/// </summary>
		/// <returns>The unique object ID.</returns>
		protected long GetNewUniqueObjectID ()
		{
			long id;
			do {
				id = _idManager.GetUniqueID ();
			}
			while (InUseUniqueID (id));

			return id;
		}
	}
}

