using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus
{
	/// <summary>
	/// List of uniquely identified objects.
	/// </summary>
	public class IdentifiedList
	{
		protected Dictionary<long, object> _identifiedValues;

		/// <summary>
		/// Gets or sets the identified values.
		/// </summary>
		/// <value>The identified values.</value>
		public Dictionary<long, object> IdentifiedValues {
			get { return _identifiedValues; }
			set { _identifiedValues = value; }
		}

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
		/// Initializes a new instance of the <see cref="Dataelus.IdentifiedList"/> class.
		/// </summary>
		public IdentifiedList ()
		{
			_identifiedValues = new Dictionary<long, object> ();
			_idManager = new UniqueIdentifierManager ();
		}

		/// <summary>
		/// Add the specified item to the list, and assigns a unique ID.
		/// </summary>
		/// <param name="item">Item.</param>
		public void Add (object item)
		{
			_identifiedValues.Add (_idManager.GetUniqueID (), item);
		}

		public void AddRange (IEnumerable<object> collection)
		{
			foreach (var item in collection) {
				Add (item);
			}
		}

		/// <summary>
		/// Tries to get the value.
		/// </summary>
		/// <returns><c>true</c>, if get value was found, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="value">Value.</param>
		public bool TryGetValue (long id, out object value)
		{
			return _identifiedValues.TryGetValue (id, out value);
		}

		/// <summary>
		/// Copies the uniquely identified objects and IDs to a list.
		/// </summary>
		/// <returns>The list.</returns>
		public virtual List<IdentifiedObject> ToList ()
		{
			return _identifiedValues
				.Select (item => new IdentifiedObject (item.Key, item.Value))
				.ToList ();
		}

		/// <summary>
		/// Copies the uniquely identified objects and IDs to a list, and sorts the list.
		/// </summary>
		/// <returns>The list sorted.</returns>
		/// <param name="itemSorter">Item sorter.</param>
		public virtual List<IdentifiedObject> ToListSorted (IComparer<object> itemSorter)
		{
			var list = ToList ();

			if (itemSorter != null)
				list.Sort (new IdentifiedObjectComparer<object> (itemSorter));

			return list;
		}
	}

	/// <summary>
	/// List of identified objects.
	/// </summary>
	public class IdentifiedList<T>
	{
		protected Dictionary<long, T> _identifiedValues;

		public Dictionary<long, T> IdentifiedValues {
			get { return _identifiedValues; }
			set { _identifiedValues = value; }
		}

		protected UniqueIdentifierManager _idManager;

		public UniqueIdentifierManager IDManager {
			get { return _idManager; }
			set { _idManager = value; }
		}

		public IdentifiedList ()
		{
			_identifiedValues = new Dictionary<long, T> ();
			_idManager = new UniqueIdentifierManager ();
		}

		/// <summary>
		/// Add the specified item to the list, and assigns a unique ID.
		/// </summary>
		/// <param name="item">Item.</param>
		public virtual void Add (T item)
		{
			_identifiedValues.Add (_idManager.GetUniqueID (), item);
		}

		/// <summary>
		/// Tries to get the value.
		/// </summary>
		/// <returns><c>true</c>, if get value was found, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="value">Value.</param>
		public virtual bool TryGetValue (int id, out T value)
		{
			return _identifiedValues.TryGetValue (id, out value);
		}

		/// <summary>
		/// Tries to get the value.
		/// </summary>
		/// <returns><c>true</c>, if get value was found, <c>false</c> otherwise.</returns>
		/// <param name="id">Identifier.</param>
		/// <param name="value">Value.</param>
		public virtual bool TryGetValue (long id, out T value)
		{
			return _identifiedValues.TryGetValue (id, out value);
		}

		/// <summary>
		/// Copies the uniquely identified objects and IDs to a list.
		/// </summary>
		/// <returns>The list.</returns>
		public virtual List<IdentifiedObject<T>> ToList ()
		{
			return _identifiedValues
				.Select (item => new IdentifiedObject<T> (item.Key, item.Value))
				.ToList ();
		}

		/// <summary>
		/// Copies the uniquely identified objects and IDs to a list, and sorts the list.
		/// </summary>
		/// <returns>The list sorted.</returns>
		/// <param name="itemSorter">Item sorter.</param>
		public virtual List<IdentifiedObject<T>> ToListSorted (IComparer<T> itemSorter)
		{
			var list = ToList ();

			if (itemSorter != null)
				list.Sort (new IdentifiedObjectComparer<T> (itemSorter));

			return list;
		}
	}

	/// <summary>
	/// A comparer (sorter) of uniquely identified objects.
	/// </summary>
	public class IdentifiedObjectComparer<T> : IComparer<IdentifiedObject<T>>
	{
		/// <summary>
		/// Gets or sets the object comparer.
		/// </summary>
		/// <value>The object comparer.</value>
		public IComparer<T> ObjectComparer { get; set; }

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.IdentifiedObjectComparer`1"/> class.
		/// </summary>
		/// <param name="objectComparer">Object comparer.</param>
		public IdentifiedObjectComparer (IComparer<T> objectComparer)
		{
			if (objectComparer == null)
				throw new ArgumentNullException ("objectComparer");
			this.ObjectComparer = objectComparer;
		}

		#region IComparer implementation

		/// <summary>
		/// Compare the specified x and y.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public virtual int Compare (IdentifiedObject<T> x, IdentifiedObject<T> y)
		{
			return ObjectComparer.Compare (x.Item, y.Item);
		}

		#endregion
	}

	/// <summary>
	/// A uniquely identified object.
	/// </summary>
	public class IdentifiedObject : IdentifiedObject<object>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.IdentifiedObject"/> class.
		/// </summary>
		public IdentifiedObject ()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.IdentifiedObject"/> class.
		/// </summary>
		/// <param name="uniqueID">Unique ID.</param>
		/// <param name="item">Item.</param>
		public IdentifiedObject (long uniqueID, object item) : base (uniqueID, item)
		{
		}
	}

	public class IdentifiedObjectCollection : ListBase<IdentifiedObject>
	{
		public IdentifiedObjectCollection ()
		{
		}

		public object FindItem (long id)
		{
			return _items.Find (x => x.UniqueID == id).Item;
		}
	}

	/// <summary>
	/// A uniquely identified object.
	/// </summary>
	public class IdentifiedObject<T> : IIdentifiedItem<T>, IUniqueIdentifier
	{
		protected long _uniqueID;

		public long UniqueID {
			get { return _uniqueID; }
			set { _uniqueID = value; }
		}

		protected T _item;

		public T Item {
			get { return _item; }
			set { _item = value; }
		}

		public IdentifiedObject ()
		{
		}

		public IdentifiedObject (long uniqueID, T item)
		{
			this._uniqueID = uniqueID;
			this._item = item;
		}

		#region IUniqueIdentifier implementation

		long IUniqueIdentifier.ObjectUniqueID {
			get { return _uniqueID; }
			set { _uniqueID = value; }
		}

		#endregion
	}

	public interface IIdentifiedItem<T>
	{
		long UniqueID { get; set; }

		T Item { get; set; }
	}
}

