using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Extensions;
using Dataelus.Table;

namespace Dataelus
{
	/*
	 * This is a Model object (Model-View-Controller pattern) for editing a generic collection
	 * The assumptions are:
	 *  - Allow the user to edit a collection of objects of type T
	 *  - where the properties of T are all basic types, editable as text.
	 * 
	 * 
	 * The constraint:
	 * 		where T : new()
	 * requires the type T to have a parameter-less constructor
	 */

	/// <summary>
	/// Collection editor model; creates an ObjectTable for a ViewModel, and uses the events to affect the original collection.
	/// </summary>
	public class CollectionEditor<T> where T : new()
	{
		protected IList<T> _collection;

		/// <summary>
		/// Gets or sets the collection.
		/// </summary>
		/// <value>The collection.</value>
		public IList<T> Collection {
			get { return _collection; }
			protected set {
				_collection = value;
				this.Table = _collection.ToObjectTable (true);
			}
		}

		protected IEqualityComparer<T> _comparer;

		/// <summary>
		/// Gets or sets the comparer.
		/// </summary>
		/// <value>The comparer.</value>
		public IEqualityComparer<T> Comparer {
			get { return _comparer; }
			set { _comparer = value; }
		}

		protected ObjectTable _table;

		/// <summary>
		/// Gets or sets the table.
		/// </summary>
		/// <value>The table.</value>
		public ObjectTable Table {
			get { return _table; }
			protected set {
				_table = value;
				_table.RowAdded += _table_RowAdded;
				_table.RowDeleted += _table_RowDeleted;
				_table.RowChanged += _table_RowChanged;
			}
		}

		/// <summary>
		/// Occurs when an item is added.
		/// </summary>
		public event ListItemAddedEventHandler<T> ItemAdded;

		/// <summary>
		/// Raises the item added event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		protected void OnItemAdded (ListItemAddedEventArgs<T> args)
		{
			if (ItemAdded != null)
				ItemAdded (this, args);
		}

		/// <summary>
		/// Occurs when an item is changed.
		/// </summary>
		public event ListItemPropertyChangedEventHandler<T> ItemChanged;

		/// <summary>
		/// Raises the item changed event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		protected void OnItemChanged (ListItemPropertyChangedEventArgs<T> args)
		{
			if (ItemChanged != null)
				ItemChanged (this, args);
		}

		/// <summary>
		/// Occurs when an item is removed.
		/// </summary>
		public event ListItemRemovedEventHandler<T> ItemRemoved;

		/// <summary>
		/// Raises the item removed event.
		/// </summary>
		/// <param name="args">Arguments.</param>
		protected void OnItemRemoved (ListItemRemovedEventArgs<T> args)
		{
			if (ItemRemoved != null)
				ItemRemoved (this, args);
		}

		void _table_RowChanged (object sender, ObjectRowChangedEventArgs args)
		{
			var item = (T)args.RowChanged.CustomTag;
			int index = _collection.FindIndex (x => _comparer.Equals (x, item));

			OnItemChanged (new ListItemPropertyChangedEventArgs<T> (item, args.NewValue, args.PriorValue, index));

			System.Reflection.PropertyInfo[] props = typeof(T).GetProperties ();
			var nameComparer = new StringEqualityComparer ();

			// Change the property of the appropriate value
			foreach (var prop in props) {
				if (nameComparer.Equals (prop.Name, args.ColumnChanged.ColumnName)) {
					prop.SetValue (item, args.NewValue, null);
					break;
				}
			}
		}

		void _table_RowAdded (object sender, ObjectRowAddedEventArgs args)
		{
			T obj = new T ();
			// Call the event handler; allow the user to change the value
			var args2 = new ListItemAddedEventArgs<T> (obj, args.RowIndex);
			OnItemAdded (args2);
			obj = args2.NewValue;

			args.RowAdded.CustomTag = obj;
			_collection.Add (obj);
		}

		void _table_RowDeleted (object sender, ObjectRowDeletedEventArgs args)
		{
			var item = (T)args.RowDeleted.CustomTag;
			int index = _collection.FindIndex (x => _comparer.Equals (x, item));

			OnItemRemoved (new ListItemRemovedEventArgs<T> (item, index));

			// Per the creation of the object table (see the constructor), the row Custom Tag is a pointer to the object
			_collection.Remove ((T)args.RowDeleted.CustomTag);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.CollectionEditor`1"/> class.
		/// </summary>
		/// <param name="collection">Collection.</param>
		public CollectionEditor (IList<T> collection)
		{
			this.Collection = collection;
		}
	}
}

