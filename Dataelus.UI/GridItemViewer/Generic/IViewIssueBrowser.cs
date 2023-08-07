using System;

namespace Dataelus.UI.GridItemViewer.Generic
{
	/// <summary>
	/// Interface for a view object, used for browsing issues.
	/// </summary>
	public interface IViewIssueBrowser<I>
	{
		/// <summary>
		/// Gets or sets the browser controller.
		/// </summary>
		/// <value>The browser controller.</value>
		IControllerIssueBrowser<I> BrowserController{ get; set; }

		/// <summary>
		/// Loads the items.
		/// </summary>
		/// <param name="data">Data.</param>
		void LoadItems (I data);
	}
}

