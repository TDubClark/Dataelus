using System;
using System.Collections.Generic;
using System.Linq;

using Dataelus.Extensions;

namespace Dataelus.UI.CollectionSelector
{
	/// <summary>
	/// Selector controller, with a simplified API - just pass in the collections, call LoadViewInit, and (optionally) get the Saver delegate
	/// </summary>
	public class SimpleController<T> : Controller<T>
	{
		/// <summary>
		/// Constructor for the simplified Selector controller
		/// </summary>
		/// <param name="comparer">Comparer.</param>
		/// <param name="viewObject">View object.</param>
		/// <param name="masterCollection">Master collection of items to display.</param>
		/// <param name="selectedCollection">Selected collection of items already selected (note: this collection will be modified).</param>
		public SimpleController (IEqualityComparer<T> comparer, IView<T> viewObject
			, ICollection<T> masterCollection, ICollection<T> selectedCollection)
			: base (comparer, viewObject)
		{
			LoadController (masterCollection, selectedCollection);
		}

		/// <summary>
		/// Loads the controller.
		/// </summary>
		/// <param name="masterCollection">Master collection.</param>
		/// <param name="selectedCollection">Selected collection.</param>
		protected virtual void LoadController (ICollection<T> masterCollection, ICollection<T> selectedCollection)
		{
			masterCollection.ForEach (x => this.AddItem (x, selectedCollection.Contains (x, this.Comparer)));

			this.Saved += delegate(object sender, EventArgs e) {
				var ctrlr = sender as Controller<T>;
				var userSelected = ctrlr.GetSelected ();  // The items selected by the user

				// If this is a tracked collection, then use the Update feature
				var tracked = selectedCollection as ICollectionTracked<T>;

				if (tracked == null) {
					selectedCollection.Clear ();
					selectedCollection.AddItems (userSelected);
				} else {
					tracked.Update (userSelected);
				}
			};
		}

		/// <summary>
		/// Initializes the widgets and loads the data into the view
		/// </summary>
		/// <param name="viewFields">View fields.</param>
		public virtual void LoadViewInit (params Dataelus.UI.ObjectFieldView[] viewFields)
		{
			this.ViewObject.ControllerObject = this;

			this.ViewObject.InitializeWidgets (viewFields);

			LoadView ();
		}

		/// <summary>
		/// Gets the saver delegate - should be called from a "Save" button or equivalent.
		/// </summary>
		/// <returns>The saver.</returns>
		public Action GetSaver ()
		{
			return delegate {
				this.ViewObject.SaveData ();
			};
		}
	}
}

