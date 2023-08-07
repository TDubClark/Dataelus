using System;

namespace Dataelus.TableIssue
{
	using Dataelus.Grid;

	/// <summary>
	/// Represents a single Issue Item
	/// </summary>
	public interface ICellIssueItem : IIssue
	{
		/// <summary>
		/// Gets the grid cell.
		/// </summary>
		/// <value>The grid cell.</value>
		IGridCell GridCell { get; }
	}
}

