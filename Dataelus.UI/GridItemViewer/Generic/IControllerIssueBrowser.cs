using System;

namespace Dataelus.UI.GridItemViewer.Generic
{
	/// <summary>
	/// Interface for a controller of an issue browser.
	/// </summary>
	public interface IControllerIssueBrowser<I>
	{
		IViewIssueBrowser<I> IssueView { get; set; }

		/// <summary>
		/// When the user selectes and item.
		/// </summary>
		/// <param name="objectId">Object identifier.</param>
		void UserItemSelected (int objectId);
	}
}

