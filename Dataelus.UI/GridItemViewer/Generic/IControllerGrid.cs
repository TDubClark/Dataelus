using System;

namespace Dataelus.UI.GridItemViewer.Generic
{
	public interface IControllerGrid<D, I>
	{
		/// <summary>
		/// Gets or sets the view.
		/// </summary>
		/// <value>The view.</value>
		IViewGrid<D, I> GridView { get; set; }

		/// <summary>
		/// Gets or sets the item data object.
		/// </summary>
		/// <value>The item data object.</value>
		I ItemDataObject { get; set; }

		/// <summary>
		/// Gets or sets the grid data object.
		/// </summary>
		/// <value>The grid data object.</value>
		D GridDataObject { get; set; }

		/// <summary>
		/// Loads the view data.
		/// </summary>
		void LoadViewData ();
	}

	public interface IControllerGridViewOnly<I>
	{
		/// <summary>
		/// Gets or sets the view.
		/// </summary>
		/// <value>The view.</value>
		IViewGridViewOnly GridView { get; set; }

		/// <summary>
		/// Gets or sets the item data object.
		/// </summary>
		/// <value>The item data object.</value>
		I ItemDataObject { get; set; }

		/// <summary>
		/// Loads the view data.
		/// </summary>
		void LoadViewData ();
	}

	public interface IControllerGridPaged<D, I>
	{
		/// <summary>
		/// Gets or sets the view.
		/// </summary>
		/// <value>The view.</value>
		IViewGridPaged<D, I> GridView { get; set; }

		/// <summary>
		/// Gets or sets the item data object.
		/// </summary>
		/// <value>The item data object.</value>
		I ItemDataObject { get; set; }

		/// <summary>
		/// Gets or sets the grid data object.
		/// </summary>
		/// <value>The grid data object.</value>
		D GridDataObject { get; set; }

		/// <summary>
		/// Loads the view data.
		/// </summary>
		void LoadViewData ();
	}

	public interface IControllerGridViewerPaged<I>
	{
		/// <summary>
		/// Gets or sets the view.
		/// </summary>
		/// <value>The view.</value>
		IViewPagedGridViewOnly GridView { get; set; }

		/// <summary>
		/// Gets or sets the item data object.
		/// </summary>
		/// <value>The item data object.</value>
		I ItemDataObject { get; set; }

		/// <summary>
		/// Loads the view data.
		/// </summary>
		void LoadViewData ();
	}
}

