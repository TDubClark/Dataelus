using System;
using System.Collections.Generic;

namespace Dataelus.Grid
{
	public enum GridSortOrder
	{
		RowFirst,
		ColumnFirst
	}

	public enum SortOrder
	{
		Ascending,
		Descending
	}

	public class IGridCellComparer : IComparer<IGridCell>
	{
		public GridSortOrder Order {
			get;
			set;
		}

		public SortOrder RowOrder {
			get;
			set;
		}

		public SortOrder ColumnOrder {
			get;
			set;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Dataelus.Grid.IGridCellComparer"/> class.
		/// Default: Sort by Row Ascending, then Column Ascending
		/// </summary>
		public IGridCellComparer ()
		{
			this.Order = GridSortOrder.RowFirst;
			this.RowOrder = SortOrder.Ascending;
			this.ColumnOrder = SortOrder.Ascending;
		}

		protected int CompareColumn (int columnX, int columnY)
		{
			return CompareValues (columnX, columnY, this.ColumnOrder);
		}

		protected int CompareRow (int rowX, int rowY)
		{
			return CompareValues (rowX, rowY, this.RowOrder);
		}

		static int CompareValues (int x, int y, SortOrder order)
		{
			switch (order) {
			case SortOrder.Ascending:
				return x.CompareTo (y);
			case SortOrder.Descending:
				return y.CompareTo (x);
			default:
				throw new Exception (String.Format ("Unrecognized sort order: {0}", order));
			}
		}

		#region IComparer implementation

		/// <summary>
		/// Compare the specified x and y.
		/// </summary>
		/// <param name="x">The x coordinate.</param>
		/// <param name="y">The y coordinate.</param>
		public int Compare (IGridCell x, IGridCell y)
		{
			int result = 0;

			int rowResult = CompareRow (x.RowIndex, y.RowIndex);
			int columnResult = CompareColumn (x.ColumnIndex, y.ColumnIndex);

			switch (this.Order) {
			case GridSortOrder.ColumnFirst:
				result = columnResult;
				if (result == 0)
					result = rowResult;
				break;
			case GridSortOrder.RowFirst:
				result = rowResult;
				if (result == 0)
					result = columnResult;
				break;
			default:
				throw new Exception (String.Format ("Unrecognized grid sort order: {0}", this.Order));
			}

			return result;
		}

		#endregion
	}
}

