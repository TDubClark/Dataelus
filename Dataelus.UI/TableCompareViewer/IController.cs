using System;
using System.Collections.Generic;

namespace Dataelus.UI.TableCompareViewer
{
	public interface IController<T, R>
	{
		/// <summary>
		/// Gets or sets the view.
		/// </summary>
		/// <value>The view.</value>
		IView<T, R> View{get;set;}

		/// <summary>
		/// Gets or sets the item data object.
		/// </summary>
		/// <value>The item data object.</value>
		List<Dataelus.Table.Comparison.IssueItem> ItemDataObject{ get; set; }

		/// <summary>
		/// Loads the view data.
		/// </summary>
		void LoadViewData();

		/// <summary>
		/// When the user selects an item.
		/// </summary>
		/// <param name="objectId">Object identifier.</param>
		void UserItemSelected (int objectId);
	}
}

