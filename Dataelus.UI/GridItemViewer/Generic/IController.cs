using System;

namespace Dataelus.UI.GridItemViewer.Generic
{
	/// <summary>
	/// Interface for a controller, which views issues (collection of type I) on a grid (data of type D).
	/// </summary>
	public interface IController<D, I> : IControllerGrid<D, I>, IControllerIssueBrowser<I>
	{
		IView<D, I> View { get; set; }
	}

	/// <summary>
	/// Interface for a controller, which views issues (collection of type I) on a grid (data of type D).
	/// </summary>
	public interface IControllerViewOnly<I> : IControllerGridViewOnly<I>, IControllerIssueBrowser<I>
	{
		IViewOnly<I> View { get; set; }
	}

	// Type over-load
	public interface IController<D> : IController<D, Dataelus.TableIssue.ICellIssueItemCollection>
	{
	}
}

