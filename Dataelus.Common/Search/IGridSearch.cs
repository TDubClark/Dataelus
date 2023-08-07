using System;

namespace Dataelus.Search
{
	/// <summary>
	/// Interface for a grid search.
	/// </summary>
	public interface IGridSearch<T> : ICollectionSearchable<T> where T : Dataelus.Grid.IGridCellText
	{
		bool Search (ISearcherPriorityCollection searchers, out int[] rowIndex, out int[] columnIndex);
	}
}

