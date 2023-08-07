using System;

namespace Dataelus.UI.GridItemViewer.Generic
{
	/// <summary>
	/// Interface for a view.  Grid data type is generic, as is the Grid Issue type.
	/// </summary>
	public interface IView<D, I> : IViewGrid<D, I>, IViewIssueBrowser<I>
	{
	}

	/// <summary>
	/// Interface for a view.  Grid data type is generic, as is the Grid Issue type.
	/// </summary>
	public interface IViewOnly<I> : IViewGridViewOnly, IViewIssueBrowser<I>
	{
	}

	public interface IView<D> : IView<D, Dataelus.TableIssue.ICellIssueItemCollection>
	{
	}
}

