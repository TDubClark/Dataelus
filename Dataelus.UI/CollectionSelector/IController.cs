using System;
using System.Collections.Generic;
using System.Linq;

namespace Dataelus.UI.CollectionSelector
{
	/// <summary>
	/// Interface for a Collection Selector controller.
	/// </summary>
	public interface IController<T>
	{
		/// <summary>
		/// Gets or sets the view object.
		/// </summary>
		/// <value>The view object.</value>
		IView<T> ViewObject { get; set; }

		/// <summary>
		/// Saves the selected items according to the unique ID.
		/// </summary>
		/// <param name="items">The collection of Unique IDs, and whether each is selected.</param>
		void SaveSelected (IEnumerable<SelectedID> items);

		/// <summary>
		/// Saves the selected items.
		/// </summary>
		/// <param name="items">The collection of items, and whether each is selected.</param>
		void SaveSelected (IEnumerable<Generic.IdentifiedSelectedListItem<T>> items);

		/// <summary>
		/// Gets the model items.
		/// </summary>
		/// <returns>The model items.</returns>
		Generic.SelectedListItem<T>[] GetModelItems ();
	}

	/// <summary>
	/// Stores whether a given Unique ID is selected for.
	/// </summary>
	public class SelectedID
	{
		public bool Selected { get; set; }

		public long UniqueID { get; set; }

		public SelectedID (bool selected, long uniqueID)
		{
			this.Selected = selected;
			this.UniqueID = uniqueID;
		}
	}

	/// <summary>
	/// Collection Selector controller.
	/// </summary>
	public class Controller<T> : IController<T>
	{
		Generic.IdentifiedSelectedList<T> _model;

		/// <summary>
		/// Gets or sets the model.
		/// </summary>
		/// <value>The model.</value>
		public Generic.IdentifiedSelectedList<T> Model {
			get { return _model; }
			set { _model = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether this instance is saved.
		/// </summary>
		/// <value><c>true</c> if this instance is saved; otherwise, <c>false</c>.</value>
		public bool IsSaved { get; set; }

		public event EventHandler Saved;

		public void OnSaved ()
		{
			if (Saved != null) {
				Saved.Invoke (this, new EventArgs ());
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.CollectionSelector.Controller`1"/> class.
		/// </summary>
		/// <param name="viewObject">View object.</param>
		public Controller (IView<T> viewObject)
			: this (null, viewObject)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.UI.CollectionSelector.Controller`1"/> class.
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <param name="viewObject">View object.</param>
		public Controller (IEqualityComparer<T> comparer, IView<T> viewObject)
		{
			if (viewObject == null)
				throw new ArgumentNullException ("viewObject");
			
			_model = new Dataelus.Generic.IdentifiedSelectedList<T> ();
			this.IsSaved = false;

			this.Comparer = comparer;
			this.ViewObject = viewObject;
		}


		/// <summary>
		/// Adds an item to the model.
		/// </summary>
		/// <param name="item">Item.</param>
		/// <param name="selected">Whether the item is selected.</param>
		public void AddItem (T item, bool selected)
		{
			_model.Add (item, selected);
		}

		/// <summary>
		/// Adds the items to the model.
		/// </summary>
		/// <param name="items">Items.</param>
		public void AddItems (IEnumerable<T> items)
		{
			foreach (var item in items) {
				AddItem (item, false);
			}
		}

		/// <summary>
		/// Gets the selected items.
		/// </summary>
		/// <returns>The selected.</returns>
		public T[] GetSelected ()
		{
			return _model.GetSelected ();
		}

		/// <summary>
		/// Loads the view.
		/// </summary>
		public void LoadView ()
		{
			this.ViewObject.LoadItems (GetIdentifiedItems ());
		}

		/// <summary>
		/// Gets the identified items.
		/// </summary>
		/// <returns>The identified items.</returns>
		Dataelus.Generic.IdentifiedSelectedListItem<T>[] GetIdentifiedItems ()
		{
			return _model.IdentifiedValues
				.Select (x => new Generic.IdentifiedSelectedListItem<T> (x.Value.Item, x.Value.Selected, x.Key))
				.ToArray ();
		}

		#region IController implementation

		public void SaveSelected (IEnumerable<SelectedID> items)
		{
			this.IsSaved = true;
			foreach (var item in items) {
				_model.IdentifiedValues [item.UniqueID].Selected = item.Selected;
			}
			OnSaved ();
		}

		public void SaveSelected (IEnumerable<Dataelus.Generic.IdentifiedSelectedListItem<T>> items)
		{
			this.IsSaved = true;
			foreach (var item in items) {
				_model.IdentifiedValues [item.UniqueID].Selected = item.Selected;
			}
			OnSaved ();
		}

		public Dataelus.Generic.SelectedListItem<T>[] GetModelItems ()
		{
			var lst = new List<Dataelus.Generic.SelectedListItem<T>> ();
			foreach (var item in _model.IdentifiedValues.Values) {
				lst.Add (item);
			}
			return lst.ToArray ();
		}

		public IEqualityComparer<T> Comparer { get; set; }

		public IView<T> ViewObject { get; set; }

		#endregion
	}
}

