using System;

namespace Dataelus.UI.CollectionSelector
{
	/// <summary>
	/// Interface for a Collection Selector view.
	/// </summary>
	public interface IView<T>
	{
		/// <summary>
		/// Gets or sets the controller object.
		/// </summary>
		/// <value>The controller object.</value>
		IController<T> ControllerObject { get; set; }

		/// <summary>
		/// Initializes the widgets.
		/// </summary>
		/// <param name="fields">Fields.</param>
		void InitializeWidgets (ObjectFieldView[] fields);

		/// <summary>
		/// Loads the items into the view.
		/// </summary>
		/// <param name="items">Items.</param>
		void LoadItems (System.Collections.Generic.IEnumerable<Generic.IdentifiedSelectedListItem<T>> items);

		/// <summary>
		/// Saves the data.
		/// </summary>
		void SaveData ();
	}
}

