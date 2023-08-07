using System;

namespace Dataelus.Grid
{
	/// <summary>
	/// Grid cell object, which has a text property.
	/// </summary>
	public class GridCellText : GridCellBase, IGridCellText
	{
		protected string _text;

		public GridCellText (GridCellText other)
			: this (other.RowIndex, other.ColumnIndex, other.Text)
		{
		}

		public GridCellText ()
			: base ()
		{
		}

		public GridCellText (int row, int column, string cellText)
			: base (row, column)
		{
			_text = cellText;
		}

		public string Text {
			get { return _text; }
			set { _text = value; }
		}
	}
}

