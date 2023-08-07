using System;

namespace Dataelus.Grid
{
	/// <summary>
	/// Inteface for a Grid cell
	/// </summary>
	public interface IGridCell
	{
		/// <summary>
		/// Gets or sets the row index.
		/// </summary>
		/// <value>The row.</value>
		int RowIndex { get; set;}

		/// <summary>
		/// Gets or sets the column index.
		/// </summary>
		/// <value>The column.</value>
		int ColumnIndex { get; set; }
	}
}

