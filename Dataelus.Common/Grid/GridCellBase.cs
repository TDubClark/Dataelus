using System;

namespace Dataelus.Grid
{
	/// <summary>
	/// Base implementation of IGridCell.
	/// </summary>
	public class GridCellBase : IGridCell
	{
		protected int _rowIndex;

		protected int _columnIndex;

		public GridCellBase ()
			: this (-1, -1)
		{

		}

		public GridCellBase (int row, int column)
		{
			_rowIndex = row;
			_columnIndex = column;
		}

		#region IGridCell implementation

		public int RowIndex {
			get { return _rowIndex; }
			set { _rowIndex = value; }
		}

		public int ColumnIndex {
			get { return _columnIndex; }
			set { _columnIndex = value; }
		}

		#endregion
	}
}

