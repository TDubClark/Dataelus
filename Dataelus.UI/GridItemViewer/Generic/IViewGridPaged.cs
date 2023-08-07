using System;

namespace Dataelus.UI.GridItemViewer.Generic
{
	public interface IViewGridPaged<D, I> : IViewGrid<D, I>
	{
		/// <summary>
		/// Sets the page.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		void SetPage(int pageIndex);

		/// <summary>
		/// Gets the current page index.
		/// </summary>
		/// <returns>The current page index.</returns>
		int GetCurrentPageIndex();
	}
}

